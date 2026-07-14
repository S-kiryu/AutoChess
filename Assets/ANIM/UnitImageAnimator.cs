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
    [SerializeField] private Image _unitImage;

    [Header("Animation Clip から操作")]
    [SerializeField] private UnitAnimationType _animationType;
    [SerializeField] private UnitDirection _direction;
    [SerializeField] private int _frameIndex;

    private UnitAnimationData _animationData;
    private UnitAnimationType _previousAnimationType;
    private UnitDirection _previousDirection;
    private int _previousFrameIndex = -1;
    private bool _isInitialized;

    private void Awake()
    {
        if (_unitImage == null)
        {
            _unitImage = GetComponentInChildren<Image>();
        }
    }

    public void SetAnimationData(UnitAnimationData animationData)
    {
        _animationData = animationData;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (_unitImage == null || _animationData == null)
        {
            return;
        }

        Sprite[] sprites = _animationData.GetSprites(
            _animationType,
            _direction);

        if (sprites == null || sprites.Length == 0)
        {
            return;
        }

        int index = Mathf.Clamp(_frameIndex, 0, sprites.Length - 1);
        _unitImage.sprite = sprites[index];
    }

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
}