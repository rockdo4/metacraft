using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
public class SkillCreaterWindow : EditorWindow
{
    private Dictionary<string, object> skillInfo;

    private string sourcePath = "";
    private string createPath = "Assets/ScriptableObjects/Skills";
    private string objectName = "";

    private int lineNumber;
    private bool isActiveSkill;

    [MenuItem("Window/CustomEidtor/SkillCreater")]
    public static void ShowWindow()
    {        
        GetWindow<SkillCreaterWindow>("SkillCreater");
    }
    private void CreateGUIFields()
    {
        sourcePath = EditorGUILayout.TextField("SourcePath", sourcePath);
        createPath = EditorGUILayout.TextField("CreatePath", createPath);
        lineNumber = EditorGUILayout.IntField("LineNumber", lineNumber);
    }
    private void LoadLine()
    {
        var loadedFile = CSVReader.ReadByPath(sourcePath);

        skillInfo = loadedFile[lineNumber - 2];
        objectName = (string)skillInfo["Name"];
    }
    private CharacterSkill CreateScriptableObject()
    {
        isActiveSkill = (SkillMainType)skillInfo["Sort"] == SkillMainType.Active;

        var characterSkill = isActiveSkill ?
            CreateInstance<ActiveSkillAOE>() : CreateInstance<CharacterSkill>();

        SetSkillValues(characterSkill);

        return characterSkill;
    }
    private void SetSkillValues(CharacterSkill characterSkill)
    {
        if (isActiveSkill)
        {
            switch((SkillAreaShape)skillInfo["Shape"])
            {
                case SkillAreaShape.Sector:
                    break;
                case SkillAreaShape.Rectangle:
                    break;
            }            
        }
    }
    private void OnGUI()
    {
        CreateGUIFields();        

        if (GUILayout.Button("Create Scriptable Object"))
        {
            LoadLine();

            AssetDatabase.CreateAsset(CreateScriptableObject(), $"{createPath}/{objectName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}