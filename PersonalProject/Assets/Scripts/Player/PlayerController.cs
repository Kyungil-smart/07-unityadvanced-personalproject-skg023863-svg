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
    [SerializeField] private float _playerSpeed = 5f; // Player 속도
    [SerializeField] private float _invincibleTime = 0.5f;
    private float _currentHP; // Player 현재 체력
    private bool _isInvincible;
    private WaitForSeconds  _invincibleWait;
    
    [Header("Player와 무기 사이의 거리")]
    [SerializeField] private float _weaponRadius = 1f; // WeaponPivot과 Player의 거리
    [SerializeField] private Transform _weaponPivot; // 자식 오브젝트 WeaponPivot의 Transform
    [SerializeField] private SpriteRenderer _weaponSprite; //무기 스프라이트
    
    public Vector2 moveInput; // Actions로부터 받을 Vector2값 저장
    private Vector2 _playerPosition; // transform.position이 Vector3이라서 Vector2로 편하게 쓰려고
    private Vector2 _mousePoint; // 마우스 좌표
    
    [SerializeField] private GameObject _bulletPrefab; // 총알 prefab
    [SerializeField] private Transform _muzzlePoint; // 총구 위치

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
        _invincibleWait =  new WaitForSeconds(_invincibleTime);
        
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
        _stateMachine.Update();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void LateUpdate()
    {
        LookMouse();
        WeaponRotate();
    }

    void OnEnable()
    {
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;
        _playerInput.actions["Shoot"].performed += OnShoot;
    }

    void OnDisable()
    {
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMove;
        _playerInput.actions["Shoot"].performed -= OnShoot;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }
    
    void OnShoot(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            ShootBullet();
        }
    }
    
    // 총알 발사 함수
    void ShootBullet()
    {
        Vector2 dir = _muzzlePoint.right;
        GameObject bullet = Instantiate(_bulletPrefab, _muzzlePoint.position, _muzzlePoint.rotation);
        bullet.GetComponent<BulletFire>().Init(dir);
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
        
        // 방향의 x값이 딱 0(캐릭터 바로 위)일 경우에는 스프라이트 안 바뀜
        if (dir.x != 0)
        {
            // 방향의 x값이 0보다 작으면 true(좌측 바라봄), 0보다 크면 false(우측 바라봄)
            _bodySprite.flipX = dir.x < 0;
        }
    }
    
    // 무기의 방향을 마우스 좌표에 따라 스프라이트를 좌우 바꿔주는 함수
    void WeaponRotate()
    {
        // 마우스 좌표와 Player좌표간의 방향벡터
        Vector2 dir = (_mousePoint - _playerPosition).normalized;

        // Player와 WeaponPivot간의 거리
        _weaponPivot.position = _playerPosition + dir * _weaponRadius;
        
        // 무기(총구, 칼 끝)가 마우스를 바라보도록(조건 : 스프라이트가 오른쪽으로 향하고 있어야 함)
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
        if (_isInvincible) return;
        
        _currentHP -= damage;
        
        StartCoroutine(InvincibleCoroutine());
        Debug.Log(_currentHP);
        if (_currentHP <= 0)
        {
            Die();
        }
    }

    private IEnumerator InvincibleCoroutine()
    {
        _isInvincible = true;
        // 여기서 깜빡임 효과 추가 가능

        yield return _invincibleWait;

        _isInvincible = false;
        yield break;
    }

    public void Die()
    {
        Debug.Log("Player 사망");
    }
}
