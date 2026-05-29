using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//ドロップ可能な編成スロットのUIクラス
public class FormationSlotView : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotIndex;
    [SerializeField] private FormationManager formationManager;
    [SerializeField] private Image unitIcon;

    private CharacterIconView currentIconView;

    private void OnEnable()
    {
        if (formationManager != null)
        {
            formationManager.OnFormationChanged += RefreshView;
        }

        RefreshView();
    }

    private void OnDisable()
    {
        if (formationManager != null)
        {
            formationManager.OnFormationChanged -= RefreshView;
        }
    }

    //ドロップされたときに呼び出されるメソッド
    public void OnDrop(PointerEventData eventData)
    {
        CharacterIconView iconView = eventData.pointerDrag != null
            ? eventData.pointerDrag.GetComponent<CharacterIconView>()
            : null;

        if (iconView == null || iconView.Unit == null)
        {
            return;
        }

        if (!formationManager.SetUnit(slotIndex, iconView.Unit))
        {
            return;
        }

        if (currentIconView != null && currentIconView != iconView)
        {
            currentIconView.ReturnHomePosition();
        }

        currentIconView = iconView;
        currentIconView.SetToSlot(this);

        RefreshView();
        Debug.Log($"スロット{slotIndex}に配置: {iconView.Unit.Data.CharacterName}");
    }

    //スロットの表示を更新するためのメソッド
    private void RefreshView()
    {
        if (unitIcon == null || formationManager == null)
        {
            return;
        }

        UnitInstance unit = formationManager.GetUnit(slotIndex);

        if (unit == null)
        {
            unitIcon.enabled = false;
            unitIcon.sprite = null;
            return;
        }

        unitIcon.enabled = true;
        unitIcon.sprite = unit.Data.Icon;
    }

    public void RemoveIcon(CharacterIconView iconView)
    {
        if (currentIconView != iconView)
        {
            return;
        }

        currentIconView = null;
        formationManager.ClearUnit(slotIndex);
        RefreshView();
    }


}