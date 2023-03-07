using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneratePolynomialTreeMap : MonoBehaviour
{
    // Root, Boss 타입은 트리에 단 하나만 존재
    public TreeNode<string> root;
    public TreeNode<string> boss;
    public int height = 5;
    public int rangeMin = 1;
    public int rangeMax = 3;

    public int normalCount = 1;
    public int threatCount = 1;
    public int supplyCount = 1;
    public int eventCount = 1;

    public GameObject nodeBundlePrefab;
    public GameObject treeNodePrefab;
    public Transform target;

    private List<TreeNode<string>> nodePool = new();
    private int nodeIndex = 0;
    private List<GameObject> bundles = new();

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
        nodeIndex = 0;
        nodePool.Clear();

        // 필수 노드 생성
        root = new($"{TreeNodeTypes.Root}{nodeIndex++}", TreeNodeTypes.Root);
        IterateCreateNode(normalCount, TreeNodeTypes.Normal);
        IterateCreateNode(threatCount, TreeNodeTypes.Threat);
        IterateCreateNode(supplyCount, TreeNodeTypes.Supply);
        IterateCreateNode(eventCount, TreeNodeTypes.Event);
        boss = new($"{TreeNodeTypes.Boss}{nodeIndex++}", TreeNodeTypes.Boss);

        // 트리의 깊이 설정
        CreateBundles(height);

        // 노드 생성과 연결
        CreateNewNodeInstance(Instantiate(treeNodePrefab, bundles[0].transform), root);
        AddChilds(root, height);
        CreateNewNodeInstance(Instantiate(treeNodePrefab, bundles[^1].transform), boss);
    }

    private void AddChilds(TreeNode<string> parent, int depth)
    {
        if (depth == 0)
        {
            parent.AddChildren(boss);
            return;
        }

        int count = Random.Range(rangeMin, rangeMax + 1);
        Debug.Log($"{parent.data}, count : {count}");
        while (count-- > 0)
        {
            if (nodePool.Count == 0)
            {
                Debug.Log("count = 0");
                return;
            }
            int randomIndex = Random.Range(0, nodePool.Count);
            // 데이터 구조상의 연결
            TreeNode<string> child = nodePool[randomIndex];
            parent.AddChildren(child);

            // 인스턴스 생성과 데이터 연결
            CreateNewNodeInstance(Instantiate(treeNodePrefab, bundles[height - depth + 1].transform), child);

            AddChilds(child, depth - 1);
            nodePool.Remove(child);
        }
    }

    private void CreateNewNodeInstance(GameObject instance, TreeNode<string> dataNode)
    {
        TreeNode node = instance.GetComponent<TreeNode>();
        node.treeNode = dataNode;
        node.nodeNameText.text = dataNode.data;
    }

    public void CreateBundles(int depth)
    {
        for (int i = 0; i < depth + 2; i++)
        {
            bundles.Add(Instantiate(nodeBundlePrefab, target));
        }
    }

    private void IterateCreateNode(int count, TreeNodeTypes type)
    {
        for (int i = 0; i < count; i++)
        {
            nodePool.Add(new TreeNode<string>($"{type}{nodeIndex}", type));
            nodeIndex++;
        }
    }
}