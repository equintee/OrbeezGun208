using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectableController : MonoBehaviour
{
    public levelController levelController;
    
    public void updateBulletCount(int amount)
    {
        levelController.updateBulletCount(amount);
    }
}
