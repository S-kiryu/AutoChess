using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    [SerializeField] private GameObject damagePopupPrefab;
    [SerializeField] private Transform damagePopupRoot;
    [SerializeField] private int initialPoolCount = 20;

    [Header("Spawn Offset")]
    [SerializeField] private Vector3 baseOffset = new Vector3(0f, 1f, 0f);
    [SerializeField] private Vector2 randomXRange = new Vector2(-0.25f, 0.25f);
    [SerializeField] private Vector2 randomYRange = new Vector2(0f, 0.25f);

    private GameObjectPool damagePopupPool;

    private void Awake()
    {
        damagePopupPool = new GameObjectPool(
            damagePopupPrefab,
            initialPoolCount,
            damagePopupRoot);
    }

    public void ShowDamage(int damage, Transform target)
    {
        if (target == null)
        {
            return;
        }

        Vector3 randomOffset = new Vector3(
            Random.Range(randomXRange.x, randomXRange.y),
            Random.Range(randomYRange.x, randomYRange.y),
            0f);

        ShowDamage(damage, target.position + baseOffset + randomOffset);
    }

    public void ShowDamage(int damage, Vector3 position)
    {
        GameObject obj = damagePopupPool.Get(position, Quaternion.identity);

        DamagePopup popup = obj.GetComponent<DamagePopup>();

        if (popup == null)
        {
            damagePopupPool.Release(obj);
            return;
        }

        popup.Initialize(damage, Release);
    }

    private void Release(DamagePopup popup)
    {
        damagePopupPool.Release(popup.gameObject);
    }
}