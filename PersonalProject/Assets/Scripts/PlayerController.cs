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
    
    public Vector2 _moveInput;
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

    void Movement()
    {
        _rb.linearVelocity = _moveInput * _playerSpeed;
    }
    
    public void ChangeState(IState state)
    {
        _stateMachine.ChangeState(state);
    }
    
    public void SetMove(float value)
    {
        _animator.SetFloat("Move", Mathf.Abs(value));
    }
}
