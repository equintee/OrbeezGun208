using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbeezPoolController : MonoBehaviour
{
    private levelController levelController;
    public int bulletPerSecond;

    private void Start()
    {
        levelController = FindObjectOfType<levelController>();
    }

    private float deltaTime = 0f;
    private void OnTriggerStay(Collider other)
    {
        deltaTime += Time.deltaTime;
        if (deltaTime >= 0.1f)
            updateBulletCount(bulletPerSecond);
    }

    public void updateBulletCount(int amount)
    {
        deltaTime = 0f;
        levelController.updateBulletCount(amount);
    }

}
