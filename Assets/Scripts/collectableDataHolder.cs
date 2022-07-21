using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectableDataHolder : MonoBehaviour
{
    public int bulletCount;

    private void OnTriggerEnter(Collider other)
    {
        transform.parent.GetComponent<collectableController>().updateBulletCount(bulletCount);
        Destroy(gameObject);

    }
}
