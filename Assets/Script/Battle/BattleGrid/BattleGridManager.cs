using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BattleGridManager : MonoBehaviour
{
    public static BattleGridManager Instance { get; private set; }
    [SerializeField] private BattleGrid grid;
    [SerializeField] private int x;
    [SerializeField] private int y;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;
    [SerializeField] private BenchManager benchManager;
    private BattleGrid[,] _battleGrid;

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
        _battleGrid = new BattleGrid[x,y];
        gridLayoutGroup.constraintCount = x;
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                var battleGrid = Instantiate(grid, transform);

                battleGrid.name = $"BattleGrid_{i}_{j}";

                bool isEven = (i + j) % 2 == 0;

                Image image = battleGrid.GetComponentInChildren<Image>();

                if (image != null)
                {
                    image.color = isEven ? color1 : color2;
                }

                _battleGrid[i, j] = battleGrid;
            }
        }
    }

    /// <summary>
    /// 範囲内かどうかをチェックする
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private bool IsInside(int x, int y)
    {
        return x >= 0 && x < _battleGrid.GetLength(0)
            && y >= 0 && y < _battleGrid.GetLength(1);
    }

}
