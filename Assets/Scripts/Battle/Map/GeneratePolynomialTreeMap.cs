using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class GeneratePolynomialTreeMap : MonoBehaviour
{
    // Root, Boss 타입은 트리에 단 하나만 존재
    // 트리 가지의 수를 최소 1개, 최대 3개로 고정하고 2, 3개의 확률을 조정함
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
    private List<List<(TreeNodeTypes nodeType, int branchCount)>> blueprint;

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
        DestroyAllObjs();
        CreateBundles(height - 2);  // 트리의 깊이 설정
        CreateBlueprint();          // 설계도 생성
        CreateNodes();              // 노드 생성

        return;
        // 노드 생성과 트리 구조 완성
        //GameObject rootObj = CreateNewNodeInstance(bundles[0].transform, "Root", TreeNodeTypes.Root);
        //GameObject bossObj = CreateNewNodeInstance(bundles[^1].transform, "Boss", TreeNodeTypes.Boss);
        //root = rootObj.GetComponent<TreeNodeObject>();
        //boss = bossObj.GetComponent<TreeNodeObject>();
        //AddChilds(root, height - 2);

        // 노드끼리 물리적 연결 (UI Line Renderer).
        // UI Layout Group에서 포지션을 배치하기 위한 시간이 필요해서 코루틴으로 실행 시간 차이를 줌
        //StartCoroutine(CoLinkNodes());
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
            foreach ((TreeNodeTypes type, int branchCount) tuple in blueprint[i])
            {
                CreateNewNodeInstance(bundles[i].transform, i, $"{tuple.type}", tuple.type);
            }
        }
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

    private void AddChilds(TreeNodeObject parent, int depth)
    {
        if (depth == 0)
        {
            parent.AddChildren(boss);
            return;
        }

        int count = 1;
        //if (parent.Equals(root))
        //    count = 2;
        //else
        //{
        //    float randValue = Random.Range(0, 1f);
        //    if (randValue < branch3Probability)
        //        count = 3;
        //    else if (randValue < branch2Probability)
        //        count = 2;
        //}

        for (int i = 0; i < count; i++)
        {
            //TreeNodeTypes randomType = (TreeNodeTypes)Random.Range((int)TreeNodeTypes.Normal, (int)TreeNodeTypes.Event + 1);

            //// 인스턴스 생성과 데이터 연결
            //TreeNodeObject childNodeObj =
            //    CreateNewNodeInstance(bundles[height - 1 - depth].transform, $"{randomType}", randomType).
            //    GetComponent<TreeNodeObject>();
            //parent.AddChildren(childNodeObj);
        }

        for (int i = 0; i < count; i++)
            AddChilds(parent.childrens[i], depth - 1);
    }

    private IEnumerator CoLinkNodes()
    {
        yield return null;
        LinkNodes(root);
    }

    private void LinkNodes(TreeNodeObject parent)
    {
        int count = parent.childrens.Count;
        for (int i = 0; i < count; i++)
        {
            TreeNodeObject child = parent.childrens[i];
            GameObject newLineRenderer = Instantiate(uiLineRendererPrefab, lineRendererTarget);
            lines.Add(newLineRenderer);
            UILineRenderer uilr = newLineRenderer.GetComponent<UILineRenderer>();
            uilr.Points[0] = lineRendererTarget.InverseTransformPoint(parent.tail.position);
            uilr.Points[1] = lineRendererTarget.InverseTransformPoint(child.head.position);
            uilr.enabled = true;
            LinkNodes(child);
        }
    }

    private GameObject CreateNewNodeInstance(Transform target, int hIndex, string data, TreeNodeTypes type)
    {
        GameObject instanceNode = Instantiate(treeNodePrefab, target);
        TreeNodeObject node = instanceNode.GetComponent<TreeNodeObject>();
        node.SetInit(data, type);
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