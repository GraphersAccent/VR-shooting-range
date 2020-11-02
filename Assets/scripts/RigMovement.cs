using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RigMovement : MonoBehaviour
{
    [SerializeField] private FolowCamera _Head;
    [SerializeField] private SteamVR_Action_Vector2 _Stick;
    [SerializeField] private SteamVR_Behaviour_Skeleton _LeftSkeleton;
    [SerializeField] private SteamVR_Behaviour_Skeleton _RightSkeleton;
    [SerializeField] private float _Speed = 100;
    [SerializeField] private Vector2 _ThumbStick;
    [SerializeField] private Rigidbody _RigidBody;


    private void FixedUpdate()
    {
        _ThumbStick = _Stick.GetAxis(_LeftSkeleton.inputSource);
        if (_ThumbStick.x > 0.2f || _ThumbStick.x < -0.2f || _ThumbStick.y > 0.2f || _ThumbStick.y < -0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + _Head.Getforward * _ThumbStick.y + _Head.GetRight * _ThumbStick.x,_Speed * Time.fixedDeltaTime);
        }
        else
        {
            _RigidBody.velocity = Vector3.zero;
        }
        
    }
}
