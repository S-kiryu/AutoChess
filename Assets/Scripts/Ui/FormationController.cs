using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FormationController : MonoBehaviour
{
    [SerializeField] private int _index;
    [SerializeField] private int X;
    [SerializeField] private int Y;

    private UnitUnlockChecker _UnitUnlockChecker;
    UnitData[] _unitDatas;

    public void Start()
    {
        _UnitUnlockChecker = new UnitUnlockChecker(_unitDatas);
    }


}
