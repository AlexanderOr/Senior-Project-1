using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class EnemyBehavior : MonoBehaviour
{
    public float EnemyHP, EnemyMaxHP = 50;
    public float MoveSpeed = 3f;
    private float LookSpeed = 10f;
    public GameObject Player;
    public HitBox HitBox;
    public bool HasTarget;
    public Animator Animator;
    bool isBleeding = false;

    private Rigidbody2D RB;

    private Vector2 DirectionOfPlayer;
    private Vector2 EnemyToPlayer;
    private float EnemyDistance;
    public float EnemyAttackDistance = 1;

    public PlayerController playerController;
    public GameObject EXPPrefab;

    [SerializeField] EnemyHPBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        healthBar = GetComponentInChildren<EnemyHPBar>();
    }

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerController = Player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isPaused == true)
        {
            RB.velocity = transform.up * 0;
        }
        else if (playerController.isPaused == false)
        {
            EnemyDistance = Vector2.Distance(Player.transform.position, transform.position);
            
            HasTarget = HitBox.Colliders.Count > 0;

            UpdateDirection();
            RotateToPlayer();
            Move();
        }

        HasTarget = false;


        if (isBleeding == true)
        {
            StartCoroutine(Bleeding());
        }
    }

    private void UpdateDirection()
    {
        EnemyToPlayer = Player.transform.position - transform.position;

        DirectionOfPlayer = EnemyToPlayer.normalized;
    }

    private void RotateToPlayer()
    {
        Quaternion TargetRotation = Quaternion.LookRotation(transform.forward, DirectionOfPlayer);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, LookSpeed);
    
        RB.SetRotation(rotation);
    }

    private void Move()
    {
        if(EnemyHP >= 0 && playerController.isPaused == false)
        {
            if (HasTarget == true)
            {
                /*//attack anim and stuff
                Animator.SetBool("InRange", true);
                RB.velocity = transform.up * 0;
                //do damage to player
                playerController.playerHit();
                return;*/
            }
            else if (HasTarget == false)
            {
                Animator.SetBool("InRange", false);
                RB.velocity = transform.up * MoveSpeed;
            }
        }
        else if (EnemyHP <= 0)
        {
            Animator.SetBool("HasHP", false);
            Instantiate(EXPPrefab, gameObject.transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
        }
        
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Spells")
        {
            if (collision.name == "Pebble(Clone)")
            {
                //knockback
                Destroy(collision.gameObject);
                StartCoroutine(Damage(10));
            }

            if (collision.name == "Thorn Throw(Clone)")
            {
                isBleeding = true;
                Destroy(collision.gameObject);
                StartCoroutine(Damage(10));
            }

            if (collision.name == "Leaf Orb(Clone)")
            {
                isBleeding = true;
                Destroy(collision.gameObject);
                StartCoroutine(Damage(30));
            }

            if (collision.name == "Arcane Missle(Clone)")
            {
                //pen
                StartCoroutine(Damage(10));
            }

            if (collision.name == "Disintegrate(Clone)")
            {
                StartCoroutine(Damage(20));
            }

            if (collision.name == "Snowball(Clone)")
            {
                if (MoveSpeed > 2)
                {
                    MoveSpeed -= 1f;
                }
                Destroy(collision.gameObject);
                StartCoroutine(Damage(10));
            }

            if (collision.name == "Ice Zone(Clone)")
            {
                if (MoveSpeed > 2)
                {
                    MoveSpeed -= 1f;
                }
                StartCoroutine(Damage(20));
            }

            if (collision.name == "FireLance(Clone)")
            {
                Destroy(collision.gameObject);
                StartCoroutine(Damage(20));
            }

            if (collision.name == "center(Clone)")
            {
                StartCoroutine(Damage(10));
            }

            if (collision.name == "Upheaval(Clone)")
            {
                StartCoroutine(Damage(20));
            }


        }

    }

    IEnumerator Damage(int DamageAmount)
    {
        EnemyHP -= DamageAmount;
        healthBar.UpdateHealthBar(EnemyHP, EnemyMaxHP);
        Debug.Log(EnemyHP);
        yield return new WaitForSeconds(0.5f);
    }

    IEnumerator Bleeding()
    {
        EnemyHP -= 5;
        yield return new WaitForSeconds(1f);
        Debug.Log(EnemyHP);
        EnemyHP -= 5;
        Debug.Log(EnemyHP);
        isBleeding = false;
        yield return null;
        Debug.Log(EnemyHP);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //attack anim and stuff
            Animator.SetBool("InRange", true);
            RB.velocity = transform.up * 0;
            //do damage to player
            playerController.playerHit();
            return;
        }
    }
}



/**/