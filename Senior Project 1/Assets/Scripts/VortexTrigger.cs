using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class VortexTrigger : MonoBehaviour
{

    public float pullForce = 10f;
    public float Duration = 5f;
    public float Timer = 0f;
    public EXPTracker Exptracker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //StartCoroutine(PullExpObjects(collision.transform));
            EXPTracker.VortexActive = true;
            Debug.Log("vortex on");
            Timer = 0f;
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        if(EXPTracker.VortexActive)
        {
            Timer += Time.deltaTime;


            if (Timer >= Duration)
            {
                EXPTracker.VortexActive = false;
                Debug.Log("vortex off");
            }
        }

        
    }

    IEnumerator PullExpObjects(Transform player)
    {
        GameObject[] expObjects = GameObject.FindGameObjectsWithTag("EXP");

        float timer = 0f;

        while (timer < Duration)
        {
            foreach (GameObject expObject in expObjects)
            {
                if (expObject != null)
                {
                    MoveTowardsPlayer(expObject.transform, player);
                }
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }

    void MoveTowardsPlayer(Transform exp,  Transform player)
    {
        Vector2 newPos = Vector2.MoveTowards(
            exp.position,
            player.position,
            pullForce * Time.deltaTime);

        exp.position = newPos;
    }
}
