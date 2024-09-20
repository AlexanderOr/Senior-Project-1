using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int Player_MaxHP = 100;
    int Player_HP = 100;
    int Player_MaxEXP = 100;
    int Player_EXP = 0;
    int Player_Level = 1;
    bool Invincible = false;
    float Player_DodgeCD = 5;
    float DodgeCDTimer;
    float RollDist = 7;

    public EnemyBehavior EnemyBehavior;


    [SerializeField] private float MoveSpeed = 5f;

    private Vector2 Movement;
    private Rigidbody2D RB;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement.Set(InputManager.Movement.x, InputManager.Movement.y);

        RB.velocity = Movement * MoveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && Movement.x != 0 && DodgeCDTimer == 0)
        {
            RB.velocity = new Vector2((Movement.x * MoveSpeed) * RollDist,RB.velocity.y);
            DodgeCDTimer = Player_DodgeCD;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && Movement.y != 0 && DodgeCDTimer == 0)
        {
            RB.velocity = new Vector2(RB.velocity.x, (Movement.y * MoveSpeed) * RollDist);
            DodgeCDTimer = Player_DodgeCD;
        }

        if (DodgeCDTimer > 0)
        {
            DodgeCDTimer -= Time.deltaTime;
        }

        if (DodgeCDTimer < 0)
        {
            DodgeCDTimer = 0;
        }

        Debug.Log(DodgeCDTimer);

        if (Input.GetKeyDown(KeyCode.E))
        {
            EnemyBehavior.EnemyHP = 0;
        }
    }



}
