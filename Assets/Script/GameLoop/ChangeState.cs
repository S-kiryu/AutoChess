using UnityEngine;

public class ChangeState : MonoBehaviour
{
    [SerializeField]private GameState _gameState;
    public void StateChange() 
    {
        GameLoopManager.Instance.ChangeState(_gameState);
        Debug.Log($"StateChange: {_gameState}");
    }
}
