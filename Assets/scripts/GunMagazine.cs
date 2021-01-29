using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Gun)), RequireComponent(typeof(Animator))]
public class GunMagazine : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Gun _gunscript;
    [SerializeField] private int _rememberedAmmo;
    [SerializeField] private GameObject _MagPrefab;
    [SerializeField] private Transform _magEjectPoint;
    int _state;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _gunscript = GetComponent<Gun>();
        _state = 0;
    }


    public void MagOut(int I)
    {
        if (_state == 0)
        {
            _state = 1;
            _rememberedAmmo = I;
            _animator.SetInteger("MagIn", _state);
        }
    }

    /// <summary>
    /// called before the mag is going inside the gun.
    /// </summary>
    /// <param name="I"></param>
    public bool MagIn(int I)
    {
        if (_state == 2)
        {
            _state = 3;
            _rememberedAmmo = I;
            _animator.SetInteger("MagIn", _state);
            return true;
        }

        return false;
    }


    bool HasMag = true;
    public void EjectEmptyMag()
    {
        if (HasMag)
        {
            HasMag = false;
            GameObject G = Instantiate(_MagPrefab, _magEjectPoint.position, _magEjectPoint.rotation);
            G.GetComponent<Magazine>().setAmmoInMag = _rememberedAmmo;
            G.GetComponent<Magazine>().SetMainGun = false;
            G.GetComponent<Magazine>().SetCommenData(this, this.gameObject);
            _gunscript.EmptyMag();
            _state = 2;
            _animator.SetInteger("MagIn", _state);
        }
    }

    public void NewMagIn()
    {
        if (!HasMag)
        {
            HasMag = true;
            _state = 0;
            _gunscript.SetAmmo(_rememberedAmmo);
            _animator.SetInteger("MagIn", _state);
        }
    }
}
