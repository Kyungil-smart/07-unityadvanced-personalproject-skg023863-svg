using UnityEngine;

public class MoveState : IState
{
    
    private PlayerController _player;

    public MoveState(PlayerController player)
    {
        _player = player;
    }
    public void Enter()
    {
        _player.SetMove(0.2f);
    }

    public void Update()
    {
        if (_player._moveInput == Vector2.zero)
        {
            _player.ChangeState(_player.idle);
        }
    }

    public void Exit()
    {
        
    }
}
