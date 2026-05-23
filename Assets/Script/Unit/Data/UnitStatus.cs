using System;
[Serializable]
public class UnitStatus
{
    public BaseStatus Base; //ランタイムデータ

    public void Initialize(BaseStatus baseStatus) 
    {
        Base = baseStatus;
    } 
}
