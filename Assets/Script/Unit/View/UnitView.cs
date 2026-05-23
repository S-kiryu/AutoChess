using UnityEngine;

public class UnitView : MonoBehaviour
{
    public UnitInstance Instance;

    public void Initialize(UnitInstance instance)
    {
        Instance = instance;
    }
}
