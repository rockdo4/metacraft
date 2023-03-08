using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeNodeObject : MonoBehaviour, IComparable<TreeNodeObject>
{
    public TextMeshProUGUI nodeNameText;
    public Transform head;
    public Transform tail;

    public string data;
    public TreeNodeTypes type;
    public List<TreeNodeObject> childrens = new ();

    public void SetInit(string data, TreeNodeTypes type = TreeNodeTypes.None)
    {
        this.data = data;
        this.type = type;
        nodeNameText.text = data;
    }

    public void AddChildren(TreeNodeObject node)
    {
        childrens.Add(node);
    }

    public int CompareTo(TreeNodeObject other)
    {
        return data.CompareTo(other.data);
    }
}