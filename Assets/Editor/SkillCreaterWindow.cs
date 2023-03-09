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
    private bool hasDuration;

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

        CharacterSkill characterSkill;

        if(isActiveSkill)
        {
            hasDuration = ValueToInt(skillInfo["Duration"]) != -1;

            characterSkill = hasDuration ?
                CreateInstance<AOEWithDuration>() : CreateInstance<ActiveSkillAOE>();
        }
        else
            characterSkill = CreateInstance<CharacterSkill>();

        SetSkillValues(characterSkill);

        return characterSkill;
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

        characterSkill.isCriticalPossible = ValueToInt(skillInfo["CanCri"]) == 1;
        //characterSkill.isAuto = ValueToInt(skillInfo["IsAutoTargeting"]) == 0;

        if (isActiveSkill)
        {
            var activeSkill = characterSkill as ActiveSkillAOE;

            activeSkill.layerM         = 1 << LayerMask.NameToLayer("Floor");
            activeSkill.castRangeLimit = ValueToInt(skillInfo["Range"]) / 100f;

            LoadIndicatorPrefab(activeSkill);
            activeSkill.sectorRadius = ValueToInt(skillInfo["Radius"]) / 100f;
            activeSkill.sectorAngle  = ValueToInt(skillInfo["Angle"]);
            activeSkill.widthZ       = ValueToInt(skillInfo["LengthZ"]);
            activeSkill.widthX       = ValueToInt(skillInfo["LengthX"]);

            if(hasDuration)
            {
                var aoeWithDuration         = activeSkill as AOEWithDuration;
                aoeWithDuration.duration    = ValueToFloat(skillInfo["Duration"]);
                aoeWithDuration.hitInterval = ValueToFloat(skillInfo["HitInterval"]);
            }
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

                    activeSkillAOE.areaShapeType = isCircle ?
                        SkillAreaShape.Circle : SkillAreaShape.Sector;
                }                
                break;
            case SkillAreaShape.Rectangle:
                {
                    skillAreaIndicator = prefabPath + "SquareIndicator.prefab";
                    activeSkillAOE.areaShapeType = SkillAreaShape.Rectangle;
                }                
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
    private int ValueToInt(object obj)
    {
        return int.Parse(obj.ToString());
    }
    private float ValueToFloat(object obj)
    {
        return float.Parse(obj.ToString());
    }
}