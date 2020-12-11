using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TargitType
{
    normal = 0,
    menu,
}

public enum TargitResponse
{
    Timetrail = 0,
    Stopwatch,
    M5,
    M10,
    M15,
    M20,
    easy,
    normal,
    hard,
    nightmare,
}

public class TargetTrigger : MonoBehaviour
{
    [SerializeField] private ShootChallenge _challangePlatform;
    [SerializeField] private TargitType _type;
    [SerializeField] private TargitResponse _response;

    public ShootChallenge SetChallangePlatform
    {
        set { _challangePlatform = value; }
    }

    public void TargitIsHit() => _challangePlatform.ResponseIn(_type, _response);
}
