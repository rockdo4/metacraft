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
    public int childrensCount = 0;
    public int floor = 0;
    public int number = 0;

    public void SetInit(string data, int floor, int number, TreeNodeTypes type = TreeNodeTypes.None)
    {
        this.data = data;
        this.type = type;
        this.floor = floor;
        this.number = number;
        nodeNameText.text = data;
    }

    public void AddChildren(TreeNodeObject node)
    {
        for (int i = 0; i < childrensCount; i++)
            if (childrens[i].Equals(node))
                return;
        childrens.Add(node);
        childrensCount++;
    }

    public int CompareTo(TreeNodeObject other)
    {
        return data.CompareTo(other.data);
    }
}