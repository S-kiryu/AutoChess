using UnityEngine;

public class DragData : MonoBehaviour
{
    private UnitView UnitView;
    private UnitModel UnitModel;

    public void Update()
    {
        
    }

    public void SetData(UnitView unitView, UnitModel unitModel)
    {
        UnitView = unitView;
        UnitModel = unitModel;
    }

}
