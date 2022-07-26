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
        levelController.animateRefill();
        Invoke("endRefill", 2f);
    }

    private void endRefill()
    {
        levelController.updateBulletCount(bulletCount);
        levelController.animateMoving();
    }

}
