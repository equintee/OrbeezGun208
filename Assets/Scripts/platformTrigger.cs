using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<levelController>().setIsShooting(true);
    }

    private void OnTriggerExit(Collider other)
    {
        transform.parent.GetComponent<levelController>().setIsShooting(false);
    }
}
