using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDataHolder : MonoBehaviour
{
    private int health = 2;

    private float moveSpeed;
    private float speedDown;
    private float decrementSpeed;
    private bool isDead = false;

    private void Start()
    {
        moveSpeed = transform.parent.GetComponent<enemyController>().getMoveSpeed();
        decrementSpeed = transform.parent.GetComponent<enemyController>().getDecrementSpeed();

        //TODO: start running animation for enemy
        //animator.SetTrigger("Run")
    }
    private void Update()
    {
         if (isDead)
             moveSpeed -= decrementSpeed * Time.deltaTime;
         if (moveSpeed <= 0)
             this.enabled = false;

         moveEnemy();


    }

    public void animateHit()
    {
        //TODO: hit animations for enemy
        decrementHealth();
        if(health == 1)
        {
            transform.GetChild(0).transform.DOMoveY(3, 4f);
            //animator.SetTrigger("Stun")
        }

        if (health == 0)
        {
            //mask move up, animator speed down
            //start roll and fall
            isDead = true;
        }
    }

    private void moveEnemy()
    {
        transform.Translate(new Vector3(0, 0, moveSpeed * Time.deltaTime));
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
