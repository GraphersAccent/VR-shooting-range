using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GunCanvasUpdate : MonoBehaviour
{
    [SerializeField] private TMP_Text _LeftHanded;
    [SerializeField] private GameObject _leftPanel;
    [SerializeField] private TMP_Text _RightHanded;
    [SerializeField] private GameObject _RightPanel;

    private void Awake()
    {
        _LeftHanded.text = "0/0";
        _RightHanded.text = "0/0";

        _leftPanel.SetActive(false);
        _RightPanel.SetActive(false);
    }

    public string SetCanvasText
    {
        set
        {
            _LeftHanded.text = value;
            _RightHanded.text = value;
        }
    }

    public bool EnebleLeftHand
    {
        set
        {
            _leftPanel.SetActive(value);
            _RightPanel.SetActive(false);
        }
    }

    public bool EnebleRightHand
    {
        set
        {
            _leftPanel.SetActive(false);
            _RightPanel.SetActive(value);
        }
    }

}
