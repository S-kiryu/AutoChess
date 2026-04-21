using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private GameObject _highlight;

    //ƒ^ƒCƒ‹‚ÌF‚ğ”»’è‚µŒˆ‚ß‚é
    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
    }

    public void SetHighlight(bool flag)
    {
        _highlight.SetActive(flag);
    }

    void OnMouseEnter()
    {
        SetHighlight(true);
    }

    void OnMouseExit()
    {
        SetHighlight(false);
    }
}
