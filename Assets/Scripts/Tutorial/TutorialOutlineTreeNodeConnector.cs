using UnityEngine;

public class TutorialOutlineTreeNodeConnector : MonoBehaviour
{
    private TutorialOutline tutorialOutline;
    public GameObject nodes;

    public int bundleIndex;
    public int nodeIndex;

    private void Awake()
    {
        tutorialOutline = GetComponent<TutorialOutline>();
        ConnectOutlineToTreeNode();        
    } 

    private void ConnectOutlineToTreeNode()
    {
        var bundle = nodes.transform.GetChild(bundleIndex);
        var node = bundle.GetChild(nodeIndex);

        tutorialOutline.originalButton = node.gameObject;
    }
}
