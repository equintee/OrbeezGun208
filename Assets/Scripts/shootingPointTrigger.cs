using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootingPointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        GameObject.Find("Level Controller").GetComponent<levelController>().animateShooting();
    }
}
