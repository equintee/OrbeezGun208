using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct speedParameters
{
    public float verticalSpeed;
    public float horizantalSpeed;
}

[System.Serializable]
public struct platformParameters
{
    public GameObject platformParent;
    public int landingPointIndex;
    public int endingPointIndex;
    public int enemyParentIndex;
    public int wallParentIndex;
}

[System.Serializable]
public struct canvasList
{
    public GameObject bulletCounter;
    public GameObject tapToStart;
    public GameObject scoreUI;
}
public class levelController : MonoBehaviour
{

    public speedParameters speedParameters;
    public canvasList canvasList;
    public platformParameters platformParameters;

    private int bulletCount;
    private Queue<GameObject> platformList = new Queue<GameObject>();
    private Transform enemyList;

    private bool isShooting;
    private bool isMoving;
    private bool isJumping;

    private int UIState = 0;
    private bool tapToStart = true;
    private cameraController cameraController;
    private Animator animator;

    private void Start()
    {
        foreach (Transform platform in platformParameters.platformParent.transform)
            platformList.Enqueue(platform.gameObject);

        cameraController = Camera.main.GetComponent<cameraController>();
        animator = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        switch (UIState)
        {
            case 0:
                showUI(canvasList.tapToStart);
                break;
            case 1:
                showUI(canvasList.scoreUI);
                break;
        }

        if (tapToStart && Input.touchCount > 0)
        {
            tapToStart = false;
            canvasList.tapToStart.SetActive(false);
            animateMoving();
        }

    }
    public int getBulletCount()
    {
        return bulletCount;
    }

    public void updateBulletCount(int amount)
    {
        bulletCount += amount;
        updateBulletCountCanvas();
    }

    public void updateBulletCountCanvas()
    {
        canvasList.bulletCounter.GetComponent<TextMeshProUGUI>().text = bulletCount.ToString();
    }


    public GameObject getNextPlatform()
    {
        return platformList.Dequeue();
    }

    public bool isLastPlatform()
    {
        return platformList.Count == 0;
    }
    public bool getIsMoving()
    {
        return isMoving;
    }

    public void setIsMoving(bool value)
    {
        isMoving = value;
    }

    public bool getIsShooting()
    {
        return isShooting;
    }

    public void setIsShooting(bool value)
    {
        isShooting = value;
    }

    public void setIsJumping(bool value)
    {
        isJumping = value;
    }

    public bool getIsJumping()
    {
        return isJumping;
    }

    public void animateJump()
    {
        animator.SetTrigger("playerJump");
        setIsShooting(false);
        setIsMoving(false);
        setIsJumping(true);

    }

    public void animateShooting()
    {
        cameraController.playerShootingCameraAngle();
        animator.SetTrigger("playerShootIdle");
        setIsShooting(true);
        setIsMoving(false);
        setIsJumping(false);
    }

    public void animateMoving()
    {
        cameraController.playerRunningCameraAngle();
        animator.SetTrigger("playerRun");
        setIsMoving(true);
        setIsShooting(false);
        setIsJumping(false);
    }

    public void showUI(GameObject canvas)
    {
        UIState = -1;
        canvas.SetActive(true);
        setIsJumping(false);
        setIsMoving(false);
        setIsJumping(false);
    }

    public void animateRefill()
    {
        animator.SetTrigger("playerRefill");
        setIsJumping(false);
        setIsMoving(false);
        setIsShooting(false);

    }

    public void animateEnding()
    {
        GameObject.Find("Player").GetComponent<playerController>().animateEnding(); 
        setIsJumping(false);
        setIsMoving(false);
        setIsShooting(false);
    }
}
