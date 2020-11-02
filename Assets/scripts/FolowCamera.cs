using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class FolowCamera : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _Midlle;
    [SerializeField] private Vector3 _rotetion;

    private void Start()
    {
        _rotetion = new Vector3();
    }

    public Vector3 Getforward
    {
        get { return transform.forward; }
    }
    public Vector3 GetRight
    {
        get { return transform.right; }
    }

    private void FixedUpdate()
    {
        if (_Midlle != null)
            transform.position = _Midlle.transform.localPosition;
        else
            transform.position = _camera.transform.position;
        _rotetion.Set(0, _camera.transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(_rotetion);
    }
}
