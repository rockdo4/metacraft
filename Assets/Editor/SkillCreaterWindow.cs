using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class SkillCreaterWindow : EditorWindow
{
    private Dictionary<string, object> skillInfo;
    private Dictionary<string, object> buffInfo;
    //private Dictionary<int, (int sort, float value)> stateInfo; //id°¡ key°ª

    private string skillTable = "";    
    private string buffTablePath = "";
    private string skillCreatePath = "Assets/ScriptableObjects/Skills";
    private string BuffCreatePath = "Assets/ScriptableObjects/Buff";
    private string BuffScriptableName = "";

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
        skillTable = EditorGUILayout.TextField("SkillTable", skillTable);        
        buffTablePath = EditorGUILayout.TextField("BuffTable", buffTablePath);
        BuffScriptableName = EditorGUILayout.TextField("BuffName", BuffScriptableName);
        skillCreatePath = EditorGUILayout.TextField("SkillCreatePath", skillCreatePath);
        BuffCreatePath = EditorGUILayout.TextField("BuffCreatePath", BuffCreatePath);
        lineNumber = EditorGUILayout.IntField("LineNumber", lineNumber);
    }
    private void LoadSkillTable()
    {        
        var loadedFile = CSVReader.ReadByStreamReaderPath(skillTable, true, false);

        skillInfo = loadedFile[lineNumber - 2];
        objectName = (string)skillInfo["Name"];        
    }
    private void LoadStateTable()
    {
        var loadedFile = CSVReader.ReadByStreamReaderPath(buffTablePath, true, false);
        buffInfo = loadedFile[lineNumber - 2];
        
        //foreach(var lines in loadedFile)
        //{
        //    stateInfo.Add(ValueToInt(lines["ID"]), (sort: ValueToInt(lines["sort"]), value: ValueToFloat(lines["Value"])));
        //}
    }
    private CharacterSkill CreateSkillScriptableObject()
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
    private BuffInfo CreateBuffScriptableObject()
    {
        BuffInfo buff = CreateInstance<BuffInfo>();
        
        buff.id = ValueToInt(buffInfo["ID"]);
        buff.name = buffInfo["Name"].ToString();
        buff.info = buffInfo["info"].ToString();
        buff.fraction = ValueToInt(buffInfo["fraction"]);
        buff.inf = buffInfo["Duration"].ToString().Equals("-");
        buff.type = (BuffType)ValueToInt(buffInfo["sort"]);
        buff.buffValue = ValueToInt(buffInfo["Value"]);
        float.TryParse(buffInfo["Duration"].ToString(), out buff.duration);

        return buff;
    }
    private void SetSkillValues(CharacterSkill characterSkill)
    {
        characterSkill.id          = ValueToInt(skillInfo["ID"]);
        characterSkill.cooldown    = ValueToFloat(skillInfo["CoolTime"]);
        characterSkill.preCooldown = ValueToFloat(skillInfo["StartCoolTime"]);

        characterSkill.targetType      = (SkillTargetType)ValueToInt(skillInfo["DamageTarget"]);
        characterSkill.coefficientType = (SkillCoefficientType)ValueToInt(skillInfo["BaseStats"]);
        characterSkill.coefficient     = ValueToFloat(skillInfo["Coefficient"]);
        
        characterSkill.skillDescription = (string)skillInfo["SkillInfo"];

        characterSkill.isCriticalPossible = ValueToInt(skillInfo["CanCri"]) == 1;

        characterSkill.targetNumLimit = ValueToInt(skillInfo["DamageTargetLimit"]);

        //characterSkill.buffTypeAndValue = new(3);
        //for(int i = 0; i < 3; i++)
        //{
        //    string stateNum = "State" + (i + 1).ToString();
        //    if (!ValueToInt(skillInfo[stateNum]).Equals(-1))
        //    {
        //        var tuple = stateInfo[ValueToInt(skillInfo[stateNum])];
        //        (BuffType, float) buffInfo = ((BuffType)tuple.sort, tuple.value);
        //        characterSkill.buffTypeAndValue.Add(buffInfo);
        //    }
        //}

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
            activeSkill.isTrackTarget = ValueToInt(skillInfo["IsAutoTargeting"]) == 1;

            if (hasDuration)
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

        if (GUILayout.Button("Create Skill Scriptable Object"))
        {
            LoadSkillTable();            

            AssetDatabase.CreateAsset(CreateSkillScriptableObject(), $"{skillCreatePath}/{objectName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        if (GUILayout.Button("Create Buff Scriptable Object"))
        {
            LoadStateTable();

            AssetDatabase.CreateAsset(CreateBuffScriptableObject(), $"{BuffCreatePath}/{BuffScriptableName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    private int ValueToInt(object obj)
    {
        var debug = int.TryParse(obj.ToString(), out int result);
        if (!debug)
            Debug.Log("Int ÆÄ½Ì ½ÇÆÐ");
        return result;
    }
    private float ValueToFloat(object obj)
    {
        var debug = float.TryParse(obj.ToString(), out float result);
        if (!debug)
            Debug.Log("Float ÆÄ½Ì ½ÇÆÐ");
        return result;
    }
}