using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using static UnityEngine.UI.Extensions.ContentScrollSnapHorizontal.MoveInfo;

public class GeneratePolynomialTreeMap : MonoBehaviour
{
    // Root, Boss 타입은 트리에 단 하나만 존재
    // 트리 가지의 수를 최소 1개, 최대 3개로 고정하고
    // 가지가 2개, 3개일 확률을 조정함
    public TreeNodeObject root;
    public TreeNodeObject boss;
    public int height = 8;
    public int width = 6;

    public int normalCount = 1;
    public int threatCount = 1;
    public int supplyCount = 1;
    public int eventCount = 1;
    private int localNormalCount = 1;
    private int localThreatCount = 1;
    private int localSupplyCount = 1;
    private int localEventCount = 1;

    public float branch2Prob = 0.25f;
    public float branch3Prob = 0.1f;

    public GameObject nodeBundlePrefab;
    public GameObject treeNodePrefab;
    public GameObject uiLineRendererPrefab;
    public Transform nodeTarget;
    public Transform lineRendererTarget;

    private List<GameObject> bundles = new();
    private List<List<TreeNodeObject>> nodes = new();
    private List<GameObject> lines = new();
    private List<List<(TreeNodeTypes nodeType, int branchCount)>> blueprint = new();
    private int nodeIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("new graph");
            CreateTreeGraph();
        }
    }

    public void CreateTreeGraph()
    {
        nodeIndex = 0;
        DestroyAllObjs();
        CreateBundles(height - 2);      // 트리의 깊이 설정
        CreateBlueprint();              // 설계도 생성
        CreateNodes();                  // 노드 생성
        StartCoroutine(CoLinkNodes());  // 노드 연결
    }

    private void CreateBlueprint()
    {
        localNormalCount = normalCount;
        localThreatCount = threatCount;
        localSupplyCount = supplyCount;
        localEventCount = eventCount;

        blueprint = new(height);
        for (int i = 0; i < height; i++)
        {
            int branchWidth = 0;
            TreeNodeTypes type;
            int branchCount = 1;
            bool fixBranchCount = false;
            if (i == 0) // Root
            {
                branchWidth = 1;
                branchCount = 2;
                fixBranchCount = true;
                type = TreeNodeTypes.Root;
            }
            else if (i == height - 1) // Boss
            {
                branchWidth = 1;
                branchCount = 0;
                fixBranchCount = true;
                type = TreeNodeTypes.Boss;
            }
            else
            {
                type = TreeNodeTypes.None;
                foreach (var elem in blueprint[i - 1])
                {
                    branchWidth += elem.branchCount;
                }
                if (branchWidth > width)
                    branchWidth = width;
            }
            blueprint.Add(new(branchWidth));
            for (int j = 0; j < branchWidth; j++)
            {
                if (!fixBranchCount)
                {
                    float randNum = Random.Range(0f, 1f);
                    branchCount =
                        randNum < branch3Prob ? 3 :
                        randNum < branch2Prob ? 2 : 1;
                }
                blueprint[i].Add((type != TreeNodeTypes.None ? type : SelectType(), branchCount));
            }
        }
    }

    private void CreateNodes()
    {
        for (int i = 0; i < height; i++)
        {
            int number = 0;
            foreach ((TreeNodeTypes type, int branchCount) in blueprint[i])
            {
                CreateNewNodeInstance(bundles[i].transform, i, number++, $"{type}{nodeIndex++}", type);
            }
        }
        root = nodes[0][0].GetComponent<TreeNodeObject>();
        boss = nodes[height - 1][0].GetComponent<TreeNodeObject>();
    }

    private TreeNodeTypes SelectType()
    {
        List<TreeNodeTypes> pool = new();
        if (localNormalCount > 0)
            pool.Add(TreeNodeTypes.Normal);
        if (localThreatCount > 0)
            pool.Add(TreeNodeTypes.Threat);
        if (localSupplyCount > 0)
            pool.Add(TreeNodeTypes.Supply);
        if (localEventCount > 0)
            pool.Add(TreeNodeTypes.Event);

        TreeNodeTypes returnType;
        if (pool.Count == 0)
            returnType = (TreeNodeTypes)Random.Range(2, 5);
        else
        {
            returnType = pool[Random.Range(0, pool.Count)];
            if (returnType == TreeNodeTypes.Normal)
                localNormalCount--;
            else if (returnType == TreeNodeTypes.Threat)
                localThreatCount--;
            else if (returnType == TreeNodeTypes.Supply)
                localSupplyCount--;
            else if (returnType == TreeNodeTypes.Event)
                localEventCount--;
            pool.Clear();
        }

        return returnType;
    }

    private IEnumerator CoLinkNodes()
    {
        // UI Layout Group에서 포지션을 배치하기 위한 시간이 필요해서 코루틴으로 실행 시간 차이를 줌
        yield return null;
        int nodesLength = nodes.Count;
        for (int i = 0; i < nodesLength - 1; i++)
        {
            int bundleLength = nodes[i].Count;
            for (int j = 0; j < bundleLength; j++)
            {
                LinkNodes(nodes[i][j]);
            }
        }
    }

    private void LinkNodes(TreeNodeObject parent)
    {
        int hIndex = parent.floor;
        int wIndex = parent.number;
        if (hIndex == height - 2)
        {
            parent.AddChildren(boss);
            GameObject lrObj = Instantiate(uiLineRendererPrefab, lineRendererTarget);
            lines.Add(lrObj);
            UILineRenderer lr = lrObj.GetComponent<UILineRenderer>();
            lr.Points[0] = lineRendererTarget.InverseTransformPoint(parent.tail.position);
            lr.Points[1] = lineRendererTarget.InverseTransformPoint(boss.head.position);
            lr.enabled = true;
            Debug.Log($"{parent.data} 보스 연결");
            return;
        }

        (_, int branchCount) = blueprint[hIndex][wIndex];
        int curBranchCount = blueprint[hIndex].Count;
        int nextBranchCount = blueprint[hIndex + 1].Count;
        int childStartIndex = wIndex;
        if (branchCount > 1 && wIndex != 0)
        {
            if (wIndex == 0)
            {
                childStartIndex = 0;
                Debug.Log($"{parent.data} 1번 가지 수:{branchCount}");
            }
            else if (wIndex == nextBranchCount - 1)
            {
                childStartIndex = 2 * nextBranchCount - branchCount - curBranchCount;
                Debug.Log($"{parent.data} 2번 가지 수:{branchCount}");
            }
            else
            {
                Debug.Log($"{parent.data} 3번 가지 수:{branchCount}");
                childStartIndex += (1 - branchCount + (nextBranchCount == wIndex + 1 ? 0 : Random.Range(0, 2)));
            }
        }
        else if (curBranchCount < nextBranchCount && wIndex == nextBranchCount - 1)
        {
            childStartIndex = 2 * nextBranchCount - branchCount - curBranchCount;
            Debug.Log($"{parent.data} 5번 가지 수:{branchCount}");
        }
        else
        {
            Debug.Log($"{parent.data} 4번 가지 수:{branchCount}");
        }

        for (int j = 0; j < branchCount; j++)
        {
            TreeNodeObject child = nodes[hIndex + 1][childStartIndex + j];
            parent.AddChildren(child);

            GameObject lrObj = Instantiate(uiLineRendererPrefab, lineRendererTarget);
            lines.Add(lrObj);
            UILineRenderer lr = lrObj.GetComponent<UILineRenderer>();
            lr.Points[0] = lineRendererTarget.InverseTransformPoint(parent.tail.position);
            lr.Points[1] = lineRendererTarget.InverseTransformPoint(child.head.position);
            lr.enabled = true;
        }
    }

    private GameObject CreateNewNodeInstance(Transform target, int hIndex, int wIndex, string data, TreeNodeTypes type)
    {
        GameObject instanceNode = Instantiate(treeNodePrefab, target);
        TreeNodeObject node = instanceNode.GetComponent<TreeNodeObject>();
        node.SetInit(data, hIndex, wIndex, type);
        nodes[hIndex].Add(node);
        return instanceNode;
    }

    public void CreateBundles(int depth)
    {
        for (int i = 0; i < depth + 2; i++)
        {
            GameObject bundle = Instantiate(nodeBundlePrefab, nodeTarget);
            bundle.name = $"bundle_{i}";
            nodes.Add(new());
            bundles.Add(bundle);
        }
    }

    public void DestroyAllObjs()
    {
        int ncount = nodes.Count;
        for (int i = 0; i < ncount; i++)
        {
            if (nodes[i] != null)
            {
                int rowCount = nodes[i].Count;
                for (int j = 0; j < rowCount; j++)
                    Destroy(nodes[i][j]);
                nodes[i].Clear();
            }
        }
        nodes.Clear();

        int bpcount = blueprint.Count;
        for (int i = 0; i < bpcount; i++)
        {
            blueprint[i]?.Clear();
        }
        blueprint.Clear();

        int bcount = bundles.Count;
        for (int i = 0; i < bcount; i++)
        {
            if (bundles[i] != null)
                Destroy(bundles[i]);
        }
        bundles.Clear();

        int lcount = lines.Count;
        for (int i = 0; i < lcount; i++)
        {
            if (lines[i] != null)
                Destroy(lines[i]);
        }
        lines.Clear();
    }
}