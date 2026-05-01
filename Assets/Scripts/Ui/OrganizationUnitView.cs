using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class OrganizationUnitView : MonoBehaviour
{
    [SerializeField]private FormationSlot _formationSlot;


    public void CreateTile(Sprite icon,bool Released)
    {
        var tile = Instantiate(_formationSlot, transform);
        tile.SetSprite(icon);
        tile.SetColor(Released ? Color.white : Color.gray); 
    }
}
