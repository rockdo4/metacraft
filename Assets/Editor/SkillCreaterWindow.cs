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
        var loadedFile = CSVReader.ReadByStreamReaderPath(sourcePath, true, false);

        skillInfo = loadedFile[lineNumber - 2];
        objectName = (string)skillInfo["Name"];        
    }
    private CharacterSkill CreateScriptableObject()
    {        
        isActiveSkill = ValueToInt(skillInfo["Sort"]) == (int)SkillMainType.Active;

        var characterSkill = isActiveSkill ?
            CreateInstance<ActiveSkillAOE>() : CreateInstance<CharacterSkill>();

        SetSkillValues(characterSkill);

        return characterSkill;
    }
    private int ValueToInt(object obj)
    {
        return int.Parse(obj.ToString());
    }
    private float ValueToFloat(object obj)
    {
        return float.Parse(obj.ToString());
    }
    private void SetSkillValues(CharacterSkill characterSkill)
    {
        characterSkill.id          = ValueToInt(skillInfo["ID"]);
        characterSkill.cooldown    = ValueToFloat(skillInfo["CoolTime"]);
        characterSkill.preCooldown = ValueToFloat(skillInfo["StartCoolTime"]);

        characterSkill.targetType      = (SkillTargetType)ValueToInt(skillInfo["TargetType"]);
        characterSkill.coefficientType = (SkillCoefficientType)ValueToInt(skillInfo["BaseStats"]);
        characterSkill.coefficient     = ValueToFloat(skillInfo["Coefficient"]);
        
        characterSkill.skillDescription = (string)skillInfo["SkillInfo"];

        if (isActiveSkill)
        {
            var activeSkill = characterSkill as ActiveSkillAOE;

            activeSkill.layerM         = 1 << LayerMask.NameToLayer("Floor");
            activeSkill.castRangeLimit = ValueToInt(skillInfo["Range"]);

            LoadIndicatorPrefab(activeSkill);
            activeSkill.sectorRadius = ValueToInt(skillInfo["Radius"]);
            activeSkill.sectorAngle  = ValueToInt(skillInfo["Angle"]);
            activeSkill.widthZ       = ValueToInt(skillInfo["LengthZ"]);
            activeSkill.widthX       = ValueToInt(skillInfo["LengthX"]);
        }
    }
    private void LoadIndicatorPrefab(ActiveSkillAOE activeSkillAOE)
    {
        string prefabPath = "Assets/Prefabs/Battle/SkillIndicator/";
        string skillAreaIndicator = "";
        string castRangeIndicator = "";

        switch ((SkillAreaShape)ValueToInt(skillInfo["Shape"]))
        {
            case SkillAreaShape.Sector:
                {
                    bool isCircle      = ValueToInt(skillInfo["Angle"]) == 360;                    
                    string prefab      = isCircle ? "CircleIndicator.prefab" : "SectorIndicator.prefab";
                    skillAreaIndicator = prefabPath + prefab;
                }                
                break;
            case SkillAreaShape.Rectangle:                
                skillAreaIndicator = prefabPath + "SquareIndicator.prefab";
                break;
        }
        castRangeIndicator = prefabPath + "CastRangeIndicator.prefab";

        activeSkillAOE.skillAreaIndicatorPrefab
            = AssetDatabase.LoadAssetAtPath<SkillAreaIndicator>(skillAreaIndicator);
        activeSkillAOE.castRangeIndicatorPrefab 
            = AssetDatabase.LoadAssetAtPath<GameObject>(castRangeIndicator);
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