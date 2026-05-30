using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ドロップ可能な編成スロットのUIクラス
public class FormationSlotView : MonoBehaviour, IDropHandler
{
    [SerializeField] private int slotIndex;
    [SerializeField] private FormationManager formationManager;
    [SerializeField] private Image unitIcon;

    private CharacterIconView currentIconView;

    public int SlotIndex => slotIndex;

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

    public void OnDrop(PointerEventData eventData)
    {
        CharacterIconView iconView = eventData.pointerDrag != null
            ? eventData.pointerDrag.GetComponent<CharacterIconView>()
            : null;

        if (iconView == null || iconView.Unit == null || formationManager == null)
        {
            return;
        }

        FormationSlotView sourceSlot = iconView.PreviousSlotView;
        CharacterIconView displacedIcon = currentIconView;

        formationManager.ClearUnitSilent(slotIndex);
        currentIconView = null;

        if (displacedIcon != null && displacedIcon != iconView)
        {
            if (sourceSlot != null)
            {
                if (!formationManager.SetUnitSilent(sourceSlot.SlotIndex, displacedIcon.Unit))
                {
                    RestoreDragSource(iconView, sourceSlot);
                    displacedIcon.SetToSlot(this);
                    formationManager.SetUnitSilent(slotIndex, displacedIcon.Unit);
                    formationManager.NotifyFormationChanged();
                    return;
                }

                sourceSlot.AcceptIcon(displacedIcon);
                displacedIcon.SetToSlot(sourceSlot);
            }
            else
            {
                displacedIcon.ReturnHomePosition();
            }
        }

        if (!formationManager.SetUnitSilent(slotIndex, iconView.Unit))
        {
            if (displacedIcon != null && displacedIcon != iconView)
            {
                displacedIcon.ReturnHomePosition();
            }

            RestoreDragSource(iconView, sourceSlot);
            formationManager.NotifyFormationChanged();
            return;
        }

        currentIconView = iconView;
        iconView.SetToSlot(this);

        formationManager.NotifyFormationChanged();
    }

    public void AcceptIcon(CharacterIconView iconView)
    {
        currentIconView = iconView;
    }

    public bool RestoreIcon(CharacterIconView iconView)
    {
        if (iconView == null || iconView.Unit == null || formationManager == null)
        {
            return false;
        }

        if (!formationManager.SetUnitSilent(slotIndex, iconView.Unit))
        {
            return false;
        }

        currentIconView = iconView;
        formationManager.NotifyFormationChanged();
        return true;
    }

    private void RestoreDragSource(CharacterIconView iconView, FormationSlotView sourceSlot)
    {
        if (sourceSlot != null && sourceSlot.RestoreIcon(iconView))
        {
            iconView.SetToSlot(sourceSlot);
            return;
        }

        iconView.ReturnHomePosition();
    }

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
    }

    public void DetachIcon(CharacterIconView iconView)
    {
        if (currentIconView != iconView)
        {
            return;
        }

        currentIconView = null;
        formationManager.ClearUnitSilent(slotIndex);
    }
}