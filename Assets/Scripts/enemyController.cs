using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{

    public float moveSpeed;
    public float decrementSpeed;

    private void Start()
    {
        foreach(Transform enemy in transform)
        {
            enemy.parent = null;
            enemy.localScale = Vector3.one;
            enemy.parent = transform;
        }
    }
    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public float getDecrementSpeed()
    {
        return decrementSpeed;
    }
}
