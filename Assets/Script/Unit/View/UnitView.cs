using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private UnitInstance instance;

    private SpriteRenderer spriteRenderer;
    private UnitPresenter presenter;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        presenter = new UnitPresenter(instance, this);
    }

    private void Start()
    {
        presenter.RefreshView();
    }

    public void Initialize(UnitInstance unitInstance)
    {
        instance = unitInstance;
        presenter = new UnitPresenter(instance, this);
        presenter.RefreshView();
    }

    public void UpdateView(CharacterData data)
    {
        if (data == null || spriteRenderer == null)
        {
            return;
        }

        spriteRenderer.sprite = data.Icon;
    }
}