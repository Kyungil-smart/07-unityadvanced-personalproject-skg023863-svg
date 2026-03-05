using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour , IDamagable
{
    // 컴포넌트
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _bodySprite;
    private PlayerInput _playerInput; 
    private Animator _animator;
    
    private StateMachine _stateMachine;

    [Header("Player의 스탯")] 
    [SerializeField] private float _maxHP = 100f; // Player 최대 체력
    public float MaxHP
    {
        get { return _maxHP; }
        set { _maxHP = value; }
    }
    [SerializeField] private float _currentHP; // Player 현재 체력
    public float CurrentHP
    {
        get { return _currentHP; }
        set { _currentHP = value; }
    }
    
    [SerializeField] private float _playerSpeed = 5f; // Player 속도
    public float PlayerSpeed
    {
        get { return _playerSpeed; }
        set { _playerSpeed = value; }
    }
    [SerializeField] private float _damage = 10f; // 총알 공격력
    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }
    [SerializeField] private float _bulletSpeed = 15f; // 총알 속도
    public float BulletSpeed
    {
        get { return _bulletSpeed; }
        set { _bulletSpeed = value; }
    }
    [SerializeField] private float _bulletDistance = 4f; // 총알 사거리
    public float BulletDistance
    {
        get { return _bulletDistance; }
        set { _bulletDistance = value; }
    }
    [SerializeField] private float _fireRate = 0.5f; // 총을 쏜 후 다음에 총을 쏠 수 있는 시간
    public float FireRate
    {
        get { return _fireRate; }
        set { _fireRate = value; }
    }
    private bool _isShoting; // 지금 총을 쏠 수 있는지
    private float _nextFireTime; // Time.time + _fireRate의 합
    
    [SerializeField] private float _invincibleTime = 1f; // 무적시간
    private bool _isInvincible; // 무적인지 아닌지
    private WaitForSeconds _invincibleWait; // 무적 코루틴에 사용할 WaitForSeconds
    [SerializeField] private float _blinkInterval = 0.1f; // 피격시 Player가 반짝거리는 주기
    
    [Header("Player와 무기 사이의 거리")]
    [SerializeField] private float _weaponRadius = 1f; // WeaponPivot과 Player의 거리
    [SerializeField] private Transform _weaponPivot; // 자식 오브젝트 WeaponPivot의 Transform
    [SerializeField] private SpriteRenderer _weaponSprite; //무기 스프라이트
    
    public Vector2 moveInput; // Actions로부터 받을 Vector2값 저장
    private Vector2 _playerPosition; // transform.position이 Vector3이라서 Vector2로 편하게 쓰려고
    private Vector2 _mousePoint; // 마우스 좌표
    
    [SerializeField] private GameObject _bulletPrefab; // 총알 prefab
    [SerializeField] private Transform _muzzlePoint; // 총구 위치

    public Action OnPlayerDead;
    public Action<float, float> OnPlayerHPChanged;

    // State 
    public IdleState idle;
    public MoveState move;
    
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _bodySprite = GetComponent<SpriteRenderer>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        
        _currentHP = _maxHP;
        _invincibleWait =  new WaitForSeconds(_blinkInterval);
        
        _stateMachine = new StateMachine();

        idle = new IdleState(this);
        move = new MoveState(this);
    }

    void Start()
    {
        // 시작시 기본 State는 idle
        _stateMachine.ChangeState(idle);
    }
    void Update()
    {
        if (Time.timeScale == 0) return;
        
        _stateMachine.Update();
        HandleShooting();
    }

    void FixedUpdate()
    {
        if (Time.timeScale == 0) return;
        
        Movement();
    }

    void LateUpdate()
    {
        if (Time.timeScale == 0) return;
        
        LookMouse();
        WeaponRotate();
    }

    void OnEnable()
    {
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;
        _playerInput.actions["Shoot"].started += OnShoot;
        _playerInput.actions["Shoot"].canceled += OnShoot;
    }

    void OnDisable()
    {
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMove;
        _playerInput.actions["Shoot"].started -= OnShoot;
        _playerInput.actions["Shoot"].canceled -= OnShoot;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    
    void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            _isShoting = true;
        }

        if (ctx.canceled)
        {
            _isShoting = false;
        }
    }

    //총 발사 쿨타임
    void HandleShooting()
    {
        if (!_isShoting) return;

        if (Time.time >= _nextFireTime)
        {
            ShootBullet();
            _nextFireTime = Time.time + _fireRate;
        }
    }
    
    // 총알 발사 함수
    void ShootBullet()
    {
        Vector2 dir = _muzzlePoint.right;
        GameObject bullet = Instantiate(_bulletPrefab, _muzzlePoint.position, _muzzlePoint.rotation);
        bullet.GetComponent<BulletFire>().Init(dir, _damage, _bulletSpeed, _bulletDistance);
    }

    // Player의 움직임을 담당하는 함수
    void Movement()
    {
        _rigidbody2D.linearVelocity = moveInput * _playerSpeed;
    }
    
    // Player가 마우스 방향에 따라 스프라이트 좌우 바꿔주는 함수
    void LookMouse()
    {
        // 마우스 좌표값 구하기
        _mousePoint = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue());
        
        // Player의 좌표로부터 마우스 좌표까지의 방향
        _playerPosition = transform.position;
        Vector2 dir = _mousePoint - _playerPosition;
        
        // 방향의 x값이 딱 0(캐릭터 바로 위 혹은 아래)일 경우에는 스프라이트 안 바뀜
        if (dir.x != 0)
        {
            // 방향의 x값이 0보다 작으면 true(좌측 바라봄), 0보다 크면 false(우측 바라봄)
            _bodySprite.flipX = dir.x < 0;
        }
    }
    
    // 무기의 방향을 마우스 좌표에 따라 움직이고 스프라이트를 좌우 바꿔주는 함수
    void WeaponRotate()
    {
        // 마우스 좌표와 Player좌표간의 방향벡터
        Vector2 dir = (_mousePoint - _playerPosition).normalized;

        // Player와 WeaponPivot간의 거리
        _weaponPivot.position = _playerPosition + dir * _weaponRadius;
        
        // 무기(총구, 칼 끝)가 마우스를 바라보도록(조건 : 스프라이트가 오른쪽으로 향하고 있어야 함)
        // transform.right는 로컬 x축을 월드 좌표 기준 벡터로 표현한 값
        _weaponPivot.right = dir;
        
        // 마우스 좌표와 Player좌표간의 방향벡터의 x가 0보다 작을 경우 _weaponPivot 반전
        if (dir.x < 0)
        {
            _weaponPivot.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            _weaponPivot.localScale = new Vector3(1, 1, 1);
        }
    }
    
    public void ChangeState(IState state)
    {
        _stateMachine.ChangeState(state);
    }
    
    // value 값이 0.1 초과시 Move, 0.1 미만이면 Idle 애니메이션 재생
    public void SetMove(float value)
    {
        _animator.SetFloat("Move", Mathf.Abs(value));
    }

    public void TakeDamage(float damage)
    {
        // 무적이면 공격받지 않음
        if (_isInvincible) return;
        
        _currentHP -= damage;
        OnPlayerHPChanged?.Invoke(_currentHP, _maxHP);
        if (_currentHP <= 0)
        {
            Die();
        }
        
        //공격 받으면 _invincibleTime시간 만큼 무적
        StartCoroutine(InvincibleCoroutine());
        Debug.Log(_currentHP);
    }

    public void PlayerUpHP(float value)
    {
        _currentHP += value;
        _maxHP += value;
        OnPlayerHPChanged?.Invoke(_currentHP, _maxHP);
    }
    
    private IEnumerator InvincibleCoroutine()
    {
        _isInvincible = true;

        float timer = 0f;

        // 무적시간 만큼 Player 반짝이는 효과
        while (timer < _invincibleTime)
        {
            _bodySprite.enabled = false;
            yield return _invincibleWait;

            _bodySprite.enabled = true;
            yield return _invincibleWait;

            timer += _blinkInterval * 2;
        }

        _bodySprite.enabled = true;
        _isInvincible = false;
    }

    public void Die()
    {
        OnPlayerDead?.Invoke();
    }
}
