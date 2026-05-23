using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    private List<UnitInstance> _characters = new List<UnitInstance>();
    public List<UnitInstance> Charactrs => _characters;
    private void Awake()
    {
        //if (Instance != null && Instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //DontDestroyOnLoad(gameObject);
        Instance = this;
    }


}
