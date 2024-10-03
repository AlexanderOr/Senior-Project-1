using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    int Player_MaxHP = 100;
    public int Player_HP = 100;
    int Player_MaxEXP = 100;
    public int Player_EXP = 0;
    public int Player_Level = 1;
    bool Invincible = false;
    float Iframes = 1;
    float IframeTimer;
    public float Player_DodgeCD = 5;
    public float DodgeCDTimer;
    float RollDist = 7;

    public bool isPaused;

    public EnemyBehavior EnemyBehavior;
    public DodgeCoolDown dodgeCoolDown;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";


    [SerializeField] private float MoveSpeed = 5f;

    private Vector2 Movement;
    private Rigidbody2D RB;
    private Animator animator;


    //Spell System
    public BasicSpells[] AllSpells;
    public BasicSpells[] PlayerSpells;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isPaused == false)
        {
            Movement.Set(InputManager.Movement.x, InputManager.Movement.y);

            RB.velocity = Movement * MoveSpeed;

            animator.SetFloat(Horizontal, Movement.x);
            animator.SetFloat(Vertical, Movement.y);

            if (Input.GetKeyDown(KeyCode.Space) && Movement.x != 0 && DodgeCDTimer == 0)
            {
                RB.velocity = new Vector2((Movement.x * MoveSpeed) * RollDist, RB.velocity.y);
                DodgeCDTimer = Player_DodgeCD;
                dodgeCoolDown.UseDodge();
            }
            else if (Input.GetKeyDown(KeyCode.Space) && Movement.y != 0 && DodgeCDTimer == 0)
            {
                RB.velocity = new Vector2(RB.velocity.x, (Movement.y * MoveSpeed) * RollDist);
                DodgeCDTimer = Player_DodgeCD;
                dodgeCoolDown.UseDodge();
            }

            if (DodgeCDTimer > 0)
            {
                DodgeCDTimer -= Time.deltaTime;
            }

            if (DodgeCDTimer < 0)
            {
                DodgeCDTimer = 0;
            }



            if (Player_HP <= 0)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
        else if(isPaused == true)
        {
            RB.velocity = Vector2.zero;
        }
        

        //kill Enemy
        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyBehavior.EnemyHP = 0;
        }

        if(Invincible)
        {
            ApplyIframeCD();
        }
    }

    public void playerHit()
    {
        if (Invincible == false && isPaused == false)
        {
            Debug.Log("hit");
            Player_HP = Player_HP - 5;
            IframeCD();
            Debug.Log(Player_HP);            
        }        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EXP")
        {
            Destroy(collision.gameObject);
            Player_EXP += 5;
        }
    }

    public void IframeCD()
    {
        IframeTimer = Iframes;
        Invincible = true;
    }

    public void ApplyIframeCD()
    {
        IframeTimer -= Time.deltaTime;

        if( IframeTimer < 0 )
        {
            Invincible = false;
        }
    }
}
