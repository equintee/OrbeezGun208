using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject target;
    public Vector3 playerOfset;
    private void Start()
    {
        transform.position = target.transform.position + playerOfset;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.transform.position + playerOfset;
        Vector3 smoothendPosition = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10f);
        transform.position = smoothendPosition;
    }
}