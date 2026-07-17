using UnityEngine;

public class PlayerLifeManager : MonoBehaviour
{
    [SerializeField] private int maxLife = 3;

    private int currentLife;

    public int MaxLife => maxLife;
    public int CurrentLife => currentLife;
    public bool IsGameOver => currentLife <= 0;

    private void Awake()
    {
        currentLife = maxLife;
    }

    public void ResetLife()
    {
        currentLife = maxLife;
    }

    public void AddLife()
    {
        if (currentLife >= maxLife)
        {
            return;
        }

        currentLife++;

        Debug.Log($"残機が増えました: {currentLife}/{maxLife}");
    }

    public void LoseLife()
    {
        if (currentLife <= 0)
        {
            return;
        }

        currentLife--;

        Debug.Log($"残機が減りました: {currentLife}/{maxLife}");
    }
}