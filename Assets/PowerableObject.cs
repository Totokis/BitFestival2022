using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class PowerableObject : MonoBehaviour
{
    public Sprite sprActive;
    public Light2D light;

    static int powerables = 0;
    public Single PositionY;

    public PowerableActivationNode[] ActivationNodes;

    public UnityEvent onPower;

    internal void AttachPowerableActivationNodes(PowerableActivationNode[] nodes)
    {
        ActivationNodes = nodes;
    }

    private void Awake()
    {
        light.gameObject.SetActive(false);
    }
    void Start()
    {
        powerables++;
        name = $"Powerable {powerables}";
    }

    Boolean _performedActivation = false;

    public Boolean WasActivated = false;
    public Boolean AllActivated() => ActivationNodes != null && ActivationNodes.Length > 0 && ActivationNodes.All(an => an.IsActivated);
    void Update()
    {
        if (!_performedActivation && ActivationNodes != null && ActivationNodes.Length > 0)
        {
            Boolean allActivated = ActivationNodes.All(an => an.IsActivated);
            if (allActivated)
            {
                _performedActivation = true;
                PerformActivation();
            }
        }
    }

    private void PerformActivation()
    {
        print("Activated " + name);

        WasActivated = true;
        onPower.Invoke();
        GetComponent<SpriteRenderer>().sprite = sprActive;
        Invoke(nameof(EnableLight), 0.2f);

        Collider2D[] results = new Collider2D[10];
        GetComponent<BoxCollider2D>().OverlapCollider(new ContactFilter2D(), results);
        foreach (var res in results)
        {
            if (res != null)
            {
                res.transform.GetChild(0).gameObject.SetActive(true);
                //res.transform.AddComponent<IncrementalGarbaz>();
            }
        }

        Int32 n = 0;
        if (cables.Count > 0)
            for (Int32 i = cables.Count - 1; i >= 0; i--)
            {

                LeanTween.scale(cables[i], new Vector3(2f, 2f), 0.19f)
                    .setLoopPingPong(1)
                    .setDelay(n * 0.013f);

                cables[i].AddComponent<IncrementalGarbaz>();

                n++;
            }

        //gameObject.AddComponent<IncrementalGarbaz>();
    }

    List<GameObject> cables;
    internal void AttachCables(List<GameObject> thisCables)
    {
        cables = thisCables;
    }

    private void EnableLight()
    {
        light.gameObject.SetActive(true);
    }
}
