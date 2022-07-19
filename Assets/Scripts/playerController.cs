using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public levelController levelController;

    // Update is called once per frame
    void Update()
    {
        playerMovement();
    }


    private Touch touch;

    private void playerMovement()
    {

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.Translate(touch.deltaPosition.x * levelController.speedParameters.horizantalSpeed, 0, 0);

                if (transform.position.x > 4.5f)
                    transform.position = new Vector3(4.5f, transform.position.y, transform.position.z);

                if (transform.position.x < -4.5f)
                    transform.position = new Vector3(-4.5f, transform.position.y, transform.position.z);
            }
        }

        transform.Translate(0, 0, 20 * Time.deltaTime/2);
    }
}
