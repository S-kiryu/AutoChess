using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

/// <summary>編成の管理を行うクラス<summary>
public class FormationController : MonoBehaviour
{
    //何ページあるか
    [SerializeField] private int _pageCount;
    //1ページに何ユニット配置できるか
    [SerializeField] private int _width, _height;
    //ユニットのデータ
    [SerializeField] private UnitData[] _unitDatas;
    [SerializeField] private FormationBoardView _formationBoardView;

    private UnitUnlockChecker _unitUnlockChecker;

    public UnitUnlockChecker UnlockChecker { get; private set; }


    public void Start()
    {
        _unitUnlockChecker = new UnitUnlockChecker(_unitDatas);
        _unitUnlockChecker.UnlockUnit(_unitDatas[0]);
        _formationBoardView.Initialize(_width, _height);
        for (int i = 0; _unitDatas.Length > i; i++)
        {
            _formationBoardView.SetUnits(_unitDatas);
        }
    }
}
