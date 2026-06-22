using NUnit.Framework;
using UnityEngine;

public abstract class BattleUnitGenerateBase : MonoBehaviour
{
    [SerializeField]private BattleGridManager _battleGridManager;
    private UnitInstance[,] _benchUnits;

    public virtual void SetUnit(int x,int y) 
    {
        
    }

    public abstract void PlaceUnit();

    public abstract void RemoveUnit();

    public abstract void SwapUnits();
}