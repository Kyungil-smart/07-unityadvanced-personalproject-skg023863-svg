using System.Collections;
using UnityEngine;

public class StopAndRunMonster : MonsterBase
{
    [SerializeField] private float _moveSeconds = 2f; // 언제까지 움직일지 시간
    [SerializeField] private float _stopSeconds = 2f; // 언제까지 멈출지 시간
    
    private WaitForSeconds _waitMove; // 언제까지 움직일지 코루틴
    private WaitForSeconds _waitStop; // 언제까지 멈출지 코루틴

    private void Start()
    {
        _waitMove = new WaitForSeconds(_moveSeconds);
        _waitStop = new WaitForSeconds(_stopSeconds);
        StartCoroutine(MoveCycle());
    }

    private void Update()
    {
        FlipSprite();
    }
    private void FixedUpdate()
    {
        if (_isMoving)
        {
            MoveToPlayer();
        }
        else if (!_isMoving)
        {
            StopMove();
        }
    }
    
    // _isMoving이 true면 MoveSecond시간 만큼 움직임, false면 stopSeconds시간 만큼 멈춤
    IEnumerator MoveCycle()
    {
        while (true)
        {
            _isMoving = true;
            SetMove(1f);
            yield return _waitMove;
            
            _isMoving = false;
            SetMove(0f);
            yield return _waitStop;
        }
    }

    protected override void Die()
    {
        // 죽으면 코루틴 멈춤
        StopAllCoroutines();
        base.Die();
    }
}
