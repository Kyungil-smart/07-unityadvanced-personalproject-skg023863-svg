using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class MonsterBase : MonoBehaviour, IDamagable
{
    //컴포넌트
    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;
    protected SpriteRenderer _bodySprite;
    
    [Header("몬스터 스탯")]
    [SerializeField] protected float _maxHP; // 몬스터 최대 체력
    [SerializeField] protected float _speed; // 몬스터 스피드
    [SerializeField] protected float _damage; // 몬스터 공격력
    
    [Header("골드량 조절")]
    protected int _gold; // 몬스터 처치시 얻는 골드
    [SerializeField] protected int _minGold;
    [SerializeField] protected int _maxGold;

    [Header("몬스터 사운드")] 
    [SerializeField] protected AudioClip _monsterHitSound;

    [Header("피격시 반짝이는 시간")] 
    protected float _blinkTime = 0.1f;
    private WaitForSeconds _blinkWait;
    
    protected float _currentHP;
    protected Transform _playerTransform; // 플레이어 좌표
    protected bool _isMoving;
    
    public event Action OnDeath; // 몬스터가 죽으면 SpawnManager한테 알려줌
    
    protected StateMachine _stateMachine;
    protected virtual void Awake()
    {
        _currentHP = _maxHP;
        
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _bodySprite = GetComponent<SpriteRenderer>();
        
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        _stateMachine = new StateMachine();

        _isMoving = true;
        _gold = Random.Range(_minGold, _maxGold);
        _blinkWait = new WaitForSeconds(_blinkTime);
    }

    protected virtual void Update()
    {
        if (Time.timeScale == 0) return;
    }

    protected virtual void FixedUpdate()
    {
        if (Time.timeScale == 0) return;
    }
    
    
    public virtual void TakeDamage(float damage)
    {
        AudioManager.Instance.PlaySFX(_monsterHitSound, 0.5f);
        _currentHP -= damage;
        Debug.Log(_currentHP);
        if (_currentHP <= 0)
        {
            Die();
        }
        StartCoroutine(StartBlink());
    }

    private IEnumerator StartBlink()
    {
        _bodySprite.enabled = false;
        yield return _blinkWait;
        _bodySprite.enabled = true;
    }
    
    protected virtual void Die()
    {
        GameManager.Instance.AddGold(_gold);
        OnDeath?.Invoke();
        OnDeath = null;
        Destroy(gameObject);
        Debug.Log("몬스터 사망");
    }
    
    protected virtual void MoveToPlayer()
    {
        if (_playerTransform == null) return;

        if (!_isMoving) return;
        
        Vector2 dir = (_playerTransform.position - transform.position).normalized;
        
        _rigidbody2D.linearVelocity = dir * _speed;
    }

    protected virtual void StopMove()
    {
        _rigidbody2D.linearVelocity = Vector2.zero;
    }

    // Player를 바라보도록 스프라이트 변경
    protected virtual void FlipSprite()
    {
        Vector2 dir =  transform.position - _playerTransform.position;

        if (dir.x != 0)
        {
            _bodySprite.flipX = dir.x < 0;
        }
    }
    
    // Player 공격
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (collision.collider.TryGetComponent<IDamagable>(out IDamagable damageable))
            {
                damageable.TakeDamage(_damage);
            }
        }
    }
    public void SetMove(float value)
    {
        _animator.SetFloat("Move", Mathf.Abs(value));
    }
}
