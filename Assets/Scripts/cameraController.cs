using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject target;
    public float playerOfset;

    private Vector3 velocity = Vector3.zero;


    void LateUpdate()
    {
        Vector3 temp = target.transform.position;
        temp.x = transform.position.x;

        transform.position = Vector3.SmoothDamp(transform.position, temp, ref velocity, 0.125f);
    }
}