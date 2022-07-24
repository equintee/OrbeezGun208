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
}

[System.Serializable]
public struct canvasList
{
    public GameObject bulletCounter;
}
public class levelController : MonoBehaviour
{

    public speedParameters speedParameters;
    public canvasList canvasList;
    public platformParameters platformParameters;

    private int bulletCount;
    private Queue<GameObject> platformList = new Queue<GameObject>();
    

    private bool isShooting;
    private bool isMoving = true;
    private bool isJumping;



    private void Start()
    {

        foreach (Transform platform in platformParameters.platformParent.transform)
            platformList.Enqueue(platform.gameObject);

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
        setIsShooting(false);
        setIsMoving(false);
        setIsJumping(true);

    }

    public void animateShooting()
    {
        setIsShooting(true);
        setIsMoving(false);
        setIsJumping(false);
    }
}
