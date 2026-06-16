using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BattleGridManager : MonoBehaviour
{
    public static BattleGridManager Instance { get; private set; }
    [SerializeField] private BattleGrid grid;
    private GameObject[] _gameObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid() 
    {
        var battleGrid = Instantiate(grid, transform);
    }
}
