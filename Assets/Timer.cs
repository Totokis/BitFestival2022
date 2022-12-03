using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI txtTime;
    public Int32 monospace = 20;
    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        txtTime.text = $"<mspace=mspace={monospace}>{time.ToString("N2")}</mspace>";

    }
    Single time;
    private void FixedUpdate()
    {
        txtTime.text = $"<mspace=mspace={monospace}>{time.ToString("N2")}</mspace>";    
        time += Time.deltaTime;
    }
}
