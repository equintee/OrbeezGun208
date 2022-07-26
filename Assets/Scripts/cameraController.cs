using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public GameObject target;
    public Vector3 shootingPlayerOfset;
    public Vector3 shootingRotation;

    
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
        transform.DOLocalMove(shootingPlayerOfset, 1f).SetEase(Ease.InQuad);
        transform.DOLocalRotate(shootingRotation, 1f).SetEase(Ease.InQuad);
    }
    public void playerFire()
    {
        DOTween.Kill(transform);
        transform.DOLocalMove(shootingPlayerOfset - new Vector3(0, 0, 0.1f), 0.1f).SetEase(Ease.InOutQuint).OnComplete(() => transform.DOLocalMove(shootingPlayerOfset + new Vector3(0, 0, 0.1f), 0.1f).SetEase(Ease.Linear));
        
    }

}