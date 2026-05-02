using UnityEngine;

public class FormationGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private int _gridSize = 10;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < _gridSize; i++)
        {
                Instantiate(gridPrefab, transform);
        }
    }
}
