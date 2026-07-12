using UnityEngine;
using DG.Tweening;
using System;

public class UIMove : MonoBehaviour
{
    [SerializeField] private RectTransform _targetUI;
    [SerializeField] private int target;
    [SerializeField] private float time = 1f;
    bool isMoving = false;

    public void MoveRight()
    {
        if (isMoving) return;
        _targetUI.DOAnchorPosX(
            _targetUI.anchoredPosition.x + target,
            time
        );

        isMoving = true;
        Debug.Log("右に移動");
    }

    public void MoveLeft()
    {
        if (!isMoving) return;
        _targetUI.DOAnchorPosX(
            _targetUI.anchoredPosition.x - target,
            time
        );
        FormationUnitLevelUpUI.Instance.Hide();

        isMoving = false;
        Debug.Log("左に移動");
    }
}