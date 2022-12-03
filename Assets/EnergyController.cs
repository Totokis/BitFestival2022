using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public const Single GAME_OVER_ENERGY = 0f;

    public Single CurrentEnergy;
    public const Single MAX_ENERGY = 100f;

    public GameOver GO;
    public ParticleSystem pssparks;

    private Vector3 _initialScale;
    void Start()
    {
        _initialScale = pssparks.transform.localScale;
        CurrentEnergy = 74;
        if (FindObjectOfType<GameOver>())
            GO = FindObjectOfType<GameOver>();
    }

    public void ChangeEnergy(Single change)
    {
        //if(change < 0)
        //{
        //    LeanTween.value()
        //}

        CurrentEnergy += change;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, Single.MinValue, MAX_ENERGY);
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
        Single scaleFactor = Mathf.Clamp01(CurrentEnergy / MAX_ENERGY * 2f);

        pssparks.transform.localScale = _initialScale * scaleFactor;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ThingToInteract tit = collision.GetComponent<ThingToInteract>();
        if (tit)
        {
            tit.Collided();
            if (tit.Kind == ThingKind.Resistor)
            {
                //print("Resistor");
                ChangeEnergy(-15f);
            }
            else if (tit.Kind == ThingKind.AdditionalEnergy)
            {
                //print("Energy");
                ChangeEnergy(20f);
            }
        }
    }
}
