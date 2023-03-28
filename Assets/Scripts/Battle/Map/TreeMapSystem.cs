using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI.Extensions;

[System.Serializable]
public struct TreeInitSetting
{
    public int height;
    public int width;
    public float branch2Prob;
    public float branch3Prob;
    public int normalCount;
    public int threatCount;
    public int supplyCount;
    public int eventCount;

    public void InitSetting(int height = 4, int width = 5, float branch2Prob = 0.25f, float branch3Prob = 0.25f,
        int normalNodeCount = 0, int threatNodeCount = 0, int supplyNodeCount = 0, int eventNodeCount = 0)
    {
        this.height = height;
        this.width = width;
        this.branch2Prob = branch2Prob;
        this.branch3Prob = branch3Prob;
        normalCount = normalNodeCount;
        threatCount = threatNodeCount;
        supplyCount = supplyNodeCount;
        eventCount = eventNodeCount;
    }
}

public class TreeMapSystem : MonoBehaviour
{
    // Root, Boss 타입은 트리에 단 하나만 존재
    // 트리 가지의 수를 최소 1개, 최대 3개로 고정하고
    // 가지가 2개, 3개일 확률을 조정함
    public TreeNodeObject root;
    public TreeNodeObject villain;
    private TreeNodeObject curNode;
    public TreeNodeObject CurNode
    { 
        get { return curNode; }
        set
        {
            curNode = value;
            SetNodeHighlighter(curNode);
        }
    }

    // 인스펙터에서 노드 수를 조정할 때
    public TreeInitSetting treeSettings;

    // 내부에서 타입을 정해줄때 사용
    private int localNormalCount = 1;
    private int localThreatCount = 1;
    private int localSupplyCount = 1;
    private int localEventCount = 1;

    public GameObject nodeBundlePrefab;
    public GameObject treeNodePrefab;
    public GameObject uiLineRendererPrefab;
    public GameObject movableHighlighterPrefab;

    public GameObject background;
    public Transform nodeTarget;
    public Transform lineRendererTarget;
    public Transform movableHighlighterTarget;

    public GameObject currentNodeHighlighter;

    private List<GameObject> bundles = new();
    private List<List<TreeNodeObject>> nodes = new();
    private List<UILineRenderer> lines = new();
    private List<List<TreeBlueprintData>> blueprint = new();
    private List<GameObject> highlighters = new();
    private bool showFirst = true;
    private int difficulty = 0;

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

    public void ShowTree(bool value)
    {
        if (showFirst)
        {
            StartCoroutine(CoLinkNodes());  // 라인렌더러 위치 조정
            showFirst = false;
        }

        background.SetActive(value);
        nodeTarget.gameObject.SetActive(value);
        lineRendererTarget.gameObject.SetActive(value);
        movableHighlighterTarget.gameObject.SetActive(value);
    }

    public void TestLinkNodes()
    {
        StartCoroutine(CoLinkNodes());  // 라인렌더러 위치 조정
    }

    public void CreateTreeGraph(int difficulty = 0)
    {
        this.difficulty = difficulty;
        DestroyAllObjs();
        InitSettings(difficulty);       // 트리의 깊이 설정, 필요한 오브젝트 생성
        CreateBlueprint();              // 설계도 생성
        CreateNodes();                  // 노드 생성
        CurNode = root;

        int nodesLength = nodes.Count;
        for (int i = 0; i < nodesLength - 1; i++)
            LinkNodes(i, nodes[i], nodes[i + 1]); // 노드끼리 연결

        CreateLineRenderer();           // 라인렌더러 생성
        if (difficulty == 0) // 튜토리얼
        {
            TreeNodeObject upNode = CurNode.childrens[0];
            TreeNodeObject downNode = CurNode.childrens[1];
            upNode.SetNodeType(TreeNodeTypes.Threat);
            downNode.SetNodeType(TreeNodeTypes.Event);
            upNode.childrens[0].SetNodeType(TreeNodeTypes.Threat);
            downNode.childrens[0].SetNodeType(TreeNodeTypes.Supply);
        }
    }

    private void SetNodeHighlighter(TreeNodeObject node)
    {
        currentNodeHighlighter.transform.position = node.transform.position;
        
        OffMovableHighlighters();
        List<TreeNodeObject> movableNodes = node.childrens;
        int count = movableNodes.Count;
        for (int i = 0; i < count; i++)
        {
            highlighters[i].transform.position = movableNodes[i].transform.position;
            highlighters[i].SetActive(true);
        }
    }

    private void OffMovableHighlighters()
    {
        for (int i = 0; i < treeSettings.width; i++)
        {
            highlighters[i].SetActive(false);
        }
    }

