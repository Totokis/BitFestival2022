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
        IsActivated = true;
        GetComponent<SpriteRenderer>().color = Color.green;
    }
}
