using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColiderCalculating : MonoBehaviour
{
    [SerializeField] private BoxCollider _Collider;
    [SerializeField] private Vector3 _size;
    [SerializeField] private Vector3 _senter;
    [SerializeField] private Transform _HighestPoint;
    [SerializeField] private Transform _LowestPoint;

    private void Awake()
    {
        _size = new Vector3();
        _senter = new Vector3();
    }

    private void FixedUpdate()
    {
        _senter.Set(0f, (_HighestPoint.position.y - _LowestPoint.position.y) / 4 * -1, 0f);
        _Collider.center = _senter;
        _size.Set(0.1f, _HighestPoint.position.y - _LowestPoint.position.y /2, 0.1f);
        _Collider.size = _size;
    }
}
