using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public levelController levelController;

    private bool isPlayerMoving = true;

    private GameObject closestEnemy;
    void Update()
    {
        //TapToStart

        if(isPlayerMoving)
            playerMovement();

        
    }


    private Touch touch;
    private float deltaTime = 0f;
    private void playerMovement()
    {

        if (Input.touchCount > 0)
        {
            deltaTime += Time.deltaTime;

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.Translate(touch.deltaPosition.x * levelController.speedParameters.horizantalSpeed, 0, 0);

                if (transform.position.x > 4.5f)
                    transform.position = new Vector3(4.5f, transform.position.y, transform.position.z);

                if (transform.position.x < -4.5f)
                    transform.position = new Vector3(-4.5f, transform.position.y, transform.position.z);
            }
            else if(touch.phase == TouchPhase.Canceled)
            {
                if (deltaTime < 0.1f)
                {
                    deltaTime = 0f;
                    shoot();
                }
            }

        }

        transform.Translate(0, 0, levelController.speedParameters.verticalSpeed * Time.deltaTime);
    }

    private void shoot()
    {

        if (levelController.getBulletCount() == 0)
            return;
        enemyDataHolder enemyController = levelController.getClosestEnemy().GetComponent<enemyDataHolder>();
        enemyController.animateHit();
        levelController.updateBulletCount(-1);

        if (enemyController.getHealth() == 0)
            levelController.updateClosestEnemy();
        
    }
}
