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
    [SerializeField] protected float _damage;
    
    protected float _currentHP;
    protected Transform _playerTransform; // 플레이어 좌표

    protected virtual void Awake()
    {
        _currentHP = _maxHP;
        
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _bodySprite = GetComponent<SpriteRenderer>();
        
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
        Vector2 dir = (_playerTransform.position - transform.position).normalized;
        _rigidbody2D.linearVelocity = dir * _speed;
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
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.collider.TryGetComponent<IDamagable>(out var damageable))
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
