using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{

    public float moveSpeed;
    public float decrementSpeed;
    public float getMoveSpeed()
    {
        return moveSpeed;
    }

    public float getDecrementSpeed()
    {
        return decrementSpeed;
    }
}
