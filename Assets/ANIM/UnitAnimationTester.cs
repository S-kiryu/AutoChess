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

    private void Start()
    {
        StartCoroutine(TestAnimation());
    }

    private IEnumerator TestAnimation()
    {
        while (true)
        {
            // 上
            SetMoveDirection(0);
            yield return new WaitForSeconds(_changeInterval);

            // 下
            SetMoveDirection(1);
            yield return new WaitForSeconds(_changeInterval);

            // 左
            SetMoveDirection(2);
            yield return new WaitForSeconds(_changeInterval);

            // 右
            SetMoveDirection(3);
            yield return new WaitForSeconds(_changeInterval);

            // 停止
            _animator.SetBool(IsMovingHash, false);
            yield return new WaitForSeconds(_changeInterval);
        }
    }

    private void SetMoveDirection(int direction)
    {
        _animator.SetInteger(DirectionHash, direction);
        _animator.SetBool(IsMovingHash, true);
    }
}