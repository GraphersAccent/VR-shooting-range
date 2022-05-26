using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class RigMovement : MonoBehaviour
{
    [SerializeField] private FolowCamera _Head; // the head camera
    [SerializeField] private SteamVR_Action_Vector2 _Stick; // what input is needed
    [SerializeField] private SteamVR_Behaviour_Skeleton _LeftSkeleton; // the skeleton of the left hand.
    [SerializeField] private SteamVR_Behaviour_Skeleton _RightSkeleton; // the skeleton of the right hand.
    [SerializeField] private float _Speed = 100; // speed of movement
    [SerializeField] private Vector2 _ThumbStick; // the posision of the thumbstick.
    [SerializeField] private Rigidbody _RigidBody; // the rigidbody that will be applied


    private void FixedUpdate()
    {
        _ThumbStick = _Stick.GetAxis(_LeftSkeleton.inputSource) + _Stick.GetAxis(_RightSkeleton.inputSource); // get the axis of the left and right controller.
        if (_ThumbStick.x > 0.2f || _ThumbStick.x < -0.2f || _ThumbStick.y > 0.2f || _ThumbStick.y < -0.2f) // check that the axis are outside there "deadzone".
        {
            transform.position = Vector3.MoveTowards(transform.position, // current position
                transform.position + _Head.Getforward * _ThumbStick.y + _Head.GetRight * _ThumbStick.x, // calculate new derection
                _Speed * Time.fixedDeltaTime); // speed of the movement
        }
        else // else make sure that the rigid body doesn't move.
        {
            _RigidBody.velocity = Vector3.zero;
        }
    }
}
