using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDataHolder : MonoBehaviour
{
    private int health = 2;

    private float moveSpeed;
    private float speedDown;
    private float decrementSpeed;
    private bool isDead = false;
    private bool headShot;
    private Animator animator;

    private Transform playerModel;
    private void Start()
    {
        moveSpeed = transform.parent.GetComponent<enemyController>().getMoveSpeed();
        decrementSpeed = transform.parent.GetComponent<enemyController>().getDecrementSpeed();
        animator = GetComponent<Animator>();

        playerModel = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);

        moveEnemy();
        //TODO: start running animation for enemy
        //animator.SetTrigger("Run")
    }
    

    public void animateHit(GameObject prison)
    {
        //TODO: hit animations for enemy
        if (isDead)
            return;

        if(health == 1)
        {
            DOTween.Kill(transform);
            animator.SetTrigger("enemyHit");
            Invoke("moveEnemy", 1f);
        }

        if (health <= 0)
        {
            //mask move up, animator speed down
            //start roll and fall
            isDead = true;
            transform.GetComponent<BoxCollider>().enabled = false;
            this.enabled = false;
            DOTween.Kill(transform);

            Vector3 prisonLocation = new Vector3(0, 0.4f, 0) + transform.position;

            prison = Instantiate(prison, prisonLocation, Quaternion.identity, transform.parent.parent);
            prison.transform.localScale = Vector3.zero;
            

            growPrison(prison);

        }

        Debug.Log(health);
    }

    private async void growPrison(GameObject prison)
    {
        prison.SetActive(true);
        DOTween.To(animationSpeed => GetComponent<Animator>().speed = animationSpeed, 1, 0, 0.3f);
        await prison.transform.DOScale(new Vector3(3, 3, 3), 0.3f).AsyncWaitForCompletion();
        prison.transform.localEulerAngles = new Vector3(0, 0, 0);

        if(transform.localPosition.x >= 0)
        {
            transform.parent = prison.transform;
            /*prison.transform.DOLocalRotate(new Vector3(0, 0, -720), 6f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
            await prison.transform.DOLocalMoveX(4.5f, 5f).SetSpeedBased().SetEase(Ease.Linear).AsyncWaitForCompletion();
            prison.transform.DOLocalMoveY(transform.position.y - 4, 5f).SetSpeedBased().SetEase(Ease.Linear);
            await prison.transform.DOLocalMoveX(7.5f, 5f).SetSpeedBased().SetEase(Ease.Linear).AsyncWaitForCompletion();*/
            await prison.transform.DOMoveY(prison.transform.position.y + 5, 1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        }
        else
        {
            transform.parent = prison.transform;
            /*prison.transform.DOLocalRotate(new Vector3(0, 0, 720), 6f, RotateMode.FastBeyond360).SetEase(Ease.Linear);
            await prison.transform.DOLocalMoveX(-4.5f, 5f).SetSpeedBased().SetEase(Ease.Linear).AsyncWaitForCompletion();
            prison.transform.DOLocalMoveY(transform.position.y - 4, 3f).SetEase(Ease.Linear).SetSpeedBased();
            await prison.transform.DOLocalMoveX(-7.5f, 5f).SetSpeedBased().SetEase(Ease.Linear).AsyncWaitForCompletion();*/
            await prison.transform.DOMoveY(prison.transform.position.y + 5, 1f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        }

        DOTween.Kill(prison.transform);
        Destroy(prison);
    }

    private void moveEnemy()
    {
        if (!isDead)
        {
            GetComponent<Animator>().SetTrigger("enemyRun");
            transform.DOMove(playerModel.position, moveSpeed).SetSpeedBased();
        }
    }

    public void decrementHealth()
    {
        if (headShot)
            health -= 2;
        else
            health--;
    }

    public int getHealth()
    {
        return health;
    }

    

    
}
