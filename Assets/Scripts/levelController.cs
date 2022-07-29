using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    public GameObject gameplayCanvas;
}
public class levelController : MonoBehaviour
{

    public speedParameters speedParameters;
    public canvasList canvasList;
    public Transform bonusHelicopter;
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


    private void Awake()
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
                canvasList.gameplayCanvas.SetActive(false);
                showUI(canvasList.scoreUI);
                showScore();
                break;
        }

        if (tapToStart && Input.touchCount > 0)
        {
            tapToStart = false;
            canvasList.gameplayCanvas.SetActive(true);
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
        bulletCount = bulletCount < 0 ? 0 : bulletCount;
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
        return platformList.Count == 1;
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

    public async void animateRefill(Transform pool, int bulletCount)
    {

        setIsJumping(false);
        setIsMoving(false);
        setIsShooting(false);

        Transform playerParent = GameObject.FindGameObjectWithTag("Player").transform;
        Transform player = playerParent.transform.GetChild(0);

        player.DOMoveX(pool.position.x, 0.5f).SetEase(Ease.Linear);
        await playerParent.DOMoveZ(pool.position.z, 0.5f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        Debug.Log(pool.localPosition);
        Debug.Log(playerParent.localPosition);
        Debug.Log(player.localPosition);

        animator.SetTrigger("playerRefill");
        await Task.Delay(TimeSpan.FromSeconds(2f));
        updateBulletCount(bulletCount);
        animateMoving();


    }

    public async void animateEnding()
    {
        setIsJumping(false);
        setIsMoving(false);
        setIsShooting(false);
        cameraController.endingAnimationCameraAngle();
        await GameObject.Find("Player").GetComponent<playerController>().animateEnding();

        UIState = 1;
        
    }


    public void changeScene()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        level++;
        level %= SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(level);
    }


    public void showScore()
    {
        int targetScore = UnityEngine.Random.Range(50, 101) * getBulletCount();
        TextMeshProUGUI scoreTMP = canvasList.scoreUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        int currentScore = 0;

        DOTween.To(() => currentScore, x => { currentScore = x; scoreTMP.text = currentScore.ToString(); }, targetScore, 1f).SetEase(Ease.Linear);
            

    }

    
}
