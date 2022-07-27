using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endOfPlatformJump : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        levelController levelController = GameObject.Find("Level Controller").GetComponent<levelController>();
        if (levelController.isLastPlatform())
            levelController.animateEnding();
        else
            levelController.animateJump();
    }
}
