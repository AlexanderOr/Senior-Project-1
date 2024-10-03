using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBehavior : MonoBehaviour
{
    public int EnemyHP = 50;
    private float MoveSpeed = 3f;
    private float LookSpeed = 10f;
    public GameObject Player;
    public HitBox HitBox;
    public bool HasTarget;
    public Animator Animator;

    private Rigidbody2D RB;

    private Vector2 DirectionOfPlayer;
    private Vector2 EnemyToPlayer;
    private float EnemyDistance;
    public float EnemyAttackDistance = 1;

    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isPaused == true)
        {
            RB.velocity = transform.up * 0;
        }
        else if (gameObject.tag == "Frozen")
        {
            RB.velocity = transform.up * 0;
        }
        else
        {
            EnemyDistance = Vector2.Distance(Player.transform.position, transform.position);
            HasTarget = HitBox.Colliders.Count > 0;

            UpdateDirection();
            RotateToPlayer();
            Move();
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
        if(EnemyHP != 0)
        {
            if (HasTarget == true)
            {
                //attack anim and stuff
                Animator.SetBool("InRange", true);
                RB.velocity = transform.up * 0;
                //do damage to player
                playerController.playerHit();
                return;
            }
            else if (HasTarget == false)
            {
                Animator.SetBool("InRange", false);
                RB.velocity = transform.up * MoveSpeed;
            }
        }
        else if (EnemyHP == 0)
        {
            Animator.SetBool("HasHP", false);
            
            Destroy(gameObject);
        }
        
        
    }


}
