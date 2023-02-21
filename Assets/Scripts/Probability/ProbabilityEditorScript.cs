//using System.IO;
//using System.Text;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(ProbabilitySetting))]
//public class ProbabilityEditorScript : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        // each json files
//        if (GUILayout.Button("Generate Tables"))
//        {
//            var probSetting = target as ProbabilitySetting;
//            var sb = new StringBuilder();
//            string open = "{";
//            string close = "}";
//            string ext = ".json";
//            string defaultPath = $"{Application.dataPath}/Tables/Characters/";

//            // Generate Each Character Datatable
//            for (int i = 0; i < probSetting.rowCount; i++)
//            {
//                sb.Append(
//                    $"{open}\n" +
//                    $"\t\"heroName\":\"{probSetting.names[i]}\",\n" +
//                    $"\t\"grade\":\"{probSetting.grades[i]}\",\n" +
//                    $"\t\"type\":\"{probSetting.types[i]}\",\n" +
//                    $"\t\"level\":\"{probSetting.levels[i]}\"\n" +
//                    $"{close}");
//                File.WriteAllText($"{defaultPath}{probSetting.names[i]}{ext}", sb.ToString());
//                sb.Clear();
//            }

//            // Generate Character List
//            sb.Append("Index,Name");
//            for (int i = 0; i < probSetting.rowCount; i++)
//                sb.Append($"\n{i},{probSetting.names[i]}");

//            File.WriteAllText($"{defaultPath}CharacterList.csv", sb.ToString());
//            sb.Clear();
//            AssetDatabase.Refresh();
//            Debug.Log("Generate");
//        }


//        // one csv file
//        /*if (GUILayout.Button("Generate Table (CSV)"))
//        {
//            var probSetting = target as ProbabilitySetting;
//            var sb = new StringBuilder();
//            sb.Append("Name,Probability,Color\n");
//            for (int i = 0; i < probSetting.rowCount; i++)
//            {
//                string r = Convert.ToInt32(probSetting.colors[i].r * 256).ToString("X");
//                string g = Convert.ToInt32(probSetting.colors[i].g * 256).ToString("X");
//                string b = Convert.ToInt32(probSetting.colors[i].b * 256).ToString("X");
//                string a = Convert.ToInt32(probSetting.colors[i].a * 256).ToString("X");
//                sb.Append($"{probSetting.names[i]},{probSetting.probs[i]:0.0000},{r}{g}{b}{a}\n");
//            }
//            var path = EditorUtility.SaveFilePanel("Save The ProbTable Enums", $"{Application.dataPath}/Tables", "ProbTable.csv", "csv");
//            File.WriteAllText(path, sb.ToString());
//            AssetDatabase.Refresh();
//            Debug.Log("Generate");
//        }*/


//        // one json file
//        /*if (GUILayout.Button("Generate Tables"))
//        {
//            var probSetting = target as ProbabilitySetting;
//            var sb = new StringBuilder();
//            string open = "{";
//            string close = "}";
//            sb.Append("{\n\t\"HeroCount\":\"" + $"{probSetting.rowCount}\",\n");
//            for (int i = 0; i < probSetting.rowCount; i++)
//            {
//                sb.Append(
//                    $"\t\"Hero{i:00}\":\n\t{open}\n" +
//                    $"\t\t\"heroName\":\"{probSetting.names[i]}\",\n" +
//                    $"\t\t\"grade\":\"{probSetting.grades[i]}\",\n" +
//                    $"\t\t\"type\":\"{probSetting.types[i]}\",\n" +
//                    $"\t\t\"level\":\"{probSetting.levels[i]}\"\n" +
//                    $"\t{close},\n");
//            }
//            var path = EditorUtility.SaveFilePanel("Save The ProbTable Enums", $"{Application.dataPath}/Tables", "HeroTable.json", "json");
//            sb.Append("\n}");
//            File.WriteAllText(path, sb.ToString());
//            AssetDatabase.Refresh();
//            Debug.Log("Generate");
//        }*/

//    }
//}