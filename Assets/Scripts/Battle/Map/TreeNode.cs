using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct TreeNode<T>
{
    public T data;
    public TreeNodeTypes type;
    public List<TreeNode<T>> childrens;

    public TreeNode(T data, TreeNodeTypes type = TreeNodeTypes.None)
    {
        this.data = data;
        this.type = type;
        childrens = new ();
    }

    public void AddChildren(T data, TreeNodeTypes type)
    {
        AddChildren(new TreeNode<T>(data, type));
    }

    public void AddChildren(TreeNode<T> node)
    {
        childrens.Add(node);
    }
}

public class TreeNode : MonoBehaviour
{
    public TreeNode<string> treeNode;
    public TextMeshProUGUI nodeNameText;
}