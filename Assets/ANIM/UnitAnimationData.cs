using UnityEngine;

[CreateAssetMenu(menuName = "AutoChess/Unit/UnitAnimationData")]
public class UnitAnimationData : ScriptableObject
{
    [SerializeField] private Sprite[] idleSprites;
    [SerializeField] private Sprite[] moveSprites;
    [SerializeField] private DirectionalSprites attackSprites;
    [SerializeField] private Sprite[] skillSprites;

    public Sprite[] GetSprites(
        UnitAnimationType animationType,
        UnitDirection direction)
    {
        return animationType switch
        {
            UnitAnimationType.Idle => idleSprites,
            UnitAnimationType.Move => moveSprites,
            UnitAnimationType.Attack => attackSprites.GetSprites(direction),
            UnitAnimationType.Skill => skillSprites,
            _ => idleSprites
        };
    }
}