using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShootChallenge : MonoBehaviour
{
    private enum RangeState
    {
        NotActive = 0,
        MenuStage,
        active,
    }

    [SerializeField] private Transform _Trigger;
    [SerializeField] private LayerMask _layer;
    [SerializeField] private GameObject[] _options1;
    [SerializeField] private GameObject[] _options2;
    [SerializeField] private GameObject[] _options3;
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private int _difecutySelectet;
    [Header("Target")]
    [SerializeField] private GameObject _CurrentTarget;
    [SerializeField] private RangeState _state = RangeState.NotActive;
    [SerializeField] private TargitResponse _GameType;
    [SerializeField] private TargitResponse _GameDistace;
    [SerializeField] private TargitResponse _GameDifaculty;
    [Header("spawnpoints")]
    [SerializeField] private Transform _FiveMeter;
    [SerializeField] private Transform _TenMeter;
    [SerializeField] private Transform _fifteenMeter;
    [SerializeField] private Transform _twentyMeter;
    [Header("Game")]
    [SerializeField] private float _timer;
    [SerializeField] private int _targetsHit;
    [Header("Time race"), Tooltip("hit as much targets as posable")]
    [SerializeField] private int[] _targetsNeeded;
    [Header("Time trail"), Tooltip("hit an amount between the time")]
    [SerializeField] private float[] _TimeraceStartingTime;
    [Header("UI")]
    [SerializeField] private TMP_Text[] _UIGameName;
    [SerializeField] private TMP_Text[] _UIDistance;
    [SerializeField] private TMP_Text[] _UIDifaculty;
    [SerializeField] private TMP_Text[] _UITimer;
    [SerializeField] private TMP_Text[] _UITargetsHit;
    [Header("ScoreBoard")]
    [SerializeField] private ScoreboardMenager _ScoreboardManeger;

    private void Awake()
    {
        EmptyUI();
    }
    private void FixedUpdate()
    {
        Collider[] triggers = Physics.OverlapBox(_Trigger.position, _Trigger.lossyScale / 2, _Trigger.rotation, _layer);
        if (triggers.Length > 0 && _state == RangeState.NotActive)
        {
            _state = RangeState.MenuStage;
            setMenuState(true, false, false);
        }
        else if (triggers.Length <= 0 && _state != RangeState.NotActive)
        {
            _state = RangeState.NotActive;
            setMenuState(false, false, false);
            if (_CurrentTarget != null)
            {
                Destroy(_CurrentTarget);
            }
        }
        else if (_GameType == TargitResponse.Stopwatch && _state == RangeState.active)
        {
            _timer -= Time.fixedDeltaTime;
            if (_timer <= 0)
            {
                GameCompleted();
            }
        }
        else if (_GameType == TargitResponse.Timetrail && _state == RangeState.active)
        {
            _timer += Time.fixedDeltaTime;
        }


        //UI
        if (_state == RangeState.active)
        {
            int min = Mathf.RoundToInt(_timer) / 60; 
            int sec = Mathf.RoundToInt(_timer) % 60;
            
            for (int i = 0; i < _UITimer.Length; i++)
            {
                _UITimer[i].text = $"{min}:{sec}";
            }
        }
    }

    public void ResponseIn(TargitType Type, TargitResponse Response)
    {
        switch (Type)
        {
            case TargitType.normal:
                NormalResponse();
                break;
            case TargitType.menu:
                MenuResponse(Response);
                break;
            default:
                break;
        }
    }

    private void EmptyUI(bool emptyGameName = true)
    {
        if (emptyGameName)
        {
            for (int i = 0; i < _UIGameName.Length; i++)
            {
                _UIGameName[i].text = string.Empty;
            }
        }
        for (int i = 0; i < _UIDistance.Length; i++)
        {
            _UIDistance[i].text = string.Empty;
        }
        for (int i = 0; i < _UIDifaculty.Length; i++)
        {
            _UIDifaculty[i].text = string.Empty;
        }
        for (int i = 0; i < _UITimer.Length; i++)
        {
            _UITimer[i].text = string.Empty;
        }
        for (int i = 0; i < _UITargetsHit.Length; i++)
        {
            _UITargetsHit[i].text = string.Empty;
        }
    }

    private void SpawnHitObject()
    {
        GameObject G = Instantiate(_targetPrefab);

        switch (_GameDistace)
        {
            case TargitResponse.M5:
                G.transform.parent = _FiveMeter;
                if (_FiveMeter.transform.childCount > 10)
                {
                    Destroy(_FiveMeter.transform.GetChild(0).gameObject);
                }
                break;
            case TargitResponse.M10:
                G.transform.parent = _TenMeter;
                if (_TenMeter.transform.childCount > 10)
                {
                    Destroy(_TenMeter.transform.GetChild(0).gameObject);
                }
                break;
            case TargitResponse.M15:
                G.transform.parent = _fifteenMeter;
                if (_fifteenMeter.transform.childCount > 10)
                {
                    Destroy(_fifteenMeter.transform.GetChild(0).gameObject);
                }
                break;
            case TargitResponse.M20:
                G.transform.parent = _twentyMeter;
                if (_twentyMeter.transform.childCount > 10)
                {
                    Destroy(_twentyMeter.transform.GetChild(0).gameObject);
                }
                break;
        }

        Vector3 SpawnPos = UnityEngine.Random.insideUnitSphere * 2.5f;
        if (SpawnPos.y < 0)
            SpawnPos.y = 0;
        G.transform.localPosition = SpawnPos;

        TargetTrigger trigger = G.GetComponent<TargetTrigger>();
        if (trigger != null)
        {
            trigger.SetChallangePlatform = this;
        }

        _CurrentTarget = G;
    }

    private void MenuResponse(TargitResponse Response)
    {
        switch (Response)
        {
            case TargitResponse.Timetrail:
                _GameType = TargitResponse.Timetrail;
                EmptyUI(false);
                setMenuState(false, true, false);
                //UI
                string GameType = Enum.GetName(typeof(TargitResponse), _GameType);
                for (int i = 0; i < _UIGameName.Length; i++)
                {
                    _UIGameName[i].text = GameType;
                }
                break;
            case TargitResponse.Stopwatch:
                _GameType = TargitResponse.Stopwatch;
                EmptyUI(false);
                setMenuState(false, true, false);
                //UI
                GameType = Enum.GetName(typeof(TargitResponse), _GameType);
                for (int i = 0; i < _UIGameName.Length; i++)
                {
                    _UIGameName[i].text = GameType;
                }
                break;
            case TargitResponse.M5:
                setMenuState(false, false, true);
                _GameDistace = Response;
                //UI
                string GameDistance = Enum.GetName(typeof(TargitResponse), _GameDistace);
                for (int i = 0; i < _UIDistance.Length; i++)
                {
                    _UIDistance[i].text = GameDistance;
                }
                break;
            case TargitResponse.M10:
                setMenuState(false, false, true);
                _GameDistace = Response;
                //UI
                GameDistance = Enum.GetName(typeof(TargitResponse), _GameDistace);
                for (int i = 0; i < _UIDistance.Length; i++)
                {
                    _UIDistance[i].text = GameDistance;
                }
                break;
            case TargitResponse.M15:
                setMenuState(false, false, true);
                _GameDistace = Response;
                //UI
                GameDistance = Enum.GetName(typeof(TargitResponse), _GameDistace);
                for (int i = 0; i < _UIDistance.Length; i++)
                {
                    _UIDistance[i].text = GameDistance;
                }
                break;
            case TargitResponse.M20:
                setMenuState(false, false, true);
                _GameDistace = Response;
                //UI
                GameDistance = Enum.GetName(typeof(TargitResponse), _GameDistace);
                for (int i = 0; i < _UIDistance.Length; i++)
                {
                    _UIDistance[i].text = GameDistance;
                }
                break;
            case TargitResponse.easy:
                setMenuState(false, false, false);
                _state = RangeState.active;
                _GameDifaculty = Response;
                _difecutySelectet = 0;
                gameSetup();
                //UI
                string Difaculty = Enum.GetName(typeof(TargitResponse), _GameDifaculty);
                for (int i = 0; i < _UIDifaculty.Length; i++)
                {
                    _UIDifaculty[i].text = Difaculty;
                }
                break;
            case TargitResponse.normal:
                setMenuState(false, false, false);
                _state = RangeState.active;
                _GameDifaculty = Response;
                _difecutySelectet = 1;
                gameSetup();
                //UI
                Difaculty = Enum.GetName(typeof(TargitResponse), _GameDifaculty);
                for (int i = 0; i < _UIDifaculty.Length; i++)
                {
                    _UIDifaculty[i].text = Difaculty;
                }
                break;
            case TargitResponse.hard:
                setMenuState(false, false, false);
                _state = RangeState.active;
                _GameDifaculty = Response;
                _difecutySelectet = 2;
                gameSetup();
                //UI
                Difaculty = Enum.GetName(typeof(TargitResponse), _GameDifaculty);
                for (int i = 0; i < _UIDifaculty.Length; i++)
                {
                    _UIDifaculty[i].text = Difaculty;
                }
                break;
            case TargitResponse.nightmare:
                setMenuState(false, false, false);
                _state = RangeState.active;
                _GameDifaculty = Response;
                _difecutySelectet = 3;
                gameSetup();
                //UI
                Difaculty = Enum.GetName(typeof(TargitResponse), _GameDifaculty);
                for (int i = 0; i < _UIDifaculty.Length; i++)
                {
                    _UIDifaculty[i].text = Difaculty;
                }
                break;
            default:
                break;
        }
    }

    private void setMenuState(bool firstState, bool secondState, bool thirdState)
    {
        for (int i = 0; i < _options1.Length; i++)
        {
            _options1[i].SetActive(firstState);
        }
        for (int i = 0; i < _options2.Length; i++)
        {
            _options2[i].SetActive(secondState);
        }
        for (int i = 0; i < _options3.Length; i++)
        {
            _options3[i].SetActive(thirdState);
        }
    }

    private void gameSetup()
    {
        _targetsHit = 0;
        if (_GameType == TargitResponse.Stopwatch)
        {
            _timer = _TimeraceStartingTime[_difecutySelectet];
        }
        else if (_GameType == TargitResponse.Timetrail)
        {
            _timer = 0;
        }
        SpawnHitObject();

        string HitText = "";
        switch (_GameType)
        {
            case TargitResponse.Timetrail:
                HitText = $"{_targetsHit} / {_targetsNeeded[_difecutySelectet]}";
                break;
            case TargitResponse.Stopwatch:
                HitText = $"{_targetsHit} Targets hit";
                break;
            default:
                break;
        }

        //UI
        for (int i = 0; i < _UITargetsHit.Length; i++)
        {

            _UITargetsHit[i].text = HitText;
        }
    }

    private void GameCompleted()
    {
        _state = RangeState.NotActive;
        setMenuState(false, false, false);
        if (_CurrentTarget != null)
        {
            Destroy(_CurrentTarget);
        }
        switch (_GameType)
        {
            case TargitResponse.Timetrail:
                _ScoreboardManeger.NewScoreInput(_GameDistace, _GameDifaculty, _GameType, _timer);
                break;
            case TargitResponse.Stopwatch:
                _ScoreboardManeger.NewScoreInput(_GameDistace, _GameDifaculty, _GameType, _targetsHit);
                break;
        }
        
    }

    private void NormalResponse()
    {
        _targetsHit++;
        
        if (_GameType == TargitResponse.Timetrail && _targetsHit >= _targetsNeeded[_difecutySelectet])// hit an amount between the time
        {
            GameCompleted();
        }
        else
        {
            SpawnHitObject();
        }

        if (_GameType == TargitResponse.Stopwatch)
        {
            _timer += (_TimeraceStartingTime[_difecutySelectet] * 0.1f);
        }

        string HitText = "";
        switch (_GameType)
        {
            case TargitResponse.Timetrail:
                HitText = $"{_targetsHit} / {_targetsNeeded[_difecutySelectet]}";
                break;
            case TargitResponse.Stopwatch:
                HitText = $"{_targetsHit} Targets hit";
                break;
            default:
                break;
        }

        //UI
        for (int i = 0; i < _UITargetsHit.Length; i++)
        {

            _UITargetsHit[i].text = HitText;
        }
    }


}
