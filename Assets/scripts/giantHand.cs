using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class giantHand : MonoBehaviour
{


    [SerializeField] private SteamVR_Action_Skeleton _skeleton;
    [Header("Thumb finger")]
    [SerializeField] private float _ThumbMaxRotation = 80f;
    [SerializeField] private GameObject[] _ThumbFingerJoinds;
    [Header("Index finger")]
    [SerializeField] private float _IndexMaxRotation = 80f;
    [SerializeField] private GameObject[] _IndexFingerJoinds;
    [Header("Middle finger")]
    [SerializeField] private float _MiddleMaxRotation = 80f;
    [SerializeField] private GameObject[] _MiddleFingerJoinds;
    [Header("Ring finger")]
    [SerializeField] private float _RingMaxRotation = 80f;
    [SerializeField] private GameObject[] _RingFingerJoinds;
    [Header("Pinky finger")]
    [SerializeField] private float _PinkyMaxRotation = 80f;
    [SerializeField] private GameObject[] _PinkyFingerJoinds;
    [Header("position & Rotation")]
    [SerializeField] private GameObject _followingObject;



    private void FixedUpdate()
    {
        #region fingers
        for (int i = 0; i < _ThumbFingerJoinds.Length; i++)
        {
            Vector3 V = _ThumbFingerJoinds[i].transform.localRotation.eulerAngles;
            V.x = _ThumbMaxRotation * _skeleton.thumbCurl;
            _ThumbFingerJoinds[i].transform.localRotation = Quaternion.Euler(V);
        }

        for (int i = 0; i < _IndexFingerJoinds.Length; i++)
        {
            Vector3 V = _IndexFingerJoinds[i].transform.localRotation.eulerAngles;
            V.x = _IndexMaxRotation * _skeleton.indexCurl;
            _IndexFingerJoinds[i].transform.localRotation = Quaternion.Euler(V);
        }

        for (int i = 0; i < _MiddleFingerJoinds.Length; i++)
        {
            Vector3 V = _MiddleFingerJoinds[i].transform.localRotation.eulerAngles;
            V.x = _MiddleMaxRotation * _skeleton.middleCurl;
            _MiddleFingerJoinds[i].transform.localRotation = Quaternion.Euler(V);
        }

        for (int i = 0; i < _RingFingerJoinds.Length; i++)
        {
            Vector3 V = _RingFingerJoinds[i].transform.localRotation.eulerAngles;
            V.x = _RingMaxRotation * _skeleton.ringCurl;
            _RingFingerJoinds[i].transform.localRotation = Quaternion.Euler(V);
        }

        for (int i = 0; i < _PinkyFingerJoinds.Length; i++)
        {
            Vector3 V = _PinkyFingerJoinds[i].transform.localRotation.eulerAngles;
            V.x = _PinkyMaxRotation * _skeleton.pinkyCurl;
            _PinkyFingerJoinds[i].transform.localRotation = Quaternion.Euler(V);
        }
        #endregion fingers

        transform.localPosition = _followingObject.transform.localPosition;
        transform.localRotation = _followingObject.transform.localRotation;

    }
}
