using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using static UnityEditor.LightingExplorerTableColumn;

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
    public GameObject highlighter;

    private List<GameObject> bundles = new();
    private List<List<TreeNodeObject>> nodes = new();
    private List<GameObject> lines = new();
    private List<List<TreeBlueprintData>> blueprint = new();
    private int nodeIndex = 0;

    private struct TreeBlueprintData
    {
        public TreeNodeTypes nodeType;
        public int branchCount;
        public int floor;
        public int number;

        public TreeBlueprintData(TreeNodeTypes _type, int _count, int _floor, int _number)
        {
            nodeType = _type;
            branchCount = _count;
            floor = _floor;
            number = _number;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
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

    public void SetHighlighter(TreeNodeObject node)
    {
        highlighter.transform.position = node.transform.position;
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
                branchCount = Random.Range(2, 4);
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
                TreeBlueprintData data = new(SelectType(type), branchCount, i, j);
                blueprint[i].Add(data);
            }
        }
    }

    private void CreateNodes()
    {
        for (int i = 0; i < height; i++)
        {
            foreach (TreeBlueprintData data in blueprint[i])
            {
                CreateNewNodeInstance(bundles[i].transform, data);
            }
        }
        root = nodes[0][0].GetComponent<TreeNodeObject>();
        boss = nodes[height - 1][0].GetComponent<TreeNodeObject>();
    }

    private TreeNodeTypes SelectType(TreeNodeTypes type)
    {
        if (type != TreeNodeTypes.None)
            return type;

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
        int nodesLength = nodes.Count;
        for (int i = 0; i < nodesLength - 1; i++)
            LinkNodes(i, nodes[i], nodes[i + 1]);
        
        // UI Layout Group에서 포지션을 배치하기 위한 시간이 필요해서 코루틴으로 실행 시간 차이를 줌
        yield return null;
        DrawLineRenderer();
        SetHighlighter(root);
    }

    private void DrawLineRenderer()
    {
        int depth = nodes.Count;

        for (int i = 0; i < depth; i++)
        {
            int bundleWidth = nodes[i].Count;
            for (int j = 0; j < bundleWidth; j++)
            {
                int childCount = nodes[i][j].childrens.Count;
                for (int k = 0; k < childCount; k++)
                {
                    CreateNewLine(nodes[i][j].tail.position, nodes[i][j].childrens[k].head.position);
                }
            }
        }
    }

    private void LinkNodes(int floor, List<TreeNodeObject> parents, List<TreeNodeObject> childrens)
    {
        int parentLength = parents.Count;
        int index = 0;

        int restBranch = 0;
        for (int i = 0; i < parentLength; i++)
            restBranch += blueprint[floor][i].branchCount;

        for (int i = 0; i < parentLength; i++)
        {
            int count = blueprint[floor][i].branchCount;

            for (int j = 0; j < count; j++)
            {
                parents[i].AddChildren(childrens[index]);
                restBranch--;
                if (index != childrens.Count - 1)
                {
                    index++;
                }
            }

            if (index != childrens.Count - 1 &&
                count != 1 &&
                restBranch > childrens.Count - index)
            {
                index--;
            }
        }
    }

    private void CreateNewLine(Vector3 start, Vector3 end)
    {
        GameObject lrObj = Instantiate(uiLineRendererPrefab, lineRendererTarget);
        lines.Add(lrObj);
        UILineRenderer lr = lrObj.GetComponent<UILineRenderer>();
        lr.Points[0] = lineRendererTarget.InverseTransformPoint(start);
        lr.Points[1] = lineRendererTarget.InverseTransformPoint(end);
        lr.enabled = true;
    }

    private GameObject CreateNewNodeInstance(Transform target, TreeBlueprintData data)
    {
        GameObject instanceNode = Instantiate(treeNodePrefab, target);
        TreeNodeObject node = instanceNode.GetComponent<TreeNodeObject>();
        node.SetInit($"{data.nodeType}{nodeIndex++}", data.nodeType);
        nodes[data.floor].Add(node);
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