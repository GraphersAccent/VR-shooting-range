using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Valve.VR;

public class PlayerIndexPickUp : MonoBehaviour
{
    [SerializeField] private SteamVR_Behaviour_Pose _hand;
    [SerializeField] private GameObject _handRendererObject;
    [SerializeField] private SteamVR_Action_Single _trigger;
    [SerializeField] private SteamVR_Action_Boolean _AButton;
    [SerializeField] private PlayerIndexPickUp _otherHand;
    [Header("Activation Box")]
    [SerializeField] private GameObject _handCollider;
    [SerializeField] private Mesh _visialMesh;
    [SerializeField] private Vector3 _Position;
    [SerializeField] private Vector3 _Size;
    [SerializeField] private Vector3 _Rotation;
    [Header("skeleton")]
    [SerializeField] private SteamVR_Action_Skeleton _skeleton;
    [Header("ectivation")]
    [SerializeField] private bool _RightHand;
    [SerializeField] private LayerMask _Layer;
    [SerializeField] private Rigidbody _ObjectRigidbody;
    [SerializeField] private GameObject _ObjectInHand;
    [SerializeField] private FixedJoint _joint;
    [SerializeField] private float _ectivationFloat;
    [SerializeField] private bool _handClosed = false;
    [SerializeField] private Gun _GunInHand;
    [SerializeField] private Magazine _magazineInHand;

    private void Update()
    {
        if (_GunInHand != null)
        {
            if (_AButton.GetLastStateDown(_hand.inputSource))
            {
                GunMagazine GunMag = _GunInHand.gameObject.GetComponent<GunMagazine>();
                if (GunMag != null)
                {
                    GunMag.MagOut(_GunInHand.GetCurrentAmmo);
                }
            }
        }
    }


    private void FixedUpdate()
    {
        _ectivationFloat = (_skeleton.pinkyCurl + _skeleton.ringCurl + _skeleton.middleCurl) / 3;
        if (_ectivationFloat > 0.5f && _handClosed == false)
        {
            _handClosed = !_handClosed;
            CheckForObjectAndPickUp();
        }
        else if(_ectivationFloat < 0.5f && _handClosed == true)
        {
            _handClosed = !_handClosed;
            if (_ObjectInHand != null)
                letGoOfObject();
        }

        if (_GunInHand != null)
        {
            _GunInHand.ShootCalculate(_trigger.GetAxis(_hand.inputSource));

        }
    }

    

    /// <summary>
    /// returns False if the object isn't held by the hand.
    /// </summary>
    /// <param name="Specement"></param>
    /// <returns></returns>
    public void ObjectIsInOntherHand(GameObject Specement)
    {
        if (Specement == _ObjectInHand)
        {
            letGoOfObject();
        }
    }

    


    private void CheckForObjectAndPickUp()
    {
        if (_ObjectInHand != null)
        {
            letGoOfObject();
            if (_handClosed == true)
                _handClosed = !_handClosed;
        }

        Collider[] colliders = Physics.OverlapBox(_handCollider.transform.position, _handCollider.transform.lossyScale / 2, _handCollider.transform.rotation, _Layer);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody RB = colliders[i].attachedRigidbody;
            if (RB != null)
            {
                _otherHand.ObjectIsInOntherHand(colliders[i].gameObject);
                RB.isKinematic = true;
                _joint.connectedBody = RB;
                _ObjectInHand = colliders[i].gameObject;
                _ObjectInHand.transform.parent = gameObject.transform;
                _ObjectRigidbody = null;
                // check if object is Gun;
                _GunInHand = colliders[i].GetComponent<Gun>();
                if (_GunInHand != null)
                {
                    Transform LockPos;
                    if (_RightHand)
                    {
                        LockPos = _GunInHand.GetRightLockPosition;
                    }
                    else
                    {
                        LockPos = _GunInHand.GetLeftLockPosition;
                    }
                    _handRendererObject.SetActive(false);
                    _ObjectInHand.transform.localPosition = LockPos.localPosition;
                    _ObjectInHand.transform.localRotation = LockPos.localRotation;
                }

                _magazineInHand = colliders[i].GetComponent<Magazine>();
                i = colliders.Length; // end the for loop.
            }
        }
    }

    private void letGoOfObject()
    {
        if (_magazineInHand != null)
        {
            _magazineInHand.CheckIfInsideGun();
            _magazineInHand = null;
        }


        _ObjectRigidbody = _joint.connectedBody;
        _joint.connectedBody = null;
        _ObjectInHand.transform.parent = null;
        _ObjectInHand = null;
        if (_GunInHand != null)
        {
            _GunInHand.DisableCanvas();
            _GunInHand = null;
        }
        _handRendererObject.SetActive(true);
        if (_ObjectRigidbody != null)
        {
            _ObjectRigidbody.isKinematic = false;
            _ObjectRigidbody.velocity = _hand.GetVelocity();
            _ObjectRigidbody.angularVelocity = _hand.GetAngularVelocity();
        }
    }
}
