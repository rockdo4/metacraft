using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class GeneratePolynomialTreeMap : MonoBehaviour
{
    // Root, Boss Ÿ���� Ʈ���� �� �ϳ��� ����
    public TreeNodeObject root;
    public TreeNodeObject boss;
    public int height = 5;
    public int branchRangeMin = 1;
    public int branchRangeMax = 3;

    //public int normalCount = 1;
    //public int threatCount = 1;
    //public int supplyCount = 1;
    //public int eventCount = 1;

    public GameObject nodeBundlePrefab;
    public GameObject treeNodePrefab;
    public GameObject uiLineRendererPrefab;
    public Transform nodeTarget;
    public Transform lineRendererTarget;

    private List<GameObject> bundles = new();
    private List<TreeNodeObject> nodes = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("new graph");
            CreateTreeGraph();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("link nodes");
            LinkNodes(root);
        }
    }

    public void CreateTreeGraph()
    {
        DestroyAllObjs();

        // Ʈ���� ���� ����
        CreateBundles(height);

        // ��� ������ Ʈ�� ���� �ϼ�
        GameObject rootObj = CreateNewNodeInstance(bundles[0].transform, "Root", TreeNodeTypes.Root);
        GameObject bossObj = CreateNewNodeInstance(bundles[^1].transform, "Boss", TreeNodeTypes.Boss);
        root = rootObj.GetComponent<TreeNodeObject>();
        boss = bossObj.GetComponent<TreeNodeObject>();
        AddChilds(root, height);

        // ��峢�� ������ ���� (UI Line Renderer)
        // LinkNodes(root);
    }

    public void DestroyAllObjs()
    {
        int ncount = nodes.Count;
        for (int i = 0; i < ncount; i++)
        {
            if (nodes[i] != null)
                Destroy(nodes[i]);
        }
        nodes.Clear();

        int bcount = bundles.Count;
        for (int i = 0; i < bcount; i++)
        {
            if (bundles[i] != null)
                Destroy(bundles[i]);
        }
        bundles.Clear();
    }

    private void AddChilds(TreeNodeObject parent, int depth)
    {
        // ��� �Լ�. Root�� ����
        if (depth == 0)
        {
            parent.AddChildren(boss);
            return;
        }

        int count = Random.Range(branchRangeMin, branchRangeMax + 1);
        while (count-- > 0)
        {
            TreeNodeTypes randomType = (TreeNodeTypes) Random.Range((int)TreeNodeTypes.Normal, (int)TreeNodeTypes.Event + 1);

            // �ν��Ͻ� ������ ������ ����
            GameObject childObj = CreateNewNodeInstance(bundles[height - depth + 1].transform, $"{randomType}", randomType);
            TreeNodeObject childNodeObj = childObj.GetComponent<TreeNodeObject>();
            parent.AddChildren(childNodeObj);

            AddChilds(childNodeObj, depth - 1);
        }
    }

    private void LinkNodes(TreeNodeObject parent)
    {
        // ��� �Լ�. Root�� ����
        int count = parent.childrens.Count;
        for (int i = 0; i < count; i++)
        {
            TreeNodeObject child = parent.childrens[i];

            GameObject newLineRenderer = Instantiate(uiLineRendererPrefab, lineRendererTarget);
            UILineRenderer uilr = newLineRenderer.GetComponent<UILineRenderer>();
            uilr.Points[0] = parent.tail.position;
            uilr.Points[1] = child.tail.position;
            uilr.enabled = true;
            LinkNodes(child);
        }
    }

    private GameObject CreateNewNodeInstance(Transform target, string data, TreeNodeTypes type)
    {
        GameObject instanceNode = Instantiate(treeNodePrefab, target);
        TreeNodeObject node = instanceNode.GetComponent<TreeNodeObject>();
        node.SetInit(data, type);
        nodes.Add(node);
        return instanceNode;
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
}