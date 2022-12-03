using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableActivationNode : MonoBehaviour
{
    public Boolean IsActivated;
    public GameObject ActivatedParticles;

    void Start()
    {
        IsActivated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("trig");
        if (!IsActivated)
        {
            IsActivated = true;
            ActivatedParticles.SetActive(true);
            ActivatedParticles.GetComponent<ParticleSystem>().Play();
            //GetComponent<SpriteRenderer>().color = Color.green;
            LeanTween.scale(gameObject, new Vector3(1.8f, 1.81f), 0.18f)
                .setLoopPingPong(1);

            GameObject.FindObjectOfType<EnergyController>().ChangeEnergy(-5f);
        }
    }

}
