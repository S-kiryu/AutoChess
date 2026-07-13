using UnityEngine;
using UnityEngine.UI;

public enum UnitAnimationType
{
    Idle,
    Move,
    Attack,
    Skill
}

public class UnitImageAnimator : MonoBehaviour
{
    [Header("画像を表示するUI(順番は上下左右)")]
    [SerializeField] private Image _unitImage;

    [Header("待機画像(順番は上下左右)")]
    [SerializeField] private Sprite[] _idleSprites;

    [Header("移動画像(順番は上下左右)")]
    [SerializeField] private Sprite[] _moveSprites;

    [Header("攻撃画像(順番は上下左右画像二枚まで)")]
    [SerializeField] private Sprite[] _attackSprites;

    [Header("スキル画像(順番は上下左右)")]
    [SerializeField] private Sprite[] _skillSprites;

    [Header("Animation Clipから操作")]
    [SerializeField] private UnitAnimationType _animationType;

    [SerializeField] private int _frameIndex;

    private UnitAnimationType _previousAnimationType;
    private int _previousFrameIndex = -1;
    private bool _isInitialized;

    private void LateUpdate()
    {
        if (_isInitialized &&
            _previousAnimationType == _animationType &&
            _previousFrameIndex == _frameIndex)
        {
            return;
        }

        _isInitialized = true;
        _previousAnimationType = _animationType;
        _previousFrameIndex = _frameIndex;

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (_unitImage == null)
        {
            return;
        }

        Sprite[] sprites = GetCurrentSprites();

        if (sprites == null || sprites.Length == 0)
        {
            return;
        }

        int index = Mathf.Clamp(
            _frameIndex,
            0,
            sprites.Length - 1);

        _unitImage.sprite = sprites[index];
    }

    private Sprite[] GetCurrentSprites()
    {
        return _animationType switch
        {
            UnitAnimationType.Idle => _idleSprites,
            UnitAnimationType.Move => _moveSprites,
            UnitAnimationType.Attack => _attackSprites,
            UnitAnimationType.Skill => _skillSprites,
            _ => _idleSprites
        };
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UpdateSprite();
    }
#endif
}