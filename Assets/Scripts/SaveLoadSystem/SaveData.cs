public abstract class SaveData
{
    public int version = 0;

    public abstract SaveData VersionDown();
    public abstract SaveData VersionUp();

    public abstract void CopyData();
}

public class SaveDataV1 : SaveData
{
    public SaveDataV1()
    {
        version = 1;
    }

    public override SaveData VersionDown()
    {
        throw new System.NotImplementedException();
    }

    public override SaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }

    public override void CopyData()
    {
        throw new System.NotImplementedException();
    }
}

public class SaveDataV2 : SaveDataV1
{
    public SaveDataV2()
    {
        version = 2;
    }

    public override SaveData VersionDown()
    {
        throw new System.NotImplementedException();
    }

    public override SaveData VersionUp()
    {
        throw new System.NotImplementedException();
    }

    public override void CopyData()
    {
        throw new System.NotImplementedException();
    }
}