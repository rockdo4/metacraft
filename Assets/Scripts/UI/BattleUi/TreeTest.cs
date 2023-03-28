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
            tree.CreateTreeGraph(Random.Range(1, 6));
            tree.ShowTree(true);
            tree.TestLinkNodes();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log($"���ο� Ʈ�� ���� Ʃ�丮�� {index++}");
            tree.CreateTreeGraph(0);
            tree.ShowTree(true);
            tree.TestLinkNodes();
        }
    }
}
