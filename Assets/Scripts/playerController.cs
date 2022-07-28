using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;
[Serializable]
public struct gameplayEffects
{
    public GameObject baloonEffect;

}
public class playerController : MonoBehaviour
{

    public levelController levelController;
    private GameObject currentPlatform;
    private Transform enemyListOfCurrentPlatform;
    private Transform wallListOfCurrentPlatform;
    private Transform playerModel;
    private Vector3 verticalMovementUnitVector;
    public GameObject bullet;
    public Transform gun;

    public float bonusShootingInterval;

    public gameplayEffects gameplayEffects;
    

    public List<Material> bulletMaterials = new List<Material>();

    private cameraController cameraController;
    private Animator animator;

    private void Start()
    {
        currentPlatform = levelController.getNextPlatform();
        enemyListOfCurrentPlatform = currentPlatform.transform.GetChild(levelController.platformParameters.enemyParentIndex);
        wallListOfCurrentPlatform = currentPlatform.transform.GetChild(levelController.platformParameters.wallParentIndex);
        verticalMovementUnitVectorCalculator();

        playerModel = transform.GetChild(0);

        cameraController = Camera.main.GetComponent<cameraController>();
        animator = transform.GetChild(0).GetComponent<Animator>();

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
                
                if (playerModel.localPosition.x > 4f)
                    playerModel.localPosition = new Vector3(4f, playerModel.localPosition.y, playerModel.localPosition.z);

                if (playerModel.localPosition.x < -4f)
                    playerModel.localPosition = new Vector3(-4f, playerModel.localPosition.y, playerModel.localPosition.z);
            }
        }
        transform.Translate(levelController.speedParameters.verticalSpeed * Time.deltaTime * verticalMovementUnitVector, Space.World);

    }

    private int wallHp = 2;
    private async void shoot()
    {

        foreach (Transform enemy in enemyListOfCurrentPlatform)
        {
            enemy.GetComponent<enemyDataHolder>().enabled = true;
            enemy.GetComponent<Animator>().SetTrigger("enemyRun");
        }
        if(Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                var ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hitinfo;
                if (Physics.Raycast(ray, out hitinfo))
                {
                    generateRandomBullet();
                    if (hitinfo.collider.CompareTag("Enemy"))
                    {
                        GameObject enemy = hitinfo.collider.gameObject;
                        if (enemy.GetComponent<enemyDataHolder>().getHealth() == 0)
                            return;
                        animator.SetTrigger("playerShoot");
                        cameraController.playerFire();

                        GameObject bulletShoot = Instantiate(bullet, gun.position + new Vector3(0, 0, 0.45f), Quaternion.identity, transform);

                        levelController.updateBulletCount(-1);
                        

                        bulletShoot.transform.DOMove(hitinfo.point, 0.3f).OnComplete(() => completeShooting(bulletShoot, enemy));
  
                    }

                    if (hitinfo.collider.CompareTag("Wall"))
                    {
                        if (wallHp <= 0)
                            return;
                        animator.SetTrigger("playerShoot");
                        cameraController.playerFire();
                        wallHp--;

                        GameObject bulletShoot = Instantiate(bullet, gun.position + new Vector3(0, 0, 0.45f), Quaternion.identity, transform);
                        levelController.updateBulletCount(-1);
                        await bulletShoot.transform.DOMove(hitinfo.point, 0.5f).AsyncWaitForCompletion();
                        await bulletShoot.transform.DOScale(new Vector3(2, 2, 2), 0.15f).SetEase(Ease.OutSine).AsyncWaitForCompletion();
                        Destroy(bulletShoot);

                        GameObject explosionEffect = Instantiate(gameplayEffects.baloonEffect, hitinfo.point, Quaternion.identity, currentPlatform.transform);
                        explosionEffect.GetComponent<ParticleSystem>().Play();

                        if (wallHp == 0) {
                            hitinfo.collider.enabled = false;
                            hitinfo.collider.transform.parent = null;
                            foreach (Transform boxTransform in hitinfo.collider.transform)
                            {
                                GameObject box = boxTransform.gameObject;
                                Rigidbody rb = box.AddComponent<Rigidbody>();
                                rb.AddExplosionForce(1000, currentPlatform.transform.position, 5);

                            }
                            Destroy(hitinfo.collider.gameObject, 2f);
                            wallHp = 2;
                        } 
                    }
                }
            }
        }

        if (enemyListOfCurrentPlatform.childCount == 0 && wallListOfCurrentPlatform.childCount == 0)
        {
            levelController.animateMoving();
        }




    }

    private void completeShooting(GameObject bulletShoot, GameObject enemy)
    {
        DOTween.Kill(bulletShoot.transform);
        Destroy(bulletShoot);
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


    private Vector3 landingPoint;
    private Vector3 endOfPlatform;
    private void animateJumping ()
    {
        levelController.setIsJumping(false);
        currentPlatform = levelController.getNextPlatform();
        enemyListOfCurrentPlatform = currentPlatform.transform.GetChild(levelController.platformParameters.enemyParentIndex);
        wallListOfCurrentPlatform = currentPlatform.transform.GetChild(levelController.platformParameters.wallParentIndex);
        verticalMovementUnitVectorCalculator();
        transform.DOJump(landingPoint, 3, 1, 0.5f * 2f).SetEase(Ease.Linear);
        transform.DORotate(currentPlatform.transform.rotation.eulerAngles, 0.5f * 2f).SetEase(Ease.Linear).OnComplete(() => levelController.animateMoving());

    }

    public async void animateEnding()
    {

        GameObject bonusPlatform = levelController.bonusPlatform;
        bonusPlatform.SetActive(true);

        float bonusPlatformMoveOnY = bonusPlatform.transform.lossyScale.y * 7;
        await bonusPlatform.transform.DOLocalMoveY(bonusPlatformMoveOnY, 1).SetEase(Ease.OutQuint).AsyncWaitForCompletion();

        float animationSpeed = 0.4f / bonusShootingInterval;

        animator.speed = animationSpeed;

        while(levelController.getBulletCount() != 0)
        {
            GameObject bulletShoot = Instantiate(bullet, gun.transform.position, Quaternion.identity, gun);
            animator.SetTrigger("bonusShoot");
            bulletShoot.transform.DOMoveY(bulletShoot.transform.position.y - 3, bonusShootingInterval).OnComplete(() => Destroy(bulletShoot));
            levelController.updateBulletCount(-1);
            await transform.DOMoveY(transform.position.y + bonusPlatform.transform.lossyScale.y / 2, bonusShootingInterval).SetEase(Ease.OutQuint).AsyncWaitForCompletion();
        }

       
    }

    private void generateRandomBullet()
    {
        bullet.GetComponent<MeshRenderer>().material = bulletMaterials[UnityEngine.Random.Range(0, bulletMaterials.Count)];
    }
}
