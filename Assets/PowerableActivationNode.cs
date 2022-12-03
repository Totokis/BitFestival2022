using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableActivationNode : MonoBehaviour
{
    public Boolean IsActivated;

    void Start()
    {
        IsActivated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("trig");
        IsActivated = true;
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("trig");

    }
}
