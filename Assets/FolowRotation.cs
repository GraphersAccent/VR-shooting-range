using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolowRotation : MonoBehaviour
{
    [SerializeField] private Transform _FollowThis;

    private void FixedUpdate()
    {
        transform.rotation = _FollowThis.rotation;
    }
}
