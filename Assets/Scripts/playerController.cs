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
    private bool isWallDestroyed = false;
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
                        /*if (enemy.GetComponent<enemyDataHolder>().getHealth() == -1)
                            return;*/
                        enemy.GetComponent<enemyDataHolder>().decrementHealth();
                        animator.SetTrigger("playerShoot");
                        cameraController.playerFire();

                        GameObject bulletShoot = Instantiate(bullet, gun.position + new Vector3(0, 0, 0.45f), Quaternion.identity, transform);

                        levelController.updateBulletCount(-1);
                        

                        bulletShoot.transform.DOMove(hitinfo.point, 0.3f).OnComplete(() => completeShooting(bulletShoot, enemy));
  
                    }

                    if (hitinfo.collider.CompareTag("Wall"))
                    {

                        GameObject bulletShoot = Instantiate(bullet, gun.position + new Vector3(0, 0, 0.45f), Quaternion.identity, transform);
                        levelController.updateBulletCount(-1);
                        
                        animator.SetTrigger("playerShoot");
                        cameraController.playerFire();

                        await bulletShoot.transform.DOMove(hitinfo.point, 0.5f).AsyncWaitForCompletion();
                        await bulletShoot.transform.DOScale(new Vector3(2, 2, 2), 0.15f).SetEase(Ease.OutSine).AsyncWaitForCompletion();
                        Destroy(bulletShoot);
                        GameObject explosionEffect = Instantiate(gameplayEffects.baloonEffect, hitinfo.point, Quaternion.identity, currentPlatform.transform);
                        explosionEffect.GetComponent<ParticleSystem>().Play();

                        animator.SetTrigger("playerShoot");
                        cameraController.playerFire();
                        wallHp--;
                        
                        

                        if (wallHp == 0) {
                            hitinfo.collider.enabled = false;
                            hitinfo.collider.transform.parent = null;
                            foreach (Transform boxTransform in hitinfo.collider.transform)
                            {
                                GameObject box = boxTransform.gameObject;
                                Rigidbody rb = box.AddComponent<Rigidbody>();
                                if (rb == null)
                                    return;
                                Vector3 explosionLocation = new Vector3(hitinfo.collider.transform.position.x, currentPlatform.transform.position.y, hitinfo.collider.transform.position.z);
                                rb.AddExplosionForce(1000, explosionLocation, 8);

                            }
                            Destroy(hitinfo.collider.gameObject, 2f);
                            wallHp = 2;
                            isWallDestroyed = false;
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
        //transform.DOLookAt(currentPlatform.transform.GetChild(levelController.platformParameters.endingPointIndex).transform.position, 1f).SetEase(Ease.Linear).OnComplete(() => levelController.animateMoving());

    }

    public async Task animateEnding()
    {
        currentPlatform = levelController.getNextPlatform();
        await transform.DOJump(currentPlatform.transform.GetChild(0).position, 3, 1, 1f).AsyncWaitForCompletion();
        Transform bonusHelicopter = levelController.bonusHelicopter;
        Transform helicopterRotor = bonusHelicopter.GetChild(2);
        Transform endOfPlatform = currentPlatform.transform.GetChild(levelController.platformParameters.endingPointIndex);
        helicopterRotor.DOLocalRotate(new Vector3(0, 360, 0), 20, RotateMode.FastBeyond360).SetSpeedBased().SetEase(Ease.OutExpo).SetLoops(-1);
        Camera.main.transform.parent = null;
        await playerModel.transform.DOMove(bonusHelicopter.GetChild(bonusHelicopter.childCount - 1).transform.position, 1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        
        playerModel.gameObject.SetActive(false);

        bonusHelicopter.DORotate(new Vector3(10, 0, 0), 1).SetEase(Ease.InQuart);
        bonusHelicopter.DOMoveY(bonusHelicopter.transform.localPosition.y + 5, 2).SetEase(Ease.InQuart);

        await Task.Delay(TimeSpan.FromSeconds(1f));
    }

    private void generateRandomBullet()
    {
        bullet.GetComponent<MeshRenderer>().material = bulletMaterials[UnityEngine.Random.Range(0, bulletMaterials.Count)];
    }
}
