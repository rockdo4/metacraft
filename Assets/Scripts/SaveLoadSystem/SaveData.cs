using System;
using System.Collections.Generic;

[Serializable]
public abstract class SaveData
{
    public int version = 0;

    public abstract SaveData VersionDown();
    public abstract SaveData VersionUp();
}

public class SaveDataV1 : SaveData
{
    public SaveDataV1()
    {
        version = 1;
    }

    public Dictionary<int, int> range;

    public override SaveData VersionDown()
    {
        throw new System.NotImplementedException();
    }

    public override SaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }
}

//public class SaveDataV3 : SaveDataV2
//{
//    public SaveDataV3()
//    {
//        version = 3;
//    }

//    public override SaveData VersionDown()
//    {
//        throw new System.NotImplementedException();
//    }

//    public override SaveData VersionUp()
//    {
//        throw new System.NotImplementedException();
//    }
//}

//public class SaveDataV2 : SaveDataV1
//{
//    public SaveDataV2()
//    {
//        version = 2;
//    }

//    public override SaveData VersionDown()
//    {
//        throw new System.NotImplementedException();
//    }

//    public override SaveData VersionUp()
//    {
//        throw new System.NotImplementedException();
//    }
//}