using System.Collections.Generic;
using UnityEngine;

public class NewGridManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    private Dictionary<Vector2Int, Tile> _tiles;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2Int, Tile>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y, 0f), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";

                bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);

                _tiles[new Vector2Int(x, y)] = spawnedTile;
            }
        }

        _cam.position = new Vector3(_width / 2f - 0.5f, _height / 2f - 0.5f, -10f);
    }

    public Tile GetTileAtPosition(Vector2Int pos)
    {
        if (_tiles.TryGetValue(pos, out Tile tile))
        {
            return tile;
        }

        return null;
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x),
            Mathf.RoundToInt(worldPos.y)
        );
    }

    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y, 0f);
    }


}
