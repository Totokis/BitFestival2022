using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay : MonoBehaviour
{
    public void Clicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
