using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class OrganizationUnitView : MonoBehaviour
{
    public Tile _tile;

    public void makeTiles() 
    {
        Instantiate(_tile, transform.position, Quaternion.identity, transform);
    }
}
