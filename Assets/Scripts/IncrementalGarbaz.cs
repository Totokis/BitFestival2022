using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementalGarbaz : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(garbaj), Random.Range(8f, 12f));
    }

    void garbaj()
    {
        Destroy(gameObject);
    }
}
