using UnityEngine;
using UnityEngine.UI;

public enum UnitAnimationType
{
    Idle,
    Move,
    Attack,
    Skill
}

public enum UnitDirection
{
    Up,
    Down,
    Left,
    Right
}

[System.Serializable]
public class DirectionalSprites
{
    [SerializeField] private Sprite[] _up;
    [SerializeField] private Sprite[] _down;
    [SerializeField] private Sprite[] _left;
    [SerializeField] private Sprite[] _right;

    public Sprite[] GetSprites(UnitDirection direction)
    {
        return direction switch
        {
            UnitDirection.Up => _up,
            UnitDirection.Down => _down,
            UnitDirection.Left => _left,
            UnitDirection.Right => _right,
            _ => _down
        };
    }
}

public class UnitImageAnimator : MonoBehaviour
{
    [Header("画像を表示するUI")]
    [SerializeField] private Image _unitImage;

    [Header("待機画像")]
    [SerializeField] private Sprite[] _idleSprites;

    [Header("移動画像")]
    [SerializeField] private Sprite[] _moveSprites;

    [Header("攻撃画像")]
    [SerializeField] private DirectionalSprites _attackSprites;

    [Header("スキル画像")]
    [SerializeField] private Sprite[] _skillSprites;

    [Header("Animation Clipから操作")]
    [SerializeField] private UnitAnimationType _animationType;
    [SerializeField] private UnitDirection _direction;
    [SerializeField] private int _frameIndex;

    private UnitAnimationType _previousAnimationType;
    private UnitDirection _previousDirection;
    private int _previousFrameIndex = -1;
    private bool _isInitialized;

    private void LateUpdate()
    {
        if (_isInitialized &&
            _previousAnimationType == _animationType &&
            _previousDirection == _direction &&
            _previousFrameIndex == _frameIndex)
        {
            return;
        }

        _isInitialized = true;
        _previousAnimationType = _animationType;
        _previousDirection = _direction;
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
            UnitAnimationType.Attack =>
                _attackSprites.GetSprites(_direction),
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