using UnityEngine;
using UnityEditor;
public class SkillCreaterWindow : EditorWindow
{
    private string path = "Assets/ScriptableObjects/Skills/";
    private string objectName = "";

    private float cooldown;
    private float distance;
    private int count;
    private float angle;

    [MenuItem("Window/CustomEidtor/SkillCreater")]
    public static void ShowWindow()
    {
        GetWindow<SkillCreaterWindow>("SkillCreater");
    }
    private void CreateGUIFields()
    {
        path       = EditorGUILayout.TextField("Path", path);
        objectName = EditorGUILayout.TextField("ObjectName", objectName);

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
            var characterSkill = CreateInstance<CharacterSkill>();
            SetScriptableObjectValues(characterSkill);

            AssetDatabase.CreateAsset(characterSkill, $"{path}{objectName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}