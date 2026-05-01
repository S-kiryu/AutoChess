using UnityEngine;

/// <summary>一つのスロットを管理するクラス<summary>
public class FormationSlot : MonoBehaviour
{
    public int SlotIndex { get; private set; }
    public UnitData CurrentUnit { get; private set; }

    public SpriteRenderer _renderer;

    //ユニットをセットする
    public void SetUnit(UnitData unitData, int slotIndex)
    {
        SlotIndex = slotIndex;
        CurrentUnit = unitData;
    }

    public void SetSprite(Sprite sprite)
    {
        _renderer.sprite = sprite;
    }

    public void SetColor(Color color) 
    {
        _renderer.color = color;
    }
}
