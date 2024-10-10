using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public enum State { Idle, Walk, Jump, Death, SIZE }

    [SerializeField] AudioSource audioSource;
    [SerializeField] Material material;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    private PlayerState[] playerState = new PlayerState[(int)State.SIZE];
    private State curState = State.Idle;

    private Animator animator;
    private Rigidbody2D rb;

    private int idleHash;
    private int walkHash;
    private int jumpHash;
    private int deathHash;

    private float loud;

    private void Awake()
    {
        playerState[(int)State.Idle] = new IdleState(this);
        playerState[(int)State.Walk] = new WalkState(this);
        playerState[(int)State.Jump] = new JumpState(this);
        playerState[(int)State.Death] = new DeathState(this);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerState[(int)curState].Enter();

        //Mobile 마이크 입력
        audioSource.clip = Microphone.Start(null, true, 10, 44100);

        //마이크 입력 반복
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) ;
        audioSource.Play();

    }

    private void Update()
    {

        playerState[(int)curState].Update();


        loud = GetAverageVolume() * 5f;
        //Debug.Log($"소리:{loud}");

        Debug.DrawRay(transform.position, Vector2.down * 10f, Color.red);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);



    }

    public void ChangeState(State state)
    {
        playerState[(int)curState].Exit();
        curState = state;
        playerState[(int)curState].Enter();
    }


    private float GetAverageVolume()
    {
        float[] data = new float[256];
        float a = 0;

        audioSource.GetOutputData(data, 0);

        foreach (float s in data)
        {
            a += s * s;
        }

        return Mathf.Sqrt(a / data.Length);

    }

    public class PlayerState : StateMachine
    {
        protected PlayerController player;

        public PlayerState(PlayerController player)
        {
            this.player = player;
            this.player.idleHash = Animator.StringToHash("PlayerIdle");
            this.player.walkHash = Animator.StringToHash("PlayerWalk");
            this.player.jumpHash = Animator.StringToHash("PlayerJump");
            this.player.deathHash = Animator.StringToHash("PlayerDeath");
        }


    }

    public class IdleState : PlayerState
    {
        private float walkLimit = 0.1f;
        private float jumpLimit = 0.2f;

        public IdleState(PlayerController player) : base(player) { }

        private Vector2 vec = new Vector2(0.5f, 2f);

        public override void Enter()
        {
            player.animator.Play(player.idleHash);

            Debug.Log("아이들 상태");



        }

        public override void Update()
        {
             
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.rb.AddForce(player.jumpPower * vec, ForceMode2D.Impulse);
            }



            if (player.loud > jumpLimit)
            {
                player.ChangeState(State.Jump);
            }
            else if (player.loud > walkLimit)
            {
                player.ChangeState(State.Walk);
            }

        }

    }

    public class WalkState : PlayerState
    {
        private float idleLimit = 0.09f;
        private float jumpLimit = 0.2f;

        public WalkState(PlayerController player) : base(player) { }

        public override void Enter()
        {
            player.animator.Play(player.walkHash);

            Debug.Log("워크 상태 진입");
        }

        public override void Update()
        {
            //물리 이동으로 변경
            player.transform.Translate(player.moveSpeed * Time.deltaTime * Vector2.right);

            if (player.loud > jumpLimit)
            {
                player.ChangeState(State.Jump);
            }
            else if (player.loud < idleLimit)
            {
                player.ChangeState(State.Idle);
            }
             
        }


    }

    public class JumpState : PlayerState
    {
        private float jumpLimit = 0.2f;

        public JumpState(PlayerController player) : base(player) { }
        public override void Enter()
        {
            player.animator.Play(player.jumpHash);
            Debug.Log("점프 상태 진입");
            

            player.rb.AddForce(player.jumpPower * Vector2.one, ForceMode2D.Impulse);
        }

        public override void Update()
        {
            //rb.velocity 가 0보다 작아지면 Gravity scale을 증가 ??


            if (player.loud < jumpLimit)
            {
                player.ChangeState(State.Idle);
            }

        }

    }

    public class DeathState : PlayerState
    {
        public DeathState(PlayerController player) : base(player) { }
        public override void Enter()
        {
            player.animator.Play(player.deathHash);
            Debug.Log("플레이어 죽음 상태 진입");
        }
    }


}
