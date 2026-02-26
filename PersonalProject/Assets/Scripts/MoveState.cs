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
        // Move 애니메이션 실행
        _player.SetMove(0.2f);
    }

    public void Update()
    {
        // MoveInput이 (0, 0)이면 Idle 실행
        if (_player.moveInput == Vector2.zero)
        {
            _player.ChangeState(_player.idle);
        }
    }

    public void Exit()
    {
        
    }
}
