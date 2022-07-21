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
public struct canvasList
{
    public GameObject bulletCounter;
}
public class levelController : MonoBehaviour
{

    public speedParameters speedParameters;
    public canvasList canvasList;
    public GameObject enemies;

    private Queue<GameObject> enemyQueue;
    private int bullet;
    private GameObject closestEnemy;

    private void Start()
    {
        foreach (Transform enemy in enemies.transform)
            enemyQueue.Enqueue(enemy.gameObject);
    }
    public int getBulletCount()
    {
        return bullet;
    }

    public void updateBulletCount(int amount)
    {
        bullet += amount;
        updateBulletCountCanvas();
    }

    public void updateBulletCountCanvas()
    {
        canvasList.bulletCounter.GetComponent<TextMeshProUGUI>().text = bullet.ToString();
    }

    public void updateClosestEnemy()
    {
        closestEnemy = enemyQueue.Dequeue();
    }

    public GameObject getClosestEnemy()
    {
        return closestEnemy;
    }
}
