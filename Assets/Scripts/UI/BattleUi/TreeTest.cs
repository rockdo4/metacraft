using UnityEngine;

public class TreeTest : MonoBehaviour
{
    public TreeMapSystem tree;
    private int index = 0;

    private void Start()
    {
        Debug.Log("L阑 穿福搁 货肺款 tree啊 积己邓聪促.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log($"货肺款 飘府 积己 {index++}");
            tree.CreateTreeGraph(Random.Range(1, 6));
            tree.ShowTree(true);
            tree.TestLinkNodes();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log($"货肺款 飘府 积己 譬配府倔 {index++}");
            tree.CreateTreeGraph(0);
            tree.ShowTree(true);
            tree.TestLinkNodes();
        }
    }
}
