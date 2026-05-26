using UnityEngine;

public class UnitView : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

        public void UpdateView(CharacterData Data)
        {
            if (Data == null || SpriteRenderer == null)
            {
                return;
            }
            SpriteRenderer.sprite = Data.Icon;
        }
}
