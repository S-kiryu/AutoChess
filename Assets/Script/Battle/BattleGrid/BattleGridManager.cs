using Unity.VisualScripting;
using UnityEngine;

public class BattleGridManager : MonoBehaviour
{
    public static BattleGridManager Instance { get; private set; }
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

    }



}
