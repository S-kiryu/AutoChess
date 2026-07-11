using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TextMesh damageText;
    [SerializeField] private int lifeTime = 1;
    private float timer = 0f;
    
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
