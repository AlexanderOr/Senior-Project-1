using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticalsOverTime : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public float startRadius = 0.01f;
    public float timeVar = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        ParticleSystem.ShapeModule shapeModule = particleSystem.shape;

        timeVar += Time.deltaTime;

        if (timeVar > 0.25f)
        {
            timeVar = 0;
            startRadius += 1;
        }

        shapeModule.radius = startRadius;
    }
}
