using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject objRefCamera;

    public Single offset;
    public Single factor = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        objRefCamera = Camera.main.gameObject;
    }//2.482521

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(offset + objRefCamera.transform.position.x / factor, transform.position.y);
    }
}
