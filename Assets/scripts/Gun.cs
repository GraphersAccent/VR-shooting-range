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
    [SerializeField] private int _MaxAmmo; // the max ammo that the gun can carry
    [SerializeField] private int _CurrentAmmo; // the current ammo that is in the gun
    [Header("gun Data")]
    [SerializeField] private GunType _GunType; //what type of gun is it
    [SerializeField] private Transform _LeftlockPosition; // the position in the left hand
    [SerializeField] private Transform _RightlockPosition; // the position in the right hand
    [Header("shooting")]
    [SerializeField] private Transform _shootpoint; // the point where the raycast will be shot from
    [SerializeField] private LayerMask _HitableLayers; //the layers that the traceline can hit.
    [SerializeField] private bool _ReadyToShoot = true; //is it ready to shoot
    [SerializeField] private bool _triggerIn = false; // is the trigger pulled
    [Header("Animation")]
    [SerializeField] private Animator _animator; //the animator of this object
    [SerializeField] private Vector3 _EjectDeraction; //the deriction the bullet shells get ejected
    [SerializeField] private float _EjectSpeed; //the speed the bullet shells get ejected
    [SerializeField] private Transform _EjectPort; // the ejection port on the gun thet ejects the bullet shells
    [SerializeField] private GameObject _EmptyShell; // a prefab of the bullet shell 
    [Header("rigidbody")]
    [SerializeField] private float _Inpact = 10f; // the force that is inpacted on the hit object by this gun.
    [SerializeField] private float _dammageMultiply = 1f; // the force and damage multiplier.
    [Header("UI")]
    [SerializeField] private GunCanvasUpdate _GunCanvas; // the player 3D canvas that shows the ammount of bullets are left.

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

    /// <summary>
    /// calculate the shot bullet.
    /// </summary>
    private void ShootBullet()
    {
        RaycastHit Hit; // de raycast hit.
        Ray ray = new Ray(_shootpoint.position, _shootpoint.forward); // de gegevens voor de ray derection.
        if (Physics.Raycast(ray, out Hit, 10000000f, _HitableLayers)) // cast de raycast met de gegevens van van de ray.
        {
            GameObject G; // the object that has been shot.
            MeshEditor Wall = Hit.collider.GetComponent<MeshEditor>(); // get the meshEditor to edit the mesh to have a bullet hole.
            if (Wall != null) // if there is a MeshEditor do code in body of this if statement. 
            {
                G = Wall.RecalculateMesh(Hit.point, _shootpoint.rotation); // recalculate the mesh with a bullet hole.
                Rigidbody RD = G.GetComponent<Rigidbody>(); // get the rigidbody.
                if (RD == null) // if there isn't a rigidbody
                {
                    RD = G.AddComponent<Rigidbody>(); // add it
                    RD.useGravity = true; // make sure that there is gravity on the object.
                }
                RD.AddForceAtPosition((_Inpact * _dammageMultiply) * ray.direction, Hit.point); // add an inpact to the rigidbody.
            }
            TargetTrigger Targit = Hit.collider.GetComponent<TargetTrigger>(); // get TargetTrigger
            if (Targit != null) // if the target isn't null.
            {
                Targit.TargitIsHit(); // tell the TargetTrigger that it has been hit.
            }
        }
        _animator.SetInteger("GunState", 1); // tell the animator to set gunstate to 1.
    }

    /// <summary>
    /// eject the empty shel in the right direction.
    /// </summary>
    public void ejectEmptyShel()
    {
        if (_EjectPort != null && _EmptyShell != null)
        {
            GameObject RD = Instantiate(_EmptyShell, _EjectPort.transform);
            RD.transform.localPosition = Vector3.zero;
            RD.transform.localRotation = _EmptyShell.transform.rotation;
            RD.transform.parent = null;
            Destroy(RD, 20);
            RD.GetComponent<Rigidbody>().velocity = _EjectPort.TransformDirection(_EjectDeraction * _EjectSpeed);
        }
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
