using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public const Single GAME_OVER_ENERGY = 0f;

    public Single CurrentEnergy;

    public GameOver GO;

    // Start is called before the first frame update
    void Start()
    {
        CurrentEnergy = 10000;

    }

    public void ChangeEnergy(Single change)
    {
        CurrentEnergy += change;
        print("Current energy " + CurrentEnergy);
    }

    Boolean yep = false;
    void Update()
    {
        if (!yep && CurrentEnergy <= GAME_OVER_ENERGY)
        {
            yep = true;
            GO.Show();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ThingToInteract tit = collision.GetComponent<ThingToInteract>();
        if (tit)
        {
            tit.Collided();
            if (tit.Kind == ThingKind.Resistor)
            {
                print("Resistor");
                ChangeEnergy(-50f);
            }
            else if (tit.Kind == ThingKind.AdditionalEnergy)
            {
                print("Energy");
                ChangeEnergy(10f);
            }
        }
    }
}
