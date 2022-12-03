using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotLitUpPowerablesDetector : MonoBehaviour
{

    MapGenerator mg;
    // Start is called before the first frame update
    void Start()
    {
        mg = FindObjectOfType<MapGenerator>();
        StartCoroutine(buraChecker());

    }

    private IEnumerator buraChecker()
    {
        foreach (var power in mg.GeneratedPowerables.Where(p => p))
        {
            PowerableObject po = power.GetComponent<PowerableObject>();
            if (!po.BuraWasApplied && power.transform.position.x + 3f < transform.position.x && !po.WasActivated)
            {
                //print(power.name + " Not activated");
                po.BuraWasApplied = true;
                GameObject.FindObjectOfType<EnergyController>().ChangeEnergy(true, -25f);
            }
        }
        mg.DeleteNonExistentPowerables();

        yield return new WaitForSeconds(0.420f);
        StartCoroutine(buraChecker());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (LayerMask.LayerToName(collision.gameObject.layer) == "Detector")
        //    return;

        //if (collision.GetComponent<PowerableObject>())
        //{
        //    if (!collision.GetComponent<PowerableObject>().WasActivated)
        //    {
        //        print(collision.name + " Not activated");
        //        GameObject.FindObjectOfType<EnergyController>().ChangeEnergy(-25f);
        //    }
        //}
    }
}
