using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SkillCreaterWindow : EditorWindow
{
    private List<Dictionary<string, object>> loadedFile;

    private string sourcePath = "";
    private string createPath = "Assets/ScriptableObjects/Skills/";
    private string objectName = "";

    private float cooldown;
    private float distance;
    private float angle;
    private int count;

    [MenuItem("Window/CustomEidtor/SkillCreater")]
    public static void ShowWindow()
    {        
        GetWindow<SkillCreaterWindow>("SkillCreater");
    }
    private void CreateGUIFields()
    {
        sourcePath       = EditorGUILayout.TextField("SourcePath", sourcePath);
        createPath       = EditorGUILayout.TextField("CreatePath", createPath);
        objectName       = EditorGUILayout.TextField("ObjectName", objectName);

        cooldown = EditorGUILayout.FloatField("Cooldown", cooldown);
        distance = EditorGUILayout.FloatField("Distance", distance);
        count    = EditorGUILayout.IntField("Count", count);
        angle    = EditorGUILayout.FloatField("Angle", angle);
    }
    private void SetScriptableObjectValues(CharacterSkill characterSkill)
    {
        characterSkill.cooldown = cooldown;
        characterSkill.distance = distance;
        characterSkill.count    = count;
        characterSkill.angle    = angle;
    }
    private void OnGUI()
    {
        CreateGUIFields();

        if (GUILayout.Button("Create Scriptable Object"))
        {
            loadedFile = CSVReader.ReadByPath(sourcePath);

            var characterSkill = CreateInstance<CharacterSkill>();
            SetScriptableObjectValues(characterSkill);

            AssetDatabase.CreateAsset(characterSkill, $"{createPath}{objectName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}