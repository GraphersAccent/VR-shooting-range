using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class HandInspector : MonoBehaviour
{
    [SerializeField] private SteamVR_Action_Skeleton _skeleton;
    [SerializeField] private Text _thumb;
    [SerializeField] private Text _Index;
    [SerializeField] private Text _Middle;
    [SerializeField] private Text _Ring;
    [SerializeField] private Text _Pinkie;

    private void FixedUpdate()
    {
        _thumb.text = _skeleton.thumbCurl.ToString("N3");
        _Index.text = _skeleton.indexCurl.ToString("N3");
        _Middle.text = _skeleton.middleCurl.ToString("N3");
        _Ring.text = _skeleton.ringCurl.ToString("N3");
        _Pinkie.text = _skeleton.pinkyCurl.ToString("N3");
    }
}
