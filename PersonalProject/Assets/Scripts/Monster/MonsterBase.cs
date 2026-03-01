using UnityEngine;

public abstract class MonsterBase : MonoBehaviour , IDamagable
{
    //컴포넌트
    protected Rigidbody2D _rigidbody2D;
    protected Animator _animator;
    protected SpriteRenderer _bodySprite;
    
    [Header("몬스터 스탯")]
    [SerializeField] protected float _maxHP; // 몬스터 최대 체력
    [SerializeField] protected float _speed; // 몬스터 스피드
    [SerializeField] protected float _damage; // 몬스터 공격력
    
    protected float _currentHP;
    protected Transform _playerTransform; // 플레이어 좌표
    protected bool _isMoving;
    
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
    }
    
    
    public virtual void TakeDamage(float damage)
    {
        _currentHP -= damage;
        Debug.Log(_currentHP);
        if (_currentHP <= 0)
        {
            Die();
        }
    }
    
    protected virtual void Die()
    {
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