    private void CreateBlueprint()
    {
        blueprint = new(treeSettings.height);
        for (int i = 0; i < treeSettings.height; i++)
        {
            int branchWidth = 0;
            TreeNodeTypes type;
            int branchCount = 1;
            bool fixBranchCount = false;
            if (i == 0) // Root
            {
                branchWidth = 1;
                branchCount = (!treeSettings.branch3Prob.Equals(0f)) ? Random.Range(2, 4) : 2;
                fixBranchCount = true;
                type = TreeNodeTypes.Root;
            }
            else if (i == treeSettings.height - 1) // Villain
            {
                branchWidth = 1;
                branchCount = 0;
                fixBranchCount = true;
                type = TreeNodeTypes.Villain;
            }
            else
            {
                type = TreeNodeTypes.None;
                foreach (var elem in blueprint[i - 1])
                {
                    branchWidth += elem.branchCount;
                }
                if (branchWidth > treeSettings.width)
                    branchWidth = treeSettings.width;
            }
            blueprint.Add(new(branchWidth));
            for (int j = 0; j < branchWidth; j++)
            {
                if (!fixBranchCount)
                {
                    float randNum = Random.Range(0f, 1f);
                    branchCount =
                        randNum < treeSettings.branch3Prob ? 3 :
                        randNum < treeSettings.branch2Prob ? 2 : 1;
                }
                TreeBlueprintData data = new(SelectType(type), branchCount, i, j);
                blueprint[i].Add(data);
            }
        }
    }

    private void CreateNodes()
    {
        for (int i = 0; i < treeSettings.height; i++)
        {
            foreach (TreeBlueprintData data in blueprint[i])
            {
                CreateNewNodeInstance(bundles[i].transform, data);
            }
        }
        root = nodes[0][0].GetComponent<TreeNodeObject>();
        villain = nodes[treeSettings.height - 1][0].GetComponent<TreeNodeObject>();
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
            returnType = (TreeNodeTypes)Random.Range(2, 6);
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
        int index = 0;
        int depth = nodes.Count;
        for (int i = 0; i < depth; i++)
        {
            int bundleWidth = nodes[i].Count;
            for (int j = 0; j < bundleWidth; j++)
            {
                int childCount = nodes[i][j].childrens.Count;
                for (int k = 0; k < childCount; k++)
                {
                    SetLinePosition(lines[index++], nodes[i][j].tail.position, nodes[i][j].childrens[k].head.position);
                }
            }
        }
        CurNode = root;
    }

    private void CreateLineRenderer()
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
                    GameObject lrObj = Instantiate(uiLineRendererPrefab, lineRendererTarget);
                    lines.Add(lrObj.GetComponent<UILineRenderer>());
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

    private void SetLinePosition(UILineRenderer lr, Vector3 start, Vector3 end)
    {
        lr.Points[0] = lineRendererTarget.InverseTransformPoint(start);
        lr.Points[1] = lineRendererTarget.InverseTransformPoint(end);
        lr.enabled = true;
    }

    private GameObject CreateNewNodeInstance(Transform target, TreeBlueprintData data)
    {
        GameObject instanceNode = Instantiate(treeNodePrefab, target);
        TreeNodeObject node = instanceNode.GetComponent<TreeNodeObject>();
        node.SetNodeType(data.nodeType);
        nodes[data.floor].Add(node);
        return instanceNode;
    }

    public void InitSettings(int difficulty = 0)
    {
        switch (difficulty)
        {
            case 0: // difficulty = 0, 튜토리얼일때
                treeSettings.InitSetting(4, 2, 0, 0);
                break;
            case 1:
            case 2:
                treeSettings.InitSetting(4);
                break;
            case 3:
            case 4:
                treeSettings.InitSetting(5);
                break;
            case 5:
                treeSettings.InitSetting(6);
                break;
            default:
                break;
        }
        localNormalCount = treeSettings.normalCount;
        localThreatCount = treeSettings.threatCount;
        localSupplyCount = treeSettings.supplyCount;
        localEventCount = treeSettings.eventCount;

        for (int i = 0; i < treeSettings.height; i++)
        {
            GameObject bundle = Instantiate(nodeBundlePrefab, nodeTarget);
            bundle.name = $"bundle_{i}";
            nodes.Add(new());
            bundles.Add(bundle);
        }

        for (int i = 0; i < treeSettings.width; i++)
        {
            GameObject highlighterObj = Instantiate(movableHighlighterPrefab, movableHighlighterTarget);
            highlighters.Add(highlighterObj);
            highlighterObj.SetActive(false);
        }
    }

    public void DestroyAllObjs()
    {
        int lcount = lines.Count;
        for (int i = 0; i < lcount; i++)
        {
            if (lines[i] != null)
                Destroy(lines[i].gameObject);
        }
        lines.Clear();

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

        int hcount = highlighters.Count;
        for (int i = 0; i < hcount; i++)
        {
            if (highlighters[i] != null)
                Destroy(highlighters[i]);
        }
        highlighters.Clear();
    }
}