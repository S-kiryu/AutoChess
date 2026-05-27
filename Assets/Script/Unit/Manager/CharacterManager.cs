using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    private readonly List<UnitInstance> characters = new();
    public IReadOnlyList<UnitInstance> Characters => characters;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    //キャラクターリストに追加するためのメソッド
    public UnitInstance CreateCharacter(CharacterData data)
    {
        var unit = UnitFactory.Create(data);
        characters.Add(unit);
        return unit;
    }
}