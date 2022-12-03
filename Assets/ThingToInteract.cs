using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum ThingKind
{
    Resistor = 1,
    AdditionalEnergy = 2
}

public class ThingToInteract : MonoBehaviour
{
    public ThingKind Kind;

    internal void Collided()
    {
        if(Kind == ThingKind.AdditionalEnergy)
        {
            transform.GetChild(0).GetComponent<ParticleSystem>().Stop();
            LeanTween.scale(gameObject, Vector3.zero, 0.3f)
                .setEaseOutSine();
            Light2D light2D = transform.GetChild(0).GetChild(0).GetComponent<Light2D>();

            LeanTween.value(gameObject, 1f, 0f, 0.2f)
                .setOnUpdate((Single val) =>
                {
                    light2D.intensity = val;
                })
                .setEaseOutSine();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
