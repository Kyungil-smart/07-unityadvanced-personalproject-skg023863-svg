using UnityEngine;

public class IdleState : IState
{
    private PlayerController _player;

    public IdleState(PlayerController player)
    {
        _player = player;
    }
    public void Enter()
    {
        _player.SetMove(0f);
    }

    public void Update()
    {
        if (_player._moveInput != Vector2.zero)
        {
            _player.ChangeState(_player.move);
        }
    }

    public void Exit()
    {
        
    }
}
