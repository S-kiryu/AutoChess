using UnityEngine;

public class GridView : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GridManager grid;

    private GameObject[,] tiles;

    void Start()
    {
        int width = grid.Width;
        int height = grid.Height;

        tiles = new GameObject[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);

                tiles[x, y] = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
            }
    }
}
