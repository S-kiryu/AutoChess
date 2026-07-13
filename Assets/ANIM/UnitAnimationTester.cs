using System.Collections;
using UnityEngine;

public class UnitAnimationTester : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _changeInterval = 5f;

    private static readonly int IsMovingHash =
        Animator.StringToHash("IsMoving");

    private static readonly int DirectionHash =
        Animator.StringToHash("Direction");

    private static readonly int AttackHash =
        Animator.StringToHash("Attack");

    private void Start()
    {
        StartCoroutine(TestAnimation());
    }

    private IEnumerator TestAnimation()
    {
        while (true)
        {
            // 上へ移動
            SetMoveDirection(0);
            yield return new WaitForSeconds(_changeInterval);

            // 下へ移動
            SetMoveDirection(1);
            yield return new WaitForSeconds(_changeInterval);

            // 左へ移動
            SetMoveDirection(2);
            yield return new WaitForSeconds(_changeInterval);

            // 右へ移動
            SetMoveDirection(3);
            yield return new WaitForSeconds(_changeInterval);

            // 停止
            StopMove();
            yield return new WaitForSeconds(_changeInterval);

            // 上攻撃
            PlayAttack(0);
            yield return new WaitForSeconds(_changeInterval);

            // 下攻撃
            PlayAttack(1);
            yield return new WaitForSeconds(_changeInterval);

            // 左攻撃
            PlayAttack(2);
            yield return new WaitForSeconds(_changeInterval);

            // 右攻撃
            PlayAttack(3);
            yield return new WaitForSeconds(_changeInterval);
        }
    }

    private void SetMoveDirection(int direction)
    {
        _animator.SetInteger(DirectionHash, direction);
        _animator.SetBool(IsMovingHash, true);
    }

    private void StopMove()
    {
        _animator.SetBool(IsMovingHash, false);
    }

    private void PlayAttack(int direction)
    {
        // 攻撃は停止中だけ行う
        _animator.SetBool(IsMovingHash, false);

        // 攻撃する方向を指定
        _animator.SetInteger(DirectionHash, direction);

        // 攻撃Triggerを実行
        _animator.SetTrigger(AttackHash);
    }
}