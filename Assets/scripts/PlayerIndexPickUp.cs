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
        _ectivationFloat = (_skeleton.pinkyCurl + _skeleton.ringCurl + _skeleton.middleCurl) / 3; // get the pinky-, ring- and middleCurl from the skeleton and divide by three.
        if (_ectivationFloat > 0.5f && _handClosed == false) // if the combined curl bigger is than 50% try picking up the object.
        {
            _handClosed = !_handClosed; // flip the bool _handClosed.
            CheckForObjectAndPickUp(); // try picking up the object.
        }
        else if(_ectivationFloat < 0.5f && _handClosed == true)// if the combined curl smaller is than 50% try lettin go of the object.
        {
            _handClosed = !_handClosed; // flip the bool _handClosed.
            if (_ObjectInHand != null) // if there is a object in the hand let it go.
                letGoOfObject();
        }

        if (_GunInHand != null) // if there is a gun in the hand.
        {
            _GunInHand.ShootCalculate(_trigger.GetAxis(_hand.inputSource)); // send the axis of the trigger to the gun.
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

    

    /// <summary>
    /// checks if there is a object in the hand.
    /// </summary>
    private void CheckForObjectAndPickUp()
    {
        if (_ObjectInHand != null) // if object in hand isn't null
        {
            letGoOfObject(); // let go of the current object
            if (_handClosed == true)
                _handClosed = !_handClosed; // flip bool _handClosed if necesery. 
        }

        Collider[] colliders = Physics.OverlapBox(_handCollider.transform.position, 
            _handCollider.transform.lossyScale / 2,
            _handCollider.transform.rotation, _Layer); // get all the colliders that overlap with this object and are in the right layer.

        for (int i = 0; i < colliders.Length; i++) //loop throug the array of colliders.
        {
            Rigidbody RB = colliders[i].attachedRigidbody; // get the rigidbody of the object in the array
            if (RB != null) // if it has a rigid body continue else look at the next entery in the array.
            {
                _otherHand.ObjectIsInOntherHand(colliders[i].gameObject); // check if the object is in the other hand. and tell them to let go.
                RB.isKinematic = true; // set the gun to kinematic
                _joint.connectedBody = RB; // connect the rigid body to the joint/
                _ObjectInHand = colliders[i].gameObject; // set the object in hand
                _ObjectInHand.transform.parent = gameObject.transform; // set the transform of the object.
                _ObjectRigidbody = null; // empty the reference to the objectrigidbody. this is used when letting go.
                _GunInHand = colliders[i].GetComponent<Gun>(); // get the Gun script.
                if (_GunInHand != null) // if the object is a Gun
                {
                    Transform LockPos; // get the lock position.
                    if (_RightHand)
                    {
                        LockPos = _GunInHand.GetRightLockPosition;
                    }
                    else
                    {
                        LockPos = _GunInHand.GetLeftLockPosition;
                    }
                    _handRendererObject.SetActive(false); // stop rendering the hand.
                    _ObjectInHand.transform.localPosition = LockPos.localPosition; // set the right position.
                    _ObjectInHand.transform.localRotation = LockPos.localRotation; // set the right rotation.
                }
                _magazineInHand = colliders[i].GetComponent<Magazine>(); // get magazine. is for letting go.
                break; // end the for loop.
                //i = colliders.Length; 
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
