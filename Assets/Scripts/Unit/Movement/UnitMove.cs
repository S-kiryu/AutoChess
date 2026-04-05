using UnityEngine;

public class UnitMove : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 5f;

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0;

        rb.linearVelocity = direction * speed;
    }
}
