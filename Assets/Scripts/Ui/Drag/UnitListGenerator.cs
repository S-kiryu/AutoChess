using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitListGenerator : MonoBehaviour
{
    [SerializeField] private GameObject unitPrefab;

    public void Generate(List<UnitData> units)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var unit in units)
        {
            var obj = Instantiate(unitPrefab, transform);

            var dragItem = obj.GetComponent<UnitDragItem>();
            if (dragItem != null)
            {
                //dragItem.Initialize(unit, -1, false);
            }

            var image = obj.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = unit.Icon;
                image.enabled = unit.Icon != null;
            }
        }
    }
}
