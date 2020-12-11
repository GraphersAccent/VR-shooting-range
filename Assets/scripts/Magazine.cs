using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    [SerializeField] private bool _MainMag = true;
    [SerializeField] private GunMagazine _GunMag;
    [SerializeField] private GameObject _Gun;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private int _ammoInMag = 16;
    [SerializeField] private Vector3 _startPos;
    [SerializeField] private Vector3 _startRotation;
    [SerializeField] private Collider _collider;

    private void Start()
    {
        _startPos = transform.position;
        _startRotation = transform.rotation.eulerAngles;
        _collider = GetComponent<Collider>();
    }


    public void CheckIfInsideGun()
    {
        bool succes = false;
        
        Collider[] overlapcolliders = Physics.OverlapBox(_collider.bounds.center, (_collider.bounds.size * 1.2f) / 2, transform.rotation, _layer);
        for (int i = 0; i < overlapcolliders.Length; i++)
        {
            if (overlapcolliders[i].gameObject == _Gun)
            {
                if (_GunMag.MagIn(_ammoInMag))
                {
                    if (_MainMag)
                    {
                        transform.position = _startPos;
                        transform.rotation = Quaternion.Euler(_startRotation);
                    }
                    else
                    {
                        Destroy(this.gameObject);
                    }
                    succes = true;
                }
            }
        }

        if (!succes)
        {
            _startPos = transform.position;
            _startRotation = transform.rotation.eulerAngles;
        }
    }

    public int setAmmoInMag
    {
        set
        {
            _ammoInMag = value;
            Mathf.Clamp(_ammoInMag, 0, 1000);
            if (_ammoInMag == 0)
            {
                Destroy(this.gameObject, 5);
            }
        }
    }

    public void SetCommenData(GunMagazine GunMag, GameObject Gun)
    {
        _GunMag = GunMag;
        _Gun = Gun;
    }

    public bool SetMainGun
    {
        set { _MainMag = value; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size * 1.2f);
    }
}
