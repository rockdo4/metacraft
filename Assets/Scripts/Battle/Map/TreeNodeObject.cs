using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TreeNodeObject : MonoBehaviour, IComparable<TreeNodeObject>
{
    public TextMeshProUGUI nodeNameText;
    public Transform head;
    public Transform tail;
    public Button nodeButton;

    public string data;
    public TreeNodeTypes type;
    public List<TreeNodeObject> childrens = new ();

    public void SetInit(string data, TreeNodeTypes type = TreeNodeTypes.None)
    {
        this.data = data;
        this.type = type;
        string str = type switch
        {
            TreeNodeTypes.Root => "사무소",
            TreeNodeTypes.Threat => "강적",
            TreeNodeTypes.Supply => "보급",
            TreeNodeTypes.Event => "이벤트",
            TreeNodeTypes.Boss => "빌런",
            _ => "일반",
        };
        nodeNameText.text = str;
    }

    public void AddChildren(TreeNodeObject node)
    {
        int childrensCount = childrens.Count;
        for (int i = 0; i < childrensCount; i++)
            if (childrens[i].Equals(node))
                return;
        childrens.Add(node);
    }

    public int CompareTo(TreeNodeObject other)
    {
        return data.CompareTo(other.data);
    }
}