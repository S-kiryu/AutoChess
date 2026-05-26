using UnityEngine;
using UnityEngine.EventSystems;

public class FormationSlotView : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotIndex;
    [SerializeField] private FormationManager formationManager;

    public void OnDrop(PointerEventData eventData)
    {
        CharacterIconView iconView = eventData.pointerDrag.GetComponent<CharacterIconView>();

        if (iconView == null || iconView.Unit == null)
        {
            return;
        }

        formationManager.SetUnit(slotIndex, iconView.Unit);
        iconView.DropTo(transform);

        Debug.Log($"スロット{slotIndex}に配置: {iconView.Unit.Data.CharacterName}");
    }
}