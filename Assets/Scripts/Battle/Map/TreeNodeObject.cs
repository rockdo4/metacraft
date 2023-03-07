using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct TreeNodeObject<T>
{
    public T data;
    public TreeNodeTypes type;
    public List<TreeNodeObject<T>> childrens;

    public TreeNodeObject(T data, TreeNodeTypes type = TreeNodeTypes.None)
    {
        this.data = data;
        this.type = type;
        childrens = new ();
    }

    public void AddChildren(T data, TreeNodeTypes type)
    {
        AddChildren(new TreeNodeObject<T>(data, type));
    }

    public void AddChildren(TreeNodeObject<T> node)
    {
        childrens.Add(node);
    }
}

public class TreeNodeObject : MonoBehaviour
{
    public TreeNodeObject<string> treeNode;
    public TextMeshProUGUI nodeNameText;
    public Transform head;
    public Transform tail;
}