using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ProbabilitySetting))]
public class ProbabilityEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Generate ProbTable Enums"))
        {
            var probSetting = target as ProbabilitySetting;
            var sb = new StringBuilder();
            sb.Append("Name,Probability,Color\n");
            for (int i = 0; i < probSetting.rowCount; i++)
            {
                string r = Convert.ToInt32(probSetting.colors[i].r * 256).ToString("X");
                string g = Convert.ToInt32(probSetting.colors[i].g * 256).ToString("X");
                string b = Convert.ToInt32(probSetting.colors[i].b * 256).ToString("X");
                string a = Convert.ToInt32(probSetting.colors[i].a * 256).ToString("X");
                sb.Append($"{probSetting.names[i]},{probSetting.probs[i]:0.0000},{r}{g}{b}{a}\n");
            }

            var path = EditorUtility.SaveFilePanel("Save The ProbTable Enums", $"{Application.dataPath}/Tables", "ProbTable.csv", "csv");
            
            File.WriteAllText(path, sb.ToString());
            AssetDatabase.Refresh();
            Debug.Log("Generate");
        }

    }
}