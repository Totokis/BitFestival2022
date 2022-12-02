using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerableObject : MonoBehaviour
{

    static int powerables = 0;
    public Single PositionY;

    public PowerableActivationNode[] ActivationNodes;

    internal void AttachPowerableActivationNodes(PowerableActivationNode[] nodes)
    {
        ActivationNodes = nodes;
    }

    // Start is called before the first frame update
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
        GetComponent<SpriteRenderer>().color = Color.green;

    }
}
