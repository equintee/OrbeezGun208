using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class renderQueue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (CompareTag("prison"))
            GetComponent<MeshRenderer>().material.renderQueue = 3003;
        else
            GetComponent<MeshRenderer>().material.renderQueue = 3002;
    }

}
