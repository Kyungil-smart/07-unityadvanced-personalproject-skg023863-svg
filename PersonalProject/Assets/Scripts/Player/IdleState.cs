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
        // Idle 애니메이션 실행
        _player.SetMove(0f);
    }

    public void Update()
    {
        // MoveInput이 (0, 0)이 아니면 Move 실행
        if (_player.moveInput != Vector2.zero)
        {
            _player.ChangeState(_player.move);
        }
    }

    public void Exit()
    {
        
    }
}
