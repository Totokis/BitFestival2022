using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class EnergyController : MonoBehaviour
{
    public ParticleSystem EnergyIndicator;
    public const Single GAME_OVER_ENERGY = 0f;

    public Single CurrentEnergy;
    public const Single MAX_ENERGY = 100f;

    public GameOver GO;
    public ParticleSystem pssparks;
    public ParticleSystem redSparks;

    private Vector3 _initialScale;
    void Start()
    {
        EnergyIndicator.Stop();
        _initialScale = pssparks.transform.localScale;
        CurrentEnergy = 74;
        if (FindObjectOfType<GameOver>())
            GO = FindObjectOfType<GameOver>();

        redSparks.Stop();
    }

    public void ChangeEnergy(Boolean isBura, Single change)
    {
        if (isBura && change < 0)
        {
            print("BURA " + change);

            LeanTween.value(4.8f, 5f, 0.13f)
                .setLoopPingPong(1)
                .setEaseInElastic()
                //.setEaseShake()
                .setOnUpdate((Single val) =>
                {
                    Camera.main.orthographicSize = val;
                    //Camera.main.
                });

            StartCoroutine(Damage());
        }

        CurrentEnergy += change;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, Single.MinValue, MAX_ENERGY);
        print("Current energy " + CurrentEnergy);

        if (change > 0)
        {
            EnergyIndicator.Play();
            EnergyIndicator.Stop();
        }
    }

    private IEnumerator Damage()
    {
        pssparks.Stop();
        redSparks.Play();
        yield return new WaitForSeconds(0.13f);
        pssparks.Play();
        redSparks.Stop();
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
                ChangeEnergy(true, -15f);
            }
            else if (tit.Kind == ThingKind.AdditionalEnergy)
            {
                //print("Energy");
                ChangeEnergy(false, 20f);
            }
        }
    }
}
