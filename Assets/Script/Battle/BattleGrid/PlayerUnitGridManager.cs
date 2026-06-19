using UnityEngine;

public class PlayerUnit : BattleUnitGenerateBase
{
    private UnitInstance[,] _benchs;
    private BattleGridManager _battleGridManager;
    private int width, height;


    void Start()
    {
        _benchs = new UnitInstance[width, height];
    }

    public override void PlaceUnit() 
    {

    }

    public override void RemoveUnit() 
    {

    }

    public override void SwapUnits() 
    {

    }
}
