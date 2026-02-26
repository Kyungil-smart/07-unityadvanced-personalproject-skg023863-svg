using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 컴포넌트
    private Rigidbody2D _rb;
    private SpriteRenderer _bodySprite;
    private PlayerInput _playerInput; 
    private Animator _animator;
    private StateMachine _stateMachine;
    
    public Vector2 _moveInput; // Actions로부터 받을 Vector2 값 저장
    [SerializeField] private float _playerSpeed = 5f; // Player 속도

    // State 
    public IdleState idle;
    public MoveState move;
    
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _bodySprite = GetComponent<SpriteRenderer>();
        _playerInput = GetComponent<PlayerInput>();
        _animator = GetComponent<Animator>();
        
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
        LookMouse();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void OnEnable()
    {
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;
    }

    void OnDisable()
    {
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMove;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    // Player의 움직임 함수
    void Movement()
    {
        _rb.linearVelocity = _moveInput * _playerSpeed;
    }
    
    // Player가 마우스 방향에 따라 스프라이트 좌우 바꿔주는 함수
    void LookMouse()
    {
        // 마우스 좌표값
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue());
        
        // Player의 좌표로부터 마우스 좌표까지의 방향
        Vector2 dir = mousePoint - (Vector2)transform.position;
        
        // 방향벡터의 x딱 0(캐릭터 바로 위)일 경우에는 스프라이트 안 바뀜
        if (dir.x != 0)
        {
            // 방향벡터의 x가 0보다 작으면 true(좌측 바라봄), 0보다 크면 false(우측 바라봄)
            _bodySprite.flipX = dir.x < 0;
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
}
