using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSpell : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.5f);
        Object.Destroy(this.gameObject);
    }
}
