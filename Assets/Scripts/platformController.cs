using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformController : MonoBehaviour
{
    [SerializeField]
    private levelController levelController;


    public void setAbleToShoot(bool value)
    {
        levelController.setAbleToShoot(value);
    }
}
