using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerableObject : MonoBehaviour
{
    public Sprite sprActive;
    public Light2D light;

    static int powerables = 0;
    public Single PositionY;

    public PowerableActivationNode[] ActivationNodes;

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
        GetComponent<SpriteRenderer>().sprite = sprActive;
        light.gameObject.SetActive(true);
    }
}
