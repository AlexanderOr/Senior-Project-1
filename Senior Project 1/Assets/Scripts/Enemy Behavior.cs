using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyBehavior : MonoBehaviour
{
    private float MoveSpeed = 3f;
    private float LookSpeed = 10f;
    public GameObject Player;
    private Rigidbody2D RB;

    private Vector2 DirectionOfPlayer;
    private Vector2 EnemyToPlayer;
    private float EnemyDistance;
    public float EnemyAttackDistance = 2;



    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyDistance = Vector2.Distance(Player.transform.position, transform.position);
        Debug.Log(EnemyDistance);
        UpdateDirection();
        RotateToPlayer();
        Move();
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
        if(EnemyDistance <= EnemyAttackDistance)
        {
            //attack anim and stuff
            RB.velocity = transform.up * 0;
            return;
        }
        else
        {
            RB.velocity = transform.up * MoveSpeed;
        }
        
    }


}
