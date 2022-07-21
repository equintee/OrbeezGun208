using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDataHolder : MonoBehaviour
{
    private int health = 2;

    private float moveSpeed;

    private void Start()
    {
        transform.parent.GetComponent<enemyController>().getMoveSpeed();

        //TODO: start running animation for enemy
        //animator.SetTrigger("Run")
    }

    private void Update()
    {
        
    }

    public void animateHit()
    {
        //TODO: hit animations for enemy
        decrementHealth();
        if(health == 1)
        {
            //animator.SetTrigger("Stun")
        }

        if (health == 0)
        {
            //mask move up, animator speed down
            //start roll and fall
        }
    }

    public void decrementHealth()
    {
        health--;
    }

    public int getHealth()
    {
        return health;
    }

   
}
