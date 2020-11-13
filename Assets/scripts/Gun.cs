using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private enum GunType
    {
        automatic = 0,
        SemiAutomatic,
    }
    [Header("gun Ammo")]
    [SerializeField] private int _MaxAmmo;
    [SerializeField] private int _CurrentAmmo;
    [Header("gun Data")]
    [SerializeField] private GunType _GunType;
    [SerializeField] private Transform _LeftlockPosition;
    [SerializeField] private Transform _RightlockPosition;
    [Header("shooting")]
    [SerializeField] private Transform _shootpoint;
    [SerializeField] private bool _ReadyToShoot = true;
    [SerializeField] private bool _triggerIn = false;
    [Header("Animation")]
    [SerializeField] private Animator _animator;
    [SerializeField] private Vector3 _EjectDeraction;
    [SerializeField] private float _EjectSpeed;
    [SerializeField] private Transform _EjectPort;
    [SerializeField] private GameObject _EmptyShell;
    [Header("rigidbody")]
    [SerializeField] private float _Inpact = 10f;
    [SerializeField] private float _dammageMultiply = 1f;
    [Header("UI")]
    [SerializeField] private GunCanvasUpdate _GunCanvas;

    public Transform GetLeftLockPosition
    {
        get 
        {
            if (_GunCanvas != null)
            {
                UpdateUiCanvas();
                _GunCanvas.EnebleLeftHand = true;
            }
            return _LeftlockPosition; 
        }
    }
    public Transform GetRightLockPosition
    {
        get 
        {
            if (_GunCanvas != null)
            {
                UpdateUiCanvas();
                _GunCanvas.EnebleRightHand = true;
            }
            return _RightlockPosition; 
        }
    }

    public void DisableCanvas()
    {
        if (_GunCanvas != null)
        {
            UpdateUiCanvas();
            _GunCanvas.EnebleLeftHand = false;
        }
    }

    public void ShootCalculate(float TriggerValeu)
    {
        if (TriggerValeu >= 0.8f && _ReadyToShoot == true && !_triggerIn && _CurrentAmmo > 0)
        {
            _triggerIn = true;
            _CurrentAmmo--;
            _ReadyToShoot = false;
            ShootBullet();
            UpdateUiCanvas();
        }
        else if (TriggerValeu <= 0.2f && _triggerIn)
        {
            _triggerIn = false;
        }
        else if (_GunType == GunType.automatic)
        {
            _triggerIn = false;
        }
    }

    private void ShootBullet()
    {
        RaycastHit Hit;
        Ray ray = new Ray(_shootpoint.position, _shootpoint.forward);
        if (Physics.Raycast(ray, out Hit, 10000000f))
        {
            GameObject G;
            MeshEditor Wall = Hit.collider.GetComponent<MeshEditor>();
            if (Wall != null)
            {
                G = Wall.RecalculateMesh(Hit.point, _shootpoint.rotation);

                Rigidbody RD = G.GetComponent<Rigidbody>();
                //Vector3 Diraction = _shootpoint.TransformDirection(Vector3.forward);

                if (RD == null)
                {
                    RD = G.AddComponent<Rigidbody>();
                    RD.useGravity = false;
                }
                //RD.AddForceAtPosition((_Inpact * _dammageMultiply) * Diraction, Hit.point);
                RD.AddForceAtPosition((_Inpact * _dammageMultiply) * ray.direction, Hit.point);

            }
        }
        _animator.SetInteger("GunState", 1);
    }

    public void ejectEmptyShel()
    {
        GameObject RD = Instantiate(_EmptyShell, _EjectPort.transform);
        RD.transform.localPosition = Vector3.zero;
        RD.transform.localRotation = _EmptyShell.transform.rotation;
        RD.transform.parent = null;
        Destroy(RD, 20);
        RD.GetComponent<Rigidbody>().velocity = _EjectPort.TransformDirection(_EjectDeraction * _EjectSpeed);
        _animator.SetInteger("GunState", 2);
    }

    public void ReadyToShootAgain()
    {
        _ReadyToShoot = true;
        _animator.SetInteger("GunState", 0);
    }


    private void UpdateUiCanvas()
    {
        if (_GunCanvas != null)
        {
            _GunCanvas.SetCanvasText = $"{_CurrentAmmo}/{_MaxAmmo}";
        }
    }

    internal int GetCurrentAmmo
    {
        get { return _CurrentAmmo; }
    }

    internal void EmptyMag()
    {
        _CurrentAmmo = 0;
        UpdateUiCanvas();
    }

    internal void SetAmmo(int rememberedAmmo)
    {
        _CurrentAmmo = rememberedAmmo;
        Mathf.Clamp(_CurrentAmmo, 0, _MaxAmmo);
        UpdateUiCanvas();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(_shootpoint.position, _shootpoint.forward);
    }


}
