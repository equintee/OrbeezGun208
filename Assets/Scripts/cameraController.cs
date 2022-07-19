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
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, target.transform.position.y + playerOfset.y, target.transform.position.z + playerOfset.z), Time.deltaTime);
    }
}