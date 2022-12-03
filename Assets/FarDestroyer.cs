using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarDestroyer : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        StartCoroutine(FarCheckcer());
    }

    private IEnumerator FarCheckcer()
    {
        yield return new WaitForSeconds(1f);
        if (player.transform.position.x > transform.position.x && Mathf.Abs(player.transform.position.x - transform.position.x) > 20)
            Destroy(gameObject);
        else
            StartCoroutine(FarCheckcer());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
