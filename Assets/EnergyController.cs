using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public const Single GAME_OVER_ENERGY = 0f;

    public Single CurrentEnergy = 50f;

    public GameOver GO;

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnergy = 50f;
    
    }

    Boolean yep = false;
    void Update()
    {
        if(!yep && CurrentEnergy <= GAME_OVER_ENERGY)
        {
            yep = true;
            GO.Show();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ThingToInteract tit = collision.GetComponent<ThingToInteract>();
        if(tit)
        {
            if(tit.Kind == ThingKind.Resistor)
            {
                print("Resistor");
                CurrentEnergy -= 50f;
            }
        }
    }
}
