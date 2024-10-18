using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPTracker : MonoBehaviour
{

    public static bool VortexActive = false;
    public GameObject Player;
    private Transform player;
    public float pullSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        player = Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (VortexActive == true)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, player.position, pullSpeed * Time.deltaTime);

            transform.position = newPos;
        }
    }
}
