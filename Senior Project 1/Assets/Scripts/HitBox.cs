using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    Collider2D hitbox;

    public List<Collider2D> Colliders = new List<Collider2D>();

    private void Awake()
    {
        hitbox = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Colliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Colliders.Remove(collision);
    }

}
