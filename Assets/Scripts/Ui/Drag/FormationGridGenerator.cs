using UnityEngine;

// フォーメーションのグリッドを生成するクラス
public class FormationGridGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _gridPrefab;
    [SerializeField] private UnitData[] _unitDatas;
    [SerializeField] private int _gridSize = 10;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int i = 0; i < _gridSize; i++)
        {

            var obj = Instantiate(_gridPrefab, transform);
            var data = obj.GetComponent<UnitDragItem>();

            if (data != null)
            {
                data.SetUnitData(_unitDatas[i]);
            }
        }
    }
}
