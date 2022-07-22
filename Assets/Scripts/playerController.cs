using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class playerController : MonoBehaviour
{

    public levelController levelController;
    private GameObject currentPlatform;
    private Transform playerModel;
    private Vector3 verticalMovementUnitVector;

    private GameObject closestEnemy;

    private void Start()
    {
        currentPlatform = levelController.getNextPlatform();
        verticalMovementUnitVectorCalculator();

        playerModel = transform.GetChild(0);

    }
    void Update()
    {
        //TODO: TapToStart

        if(levelController.getIsMoving())
            playerMovement();

        if (levelController.getIsJumping())
            animateJumping();

        
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
                playerModel.localPosition = new Vector3(playerModel.localPosition.x + touch.deltaPosition.x * levelController.speedParameters.horizantalSpeed, 0, 0);
                
                if (playerModel.localPosition.x > 2f)
                    playerModel.localPosition = new Vector3(2f, playerModel.localPosition.y, playerModel.localPosition.z);

                if (playerModel.localPosition.x < -2f)
                    playerModel.localPosition = new Vector3(-2f, playerModel.localPosition.y, playerModel.localPosition.z);
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
        transform.Translate(levelController.speedParameters.verticalSpeed * Time.deltaTime * verticalMovementUnitVector, Space.World);

    }

    private void verticalMovementUnitVectorCalculator()
    {
        landingPoint = currentPlatform.transform.GetChild(levelController.platformParameters.landingPointIndex).position;
        endOfPlatform = currentPlatform.transform.GetChild(levelController.platformParameters.endingPointIndex).position;

        landingPoint.y += 0.5f;
        endOfPlatform.y += 0.5f;

        verticalMovementUnitVector = Vector3.Normalize(endOfPlatform - landingPoint);
    }

    private void shoot()
    {
        if (levelController.getBulletCount() == 0 || !levelController.getAbleToShoot())
            return;
        var ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hitinfo;
        if (Physics.Raycast(ray, out hitinfo))
        {
            Debug.Log(hitinfo.collider.gameObject.transform.name);
        }


        /*
        enemyController.animateHit();
        levelController.updateBulletCount(-1);*/


    }

    private Vector3 landingPoint;
    private Vector3 endOfPlatform;
    private void animateJumping ()
    {
        levelController.setIsJumping(false);
        currentPlatform = levelController.getNextPlatform();
        verticalMovementUnitVectorCalculator();

        transform.DOJump(landingPoint, 3, 1, 1f);
        transform.DORotate(currentPlatform.transform.rotation.eulerAngles, 1f).OnComplete(() => levelController.setIsMoving(true));

    }


}
