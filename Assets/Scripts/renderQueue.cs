using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renderQueue : MonoBehaviour
{
    void Start()
    {
        if (CompareTag("prison"))
            GetComponent<MeshRenderer>().material.renderQueue = 3003;
        else
            GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }

}
