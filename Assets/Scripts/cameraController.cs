using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject target;
    public Vector3 shootingPlayerOfset;
    public Vector3 shootingRotation;

    public Vector3 bonusAnimationPlayerOfset;
    public Vector3 bonusAnimationRotation;
    public float bonusAnimationCameraMovingTime;
    
    private Vector3 movingPosition; //= new Vector3(0.2f, 4.0f, -3.0f);
    private Vector3 movingRotation; //= new Vector3(25.0f, 0.0f, 0.0f);
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        movingPosition = transform.localPosition;
        movingRotation = transform.localRotation.eulerAngles;


    }
    

    public void playerRunningCameraAngle()
    {
        DOTween.Kill(transform);
        transform.DOLocalMove(movingPosition, 1f).SetEase(Ease.InQuad);
        transform.DOLocalRotate(movingRotation, 1f).SetEase(Ease.InQuad);
    }

    public void playerShootingCameraAngle()
    {
        DOTween.Kill(transform);
        transform.DOLocalMove(shootingPlayerOfset, 0.3f).SetEase(Ease.InQuad);
        transform.DOLocalRotate(shootingRotation, 0.3f).SetEase(Ease.InQuad);
    }
    public void playerFire()
    {
        Vector3 temp = Vector3.Normalize(new Vector3(0, Mathf.Cos(shootingRotation.x), Mathf.Sin(shootingRotation.x))) * 0.1f;
        //sin y cos z
        DOTween.Kill(transform);
        transform.DOLocalMove(shootingPlayerOfset - temp, 0.1f).SetEase(Ease.Linear).OnComplete(() => transform.DOLocalMove(shootingPlayerOfset, 0.1f).SetEase(Ease.Linear));

    }

    public void endingAnimationCameraAngle()
    {
        DOTween.Kill(transform);
        transform.DOLocalMove(bonusAnimationPlayerOfset, bonusAnimationCameraMovingTime).SetEase(Ease.OutSine);
        transform.DOLocalRotate(bonusAnimationRotation, bonusAnimationCameraMovingTime).SetEase(Ease.OutSine);
    }

}