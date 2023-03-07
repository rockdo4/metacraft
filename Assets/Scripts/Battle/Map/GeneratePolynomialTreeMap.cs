using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class GeneratePolynomialTreeMap : MonoBehaviour
{
    // Root, Boss 타입은 트리에 단 하나만 존재
    public TreeNodeObject<string> root;
    public TreeNodeObject<string> boss;
    public int height = 5;
    public int rangeMin = 1;
    public int rangeMax = 3;

    public int normalCount = 1;
    public int threatCount = 1;
    public int supplyCount = 1;
    public int eventCount = 1;

    public GameObject nodeBundlePrefab;
    public GameObject treeNodePrefab;
    public GameObject uiLineRendererPrefab;
    public Transform nodeTarget;
    public Transform lineRendererTarget;

    private List<TreeNodeObject<string>> nodePool = new();
    private List<GameObject> bundles = new();
    private List<GameObject> nodes = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("new graph");
            CreateTreeGraph();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("print current pool");
            foreach (var elem in nodePool)
            {
                Debug.Log($"{elem.data}");
            }
        }
    }

    public void CreateTreeGraph()
    {
        nodePool.Clear();
        DestroyAllObjs();

        // 필수 노드 생성
        root = new($"{TreeNodeTypes.Root}", TreeNodeTypes.Root);
        IterateCreateNode(normalCount, TreeNodeTypes.Normal);
        IterateCreateNode(threatCount, TreeNodeTypes.Threat);
        IterateCreateNode(supplyCount, TreeNodeTypes.Supply);
        IterateCreateNode(eventCount, TreeNodeTypes.Event);
        boss = new($"{TreeNodeTypes.Boss}", TreeNodeTypes.Boss);

        // 트리의 깊이 설정
        CreateBundles(height);

        // 노드 생성과 트리 구조 완성
        GameObject rootObj = Instantiate(treeNodePrefab, bundles[0].transform);
        nodes.Add(rootObj);
        CreateNewNodeInstance(rootObj, root);

        AddChilds(root, height);

        GameObject bossObj = Instantiate(treeNodePrefab, bundles[^1].transform);
        CreateNewNodeInstance(bossObj, boss);
        nodes.Add(bossObj);

        // 노드끼리 물리적 연결 (UI Line Renderer)
        // LinkNodes(rootObj);
    }

    public void DestroyAllObjs()
    {
        int ncount = nodes.Count;
        for (int i = 0; i < ncount; i++)
        {
            Destroy(nodes[i]);
        }
        nodes.Clear();
        int bcount = bundles.Count;
        for (int i = 0; i < bcount; i++)
        {
            Destroy(bundles[i]);
        }
        bundles.Clear();
    }

    private void AddChilds(TreeNodeObject<string> parent, int depth)
    {
        if (depth == 0)
        {
            parent.AddChildren(boss);
            return;
        }

        int count = Random.Range(rangeMin, rangeMax + 1);
        while (count-- > 0)
        {
            if (nodePool.Count == 0)
            {
                Debug.Log("count = 0");
                return;
            }
            int randomIndex = Random.Range(0, nodePool.Count);
            // 데이터 구조상의 연결
            TreeNodeObject<string> child = nodePool[randomIndex];
            parent.AddChildren(child);

            // 인스턴스 생성과 데이터 연결
            GameObject childObj = Instantiate(treeNodePrefab, bundles[height - depth + 1].transform);
            CreateNewNodeInstance(childObj, child);
            nodes.Add(childObj);

            AddChilds(child, depth - 1);
            nodePool.Remove(child);
        }
    }

    //private void LinkNodes(GameObject parent)
    //{
    //    TreeNodeObject ptno = parent.GetComponent<TreeNodeObject>();
    //    int count = ptno.treeNode.childrens.Count;
    //    for (int i = 0; i < count; i++)
    //    {
    //        GameObject child = FindNodeObj(ptno.treeNode);
    //        if (child == null)
    //            return;
    //        GameObject newLineRenderer = Instantiate(uiLineRendererPrefab, lineRendererTarget);
    //        UILineRenderer uilr = newLineRenderer.GetComponent<UILineRenderer>();
    //        TreeNodeObject ctno = child.GetComponent<TreeNodeObject>();
    //        uilr.Points[0] = ptno.tail.position;
    //        uilr.Points[1] = ctno.head.position;

    //        Debug.Log($"start: {parent.transform.position} / end: {child.transform.position}");
    //        LinkNodes(child);
    //    }
    //}

    private GameObject FindNodeObj(TreeNodeObject<string> nodeObject)
    {
        int count = nodes.Count;
        for (int i = 0; i < count; i++)
        {
            TreeNodeObject<string> compareData = nodes[i].GetComponent<TreeNodeObject>().treeNode;
            if (nodeObject.data.CompareTo(compareData.data) == 0)
                return nodes[i];
        }
        return null;
    }

    private void CreateNewNodeInstance(GameObject instance, TreeNodeObject<string> dataNode)
    {
        TreeNodeObject node = instance.GetComponent<TreeNodeObject>();
        node.treeNode = dataNode;
        node.nodeNameText.text = dataNode.data;
    }

    public void CreateBundles(int depth)
    {
        for (int i = 0; i < depth + 2; i++)
        {
            GameObject bundle = Instantiate(nodeBundlePrefab, nodeTarget);
            bundle.name = $"bundle_{i}";
            bundles.Add(bundle);
        }
    }

    private void IterateCreateNode(int count, TreeNodeTypes type)
    {
        for (int i = 0; i < count; i++)
        {
            nodePool.Add(new TreeNodeObject<string>($"{type}{i}", type));
        }
    }
}