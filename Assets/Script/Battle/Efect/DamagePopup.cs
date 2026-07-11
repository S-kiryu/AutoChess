using System;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI damageText;
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float moveSpeed = 1f;

    private float timer;
    private Action<DamagePopup> onFinished;

    public DamagePopup Initialize(int damage, Action<DamagePopup> onFinished)
    {
        this.onFinished = onFinished;
        timer = 0f;

        if (damageText != null)
        {
            damageText.text = damage.ToString();
        }

        return this;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (timer >= lifeTime)
        {
            onFinished?.Invoke(this);
        }
    }
}