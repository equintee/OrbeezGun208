using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbeezPoolController : MonoBehaviour
{
    private levelController levelController;
    public int bulletCount;

    private void Start()
    {
        levelController = FindObjectOfType<levelController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        levelController.animateRefill(transform, bulletCount);
    }


}
