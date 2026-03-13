using System.Collections;
using UnityEngine;

public class StopAndRunMonster : MonsterBase
{
    [SerializeField] private float _moveSeconds = 2f; // 몇 초간 움직일지 시간
    [SerializeField] private float _stopSeconds = 2f; // 몇 초간 멈출지 시간
    
    private WaitForSeconds _waitMove; // 몇 초간 움직일지 코루틴
    private WaitForSeconds _waitStop; // 몇 초간 멈출지 코루틴

    protected override void Awake()
    {
        base.Awake();
        _waitMove = new WaitForSeconds(_moveSeconds);
        _waitStop = new WaitForSeconds(_stopSeconds);
        // StartCoroutine(MoveCycle());
    }

    protected override void Update()
    {
        base.Update();
        FlipSprite();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (_isMoving)
        {
            MoveToPlayer();
        }
        else
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
        base.Die();
        ObjectPoolManager.Instance.Release(Resources.Load<GameObject>("RunAndStopMonster"), gameObject);
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        
        _isMoving = true;
        StartCoroutine(MoveCycle());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        
        StopAllCoroutines();
        _isMoving = false;
    }
}
