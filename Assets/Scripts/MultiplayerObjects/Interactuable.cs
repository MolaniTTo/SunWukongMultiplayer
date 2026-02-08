using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
//using Photon.Pun;

public class Interactuable : MonoBehaviour
{
    [Header("Rotacion y movimiento del objeto interactuable")]
    public Vector3 _rotationAmount;
    public Vector3 RotationAmount { get { return _rotationAmount; } set { _rotationAmount = value; } } //si volem que rote

    public Vector3 _moveAmount;
    public Vector3 MoveAmount { get { return _moveAmount; } set { _moveAmount = value; } } //si volem que es mogui

    [Header("Condiciones SOLO para plataformas verticales!")]
    public Vector3 VerticalRotation;
    public Vector3 HorizontalRotation;

    [Header("Parametros animacion")]
    public float duration = 0.6f; //duracio de l'animacio
    public bool useRotation = true; //si cal que roti o no
    public bool useMove = true;
    public bool calledByButton = true; //si ha estat cridat per un boto
    public bool useToggle; //si volem alternar entre dos estats
    private bool toggled = false; //si volem alternar entre dos estats
    public bool isEnabled = true; //desactivar el objeto interactuable
    public bool shake;
    public bool isPlayerNecessary = false;
    public bool isPlayer = false;
    public Animator animatorController; //animacio que es vol reproduir


    [Header("Photon")]
    //public PhotonView photonView;

    private Quaternion initialRotation;

    //public CameraShakeController cameraShakeController;


    private void Start()
    {
        initialRotation = transform.rotation;
    }

    public void UpdateAnimator(bool isEnabled)
    {
        if (animatorController != null)
        {
            animatorController.SetBool("Enabled", isEnabled);
        }
    }

    public void Interact()
    {

        if (!isEnabled)
        {
            return;
        }
        /*if (photonView != null && !photonView.IsMine)
        {
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            Debug.Log("Transfering ownership to " + PhotonNetwork.LocalPlayer);
        }*/

        //if (shake) { cameraShakeController.Shake(); }

        transform.DOComplete();

        if (calledByButton)
        {
            if (useRotation && !useMove)
            {
                transform.DORotate(useToggle && toggled ? initialRotation.eulerAngles : initialRotation.eulerAngles + _rotationAmount, duration, RotateMode.FastBeyond360).SetEase(Ease.OutBack);
            }
            else if (useMove && !useRotation)
            {
                transform.DOMove(useToggle && toggled ? transform.position - _moveAmount : transform.position + _moveAmount, duration).SetEase(Ease.OutBack);
            }
            else if (useMove && useRotation)
            {
                transform.DORotate(useToggle && toggled ? initialRotation.eulerAngles : initialRotation.eulerAngles + _rotationAmount, duration, RotateMode.FastBeyond360).SetEase(Ease.OutBack);
                transform.DOMove(useToggle && toggled ? transform.position - _moveAmount : transform.position + _moveAmount, duration).SetEase(Ease.OutBack);
            }
            else if (animatorController != null)
            {
                animatorController.SetTrigger("Final");
            }
        }

        else
        {
            if (useRotation && !useMove) //si nomes volem que roti
            {
                transform.DORotate(toggled ? Vector3.zero : _rotationAmount, duration, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack);
            }
            else if (useMove && !useRotation) //si nomes volem que es mogui
            {
                transform.DOMove(toggled ? transform.position - _moveAmount : transform.position + _moveAmount, duration).SetEase(Ease.OutBack);
            }

            else if (useMove && useRotation) //si volem que es mogui i roti
            {
                transform.DORotate(toggled ? Vector3.zero : _rotationAmount, duration, RotateMode.WorldAxisAdd).SetEase(Ease.OutBack);
                transform.DOMove(toggled ? transform.position - _moveAmount : transform.position + _moveAmount, duration).SetEase(Ease.OutBack);
            }
        }

        if (useToggle)
        {
            toggled = !toggled;
        }
    }
}
