using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour,Player.INomalActions
{
    [Header("移动参数")]
    public float InputX;
    public float MoveSpeed;
    public float JumpForce;
    public float WallJumpForce;
    public int facingDir = 1;

    [Header("得分")]
    public int Score;

    [Header("碰撞检测")]
    //地面以及墙壁
    public Transform GroundCheck;
    public Transform WallCheck;
    public float GroundCheckRadius;
    public float WallCheckRadius;
    public LayerMask Groundlayer;
    public LayerMask WallLayer;

    //状态
    public bool isGrounded;
    public bool wasGround;
    public bool isTouchWall;
    public int JumpCount;
    public int maxJumpCount;
    public int maxHealth = 3;
    private int currenHealth;
    public float invincibleTime = 1f;
    private bool isInvincible = false;

    //状态机
    public PlayerState currenState;
    public IdleState idleState;
    public RunState runState;
    public JumpState jumpState;
    public FallState fallState;
    public WallJumpState wallJumpState;
    public HurtState hurtState;

    //组件
    public SpriteRenderer sr;
    public Rigidbody2D rig;
    [SerializeField]private Animator anim;                      //让anim可以在窗口可视化
    public Animator Anim => anim;                               //为了状态机可以正常引用anim，但不让状态机修改anim，所以另外设置一个public的Anim指向原本的anim
    private Player input;



    void Awake()
    {
        //初始化组件
        sr = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = new Player();                                       //来自 Unity Input System 自动生成的输入类机制，让代码可以访问你在 Input Actions 里配置的所有按键
        input.Nomal.SetCallbacks(this);                             //把当前脚本注册成输入事件的接收者，Nomal是inputaction里面设置的

        //初始化状态
        idleState = new IdleState(this);
        runState = new RunState(this);
        jumpState = new JumpState(this);
        fallState = new FallState(this);
        wallJumpState = new WallJumpState(this);
        hurtState = new HurtState(this);
        

        //初始状态
        currenState = idleState;                                    //一开始让状态变为idle
    }

    void OnEnable() => input.Enable();                              //当函数体只有一行代码时，可以用 => 简写。当脚本启用时，激活输入系统开始监听按键

    void Start()
    {
        JumpCount = maxJumpCount;
        currenHealth = maxHealth;

    }

    void Update()
    {
        //碰撞检测
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, Groundlayer)!=null;   //isGrounded = Physics2D.Raycast(transform.position, Vector2.down, GroundCheckDistance, Groundlayer);//射线方法
        isTouchWall = Physics2D.OverlapCircle(WallCheck.position, WallCheckRadius, WallLayer);

        //落地重置跳跃次数
        if (isGrounded && !wasGround)
        {
            JumpCount = maxJumpCount;
        }

        wasGround = isGrounded;                                     // 当空中 - 落地一瞬间isGrounded = true，wasGrounded = false   ← 这是上一帧留下来的，此时能触发if (isGrounded && !wasGround)，从而使原本每一帧都检测的isgrounded变为只在落地的一帧

        //更新动画参数
        UpdateAnimationParameters();

        //方向处理
        HandleDirection();

        //状态更新
        currenState.LogicUpdate();                                  //让当前state的update也同步运行，因为state均没有挂载在角色身上，所以最终运行的update其实只有这里

    }

    private void FixedUpdate()
    {
        //物理更新
        currenState.PhysicsUpdate();
        
        //移动处理
        rig.velocity = new Vector2(InputX * MoveSpeed, rig.velocity.y);
    }

    // 更新动画参数
    private void UpdateAnimationParameters()
    {
        anim.SetBool("isGround", isGrounded);
        anim.SetFloat("VelocityY", rig.velocity.y);
        anim.SetBool("isTouchWall", isTouchWall);
        anim.SetFloat("Run", Mathf.Abs(InputX));
    }

    // 处理角色方向
    private void HandleDirection()
    {
        if (InputX != 0)
        {
            facingDir = InputX > 0 ? 1 : -1;
            sr.flipX = facingDir == -1;                                 //如果facingDir = -1 为true
        }
    }

    // 输入处理
    public void OnMove(InputAction.CallbackContext context)
    {
        InputX = context.ReadValue<float>();
    }

    //此方法是直接使用的inputsystem勾选自动生成C#
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;//PressOnly 的确可以省略，但最好保留：


        // 墙跳优先
        if (isTouchWall && !isGrounded)
        {
            SwitchState(wallJumpState);
            return;
        }

        //普通跳跃
        if (isGrounded || JumpCount > 0)
        {
            rig.velocity = new Vector2(rig.velocity.x, JumpForce);
            JumpCount--;
            SwitchState(jumpState);
            anim.SetTrigger(JumpCount == maxJumpCount - 1 ? "Jump" : "DoubleJump");
        }
    }

    //状态切换
    public void SwitchState(PlayerState newState)
    {
        if (currenState != null) currenState.Exit();
        currenState = newState;
        currenState.Enter();
    }

    // 处理落地后的状态切换
    public void HandleLandingState()
    {
        if (Mathf.Abs(InputX) > 0.1f)
            SwitchState(runState);
        else
            SwitchState(idleState);
    }

    // 触发受击状态（默认从左侧受到攻击）
    public void TakeDamage(int damage,Vector2 direction)
    {
        if (isInvincible) return;

        currenHealth -= damage;

        hurtState.SetAttackDirection(direction);

        SwitchState(hurtState);

        if(currenHealth <= 0)
        {
            Die();
        }

        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }

    void Die()
    {
        LevelManager.Instance.GameOver();
    }

    //得分机制
    public void AddScore(int amount)
    {
        Score += amount;
        UIManager.Instance.AddScore(Score);
    }

    private void OnDrawGizmos()
    {
        if (GroundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(WallCheck.position, WallCheckRadius);

    }

    void OnDisable() => input.Disable();// 当脚本禁用时，关闭输入监听


    //这两种方法都需要在Inspector挂载PlayerInput
    //public void OnJump()      //Send Messages 模式 （无需 Inspector 绑定）：需使用On<ActionName>()格式
    //{
    //    Debug.Log("...");
    //}

    //public void Jump()          //Invoke Unity Events 模式 （需要在 Inspector 绑定）：方法名自定义，无严格命名
    //{
    //    Debug.Log("跳跃触发!");
    //}

}