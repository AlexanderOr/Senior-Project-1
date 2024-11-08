using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public static int Player_MaxHP = 100;
    public static int Player_HP = 100;
    public static int Player_MaxEXP = 50;
    public static int Player_EXP = 0;
    public static int Player_Level = 0;
    bool Invincible = false;
    float Iframes = 0.5f;
    float IframeTimer;
    public float Player_DodgeCD = 5;
    public float DodgeCDTimer;
    public float RollDist = 10;
    bool Immune = false;
    float ImmuneTimer;

    public bool isPaused;

    public EnemyBehavior EnemyBehavior;
    public DodgeCoolDown dodgeCoolDown;

    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";


    [SerializeField] private float MoveSpeed = 5f;

    private Vector2 Movement;
    private Rigidbody2D RB;
    private Animator animator;


    //spells
    public SpellHolder SpellHolder;
    public int currentSpellIndex = 0;

    //Spell Menu
    public SpellMenuManager SpellMenuManager;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //Player = GameObject.FindGameObjectWithTag("Player").transform;
        Player_MaxEXP = 50;
        Player_Level = 0;
        Player_EXP = 0;
        Player_HP = 100;
        Player_MaxHP = 100;
    }

    private void Update()
    {
        //spells
        HandleSpellSelection();
        HandleSpellCasting();

        if (Player_EXP >= Player_MaxEXP)
        {
            LevelUp();
        }


        //movement
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

        if(Invincible == true)
        {
            ApplyIframeCD();
        }

        if(Player_HP > Player_MaxHP)
        {
            Player_HP = Player_MaxHP;
        }

        if (Immune == true)
        {
            ImmuneTimer -= Time.deltaTime;
            Debug.Log(ImmuneTimer);
            Debug.Log(Immune);

            if(ImmuneTimer <= 0) { Immune = false; }
        }

    }

    public void playerHit()
    {
        if (Invincible == false && isPaused == false && Immune == false)
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

        if(collision.tag == "Chest")
        {
            Destroy(collision.gameObject);
            //open spell menu
            SpellHolder.LevelUp();
            //"pause" game
            isPaused = true;
        }

        if(collision.tag == "EnemyBullet")
        {
            Player_HP -= 5;
            Destroy(collision.gameObject);
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
        Debug.Log(IframeTimer);
    }

    void HandleSpellSelection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentSpellIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentSpellIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentSpellIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) currentSpellIndex = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) currentSpellIndex = 4;

    }

    void HandleSpellCasting()
    {
        if (isPaused == false)
        {
            if (Input.GetMouseButtonDown(0) && SpellHolder.playerSpells.Count > currentSpellIndex)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2 = new Vector2(mousePosition.x, mousePosition.y);
                Debug.Log(mousePosition.x + " " + mousePosition.y);

                Vector2 playerPosition = transform.position;
                SpellHolder.playerSpell[currentSpellIndex].CastSpell(mousePos2, playerPosition);

            }
        } 
    }

    void LevelUp()
    {
        //Player_Level++;
        Player_MaxEXP += (10 * Player_Level);
        Player_EXP = 0;
        SpellHolder.LevelUp();

    }

    public void Heal()
    {
        if (Player_HP < Player_MaxHP)
        {
            Player_HP += 15;
        }
        
    }

    public void Teleport(Vector2 mousePos2)
    {
        transform.position = mousePos2;
    }

    public void ImmuneRock(float Duration)
    {
        Immune = true;
        ImmuneTimer = Duration;
    }

    public void TakeDamage(int damage)
    {
        if (Invincible == false && isPaused == false && Immune == false)
        {
            Player_HP -= damage;
            IframeCD();
        }
    }
}
