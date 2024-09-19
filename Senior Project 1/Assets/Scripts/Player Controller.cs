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
    int Player_DodgeCD = 3;


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
    }


}
