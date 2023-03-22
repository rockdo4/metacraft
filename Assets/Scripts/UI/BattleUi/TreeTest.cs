using UnityEngine;

public class TreeTest : MonoBehaviour
{
    public TreeMapSystem tree;
    private int index = 0;

    private void Start()
    {
        Debug.Log("L을 누르면 새로운 tree가 생성됩니다.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log($"새로운 트리 생성 {index++}");
            tree.CreateTreeGraph();
            tree.ShowTree(true);
            tree.TestLinkNodes();
        }
    }
}
