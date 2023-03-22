using UnityEngine;

public class TreeTest : MonoBehaviour
{
    public TreeMapSystem tree;
    private int index = 0;

    private void Start()
    {
        Debug.Log("L�� ������ ���ο� tree�� �����˴ϴ�.");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log($"���ο� Ʈ�� ���� {index++}");
            tree.CreateTreeGraph();
            tree.ShowTree(true);
            tree.TestLinkNodes();
        }
    }
}
