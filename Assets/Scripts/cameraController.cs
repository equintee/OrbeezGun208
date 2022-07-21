using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject target;
    public Vector3 playerOfset;

    private Vector3 velocity = Vector3.zero;
    private void Start()
    {
        transform.position = target.transform.position + playerOfset;
    }

    void LateUpdate()
    {

        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position + playerOfset, ref velocity, 0.125f);
    }
}