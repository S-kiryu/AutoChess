using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class FormationController : MonoBehaviour
{
    //何ページあるか
    [SerializeField] private int _pageCount;
    //1ページに何ユニット配置できるか
    [SerializeField] private int _width,_height;
    //ユニットを配置する場所
    [SerializeField] private OrganizationUnitView organizationUnitView;

    private UnitUnlockChecker _UnitUnlockChecker;
    public List<Sprite> _unitOrganizationView = new List<Sprite>();
    private UnitData[] _unitDatas;

    public void Start()
    {
        _UnitUnlockChecker = new UnitUnlockChecker(_unitDatas);

        for(int i = 0;_unitDatas.Length > i; i++)
        {
            if (_UnitUnlockChecker.IsUnitUnlocked(_unitDatas[i]))
            {
                SetUnit(_unitDatas[i]);
            }
        }
    }

    private void  tiles() 
    {
        for (int i = 0; i < _pageCount; i++)
        {
            for (int j = 0; j < _width * _height; j++)
            {
                //タイルを生成する処理
            }
        }
    }

    private void SetUnit(UnitData unitData)
    {
        if (unitData == null || unitData.Icon == null)
        {
            Debug.LogWarning("UnitData or Icon is null.");
            return;
        }
        _unitOrganizationView.Add(unitData.Icon);
    }


}
