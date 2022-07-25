using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class playerController : MonoBehaviour
{

    public levelController levelController;
    private GameObject currentPlatform;
    private Transform enemyListOfCurrentPlatform;
    private Transform playerModel;
    private Vector3 verticalMovementUnitVector;
    public GameObject bullet;
    private GameObject closestEnemy;


    private void Start()
    {
        currentPlatform = levelController.getNextPlatform();
        enemyListOfCurrentPlatform = currentPlatform.transform.GetChild(levelController.platformParameters.enemyParentIndex);
        verticalMovementUnitVectorCalculator();

        playerModel = transform.GetChild(0);

    }
    void Update()
    {

        if(levelController.getIsMoving())
            playerMovement();

        if (levelController.getIsJumping())
            animateJumping();

        if (levelController.getIsShooting())
            shoot();
        
    }


    private Touch touch;
    private void playerMovement()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                playerModel.localPosition = new Vector3(playerModel.localPosition.x + touch.deltaPosition.x * levelController.speedParameters.horizantalSpeed, 0, 0);
                
                if (playerModel.localPosition.x > 2f)
                    playerModel.localPosition = new Vector3(2f, playerModel.localPosition.y, playerModel.localPosition.z);

                if (playerModel.localPosition.x < -2f)
                    playerModel.localPosition = new Vector3(-2f, playerModel.localPosition.y, playerModel.localPosition.z);
            }
        }
        transform.Translate(levelController.speedParameters.verticalSpeed * Time.deltaTime * verticalMovementUnitVector, Space.World);

    }

    private void shoot()
    {
        foreach (Transform enemy in enemyListOfCurrentPlatform)
            enemy.GetComponent<enemyDataHolder>().enabled = true;
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hitinfo;
                if (Physics.Raycast(ray, out hitinfo))
                {
                    Transform gunTranform = transform.GetChild(0);
                    GameObject bulletShoot = Instantiate(bullet, gunTranform.position + new Vector3(0, 0, 0.45f), Quaternion.identity, gunTranform);
                
                    levelController.updateBulletCount(-1);

                    GameObject enemy = hitinfo.collider.gameObject;

                    bulletShoot.transform.DOMove(enemy.transform.position, 0.3f).OnComplete(() => completeShooting(bulletShoot, enemy));
                }
            }
        }

        if (enemyListOfCurrentPlatform.childCount == 0)
            levelController.animateMoving();
            
    }

    private void completeShooting(GameObject bulletShoot, GameObject enemy)
    {
        bulletShoot.SetActive(false);
        enemy.GetComponent<enemyDataHolder>().animateHit(bulletShoot);
    }

    private void verticalMovementUnitVectorCalculator()
    {
        landingPoint = currentPlatform.transform.GetChild(levelController.platformParameters.landingPointIndex).position;
        endOfPlatform = currentPlatform.transform.GetChild(levelController.platformParameters.endingPointIndex).position;

        landingPoint.y += 0.5f;
        endOfPlatform.y += 0.5f;

        verticalMovementUnitVector = Vector3.Normalize(endOfPlatform - landingPoint);
    }

    /*private void shoot()
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
        levelController.updateBulletCount(-1);


    }*/

    private Vector3 landingPoint;
    private Vector3 endOfPlatform;
    private void animateJumping ()
    {
        levelController.setIsJumping(false);
        currentPlatform = levelController.getNextPlatform();
        enemyListOfCurrentPlatform = currentPlatform.transform.GetChild(levelController.platformParameters.enemyParentIndex);
        verticalMovementUnitVectorCalculator();
        transform.GetChild(0).GetComponent<Animator>().SetTrigger("playerJump");
        transform.DOJump(landingPoint, 3, 1, 0.5f * 2f).SetEase(Ease.Linear);
        transform.DORotate(currentPlatform.transform.rotation.eulerAngles, 0.5f * 2f).SetEase(Ease.Linear).OnComplete(() => levelController.animateMoving());

    }


}
