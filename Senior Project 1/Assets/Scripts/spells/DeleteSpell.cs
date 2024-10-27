using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSpell : MonoBehaviour
{
    public float Duration;
    // Start is called before the first frame update
    private void Awake()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(Duration);
        Object.Destroy(this.gameObject);
    }
}
