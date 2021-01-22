using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using System.IO;

public class ScoreBoard : MonoBehaviour
{
    [Header("Save")]
    [SerializeField] private string _name;
    [Header("Time Trail")]
    [SerializeField] private TMP_Text _TimeTrailEasyText;
    [SerializeField] private List<float> _TimeTrailEasyList;
    [SerializeField] private TMP_Text _TimeTrailNormalText;
    [SerializeField] private List<float> _TimeTrailNormalList;
    [SerializeField] private TMP_Text _TimeTrailHardText;
    [SerializeField] private List<float> _TimeTrailHardList;
    [SerializeField] private TMP_Text _TimeTrailNightmareText;
    [SerializeField] private List<float> _TimeTrailNightmareList;
    [Header("Stopwatch")]
    [SerializeField] private TMP_Text _StopwatchEasyText;
    [SerializeField] private List<float> _StopwatchEasyList;
    [SerializeField] private TMP_Text _StopwatchNormalText;
    [SerializeField] private List<float> _StopwatchNormalList;
    [SerializeField] private TMP_Text _StopwatchHardText;
    [SerializeField] private List<float> _StopwatchHardList;
    [SerializeField] private TMP_Text _StopwatchNightmareText;
    [SerializeField] private List<float> _StopwatchNightmareList;

    private void Awake()
    {
        if (File.Exists(jsonSavePath))
        {
            loadData();
        }
        else
        {
            _TimeTrailEasyList = new List<float>();
            _TimeTrailNormalList = new List<float>();
            _TimeTrailHardList = new List<float>();
            _TimeTrailNightmareList = new List<float>();
            _StopwatchEasyList = new List<float>();
            _StopwatchNormalList = new List<float>();
            _StopwatchHardList = new List<float>();
            _StopwatchNightmareList = new List<float>();
        }
    }

    public void NewScoreInput(TargitResponse Difeculty, TargitResponse GameType, float score)
    {
        switch (GameType)
        {
            case TargitResponse.Timetrail:
                TimeTrailInput(Difeculty, score);
                break;
            case TargitResponse.Stopwatch:
                StopwatchInput(Difeculty, score);
                break;
        }
    }

    public void TimeTrailInput(TargitResponse Difeculty, float score)
    {
        List<float> temporal;
        switch (Difeculty)
        {
            case TargitResponse.easy:
                temporal = _TimeTrailEasyList;
                break;
            case TargitResponse.normal:
                temporal = _TimeTrailNormalList;
                break;
            case TargitResponse.hard:
                temporal = _TimeTrailHardList;
                break;
            case TargitResponse.nightmare:
                temporal = _TimeTrailNightmareList;
                break;
            default:
                temporal = new List<float>();
                break;
        }

        if (!temporal.Contains(score))
        {
            temporal.Add(score);
        }

        temporal = temporal.OrderBy(number => number).ToList();

        for (int i = 5; i < temporal.Count; i++)
        {
            temporal.Remove(temporal[i]);
        }

        string NewText = "";
        int Rank = 1;
        TimeSpan time;
        foreach (float item in temporal)
        {
            time = TimeSpan.FromSeconds(item);
            if (item == score)
            {
                NewText += $"<color=red>{Rank}. {time.ToString(@"mm\:ss\:fff")}</color>\n";
            }
            else
            {
                NewText += $"{Rank}. {time.ToString(@"mm\:ss\:fff")}\n";
            }
            Rank++;
        }

        switch (Difeculty)
        {
            case TargitResponse.easy:
                _TimeTrailEasyList = temporal;
                _TimeTrailEasyText.text = NewText;
                break;
            case TargitResponse.normal:
                _TimeTrailNormalList = temporal;
                _TimeTrailNormalText.text = NewText;
                break;
            case TargitResponse.hard:
                _TimeTrailHardList = temporal;
                _TimeTrailHardText.text = NewText;
                break;
            case TargitResponse.nightmare:
                _TimeTrailNightmareList = temporal;
                _TimeTrailNightmareText.text = NewText;
                break;
        }
    }

    public void StopwatchInput(TargitResponse Difeculty, float score)
    {
        List<float> temporal;
        switch (Difeculty)
        {
            case TargitResponse.easy:
                temporal = _StopwatchEasyList;
                break; 
            case TargitResponse.normal:
                temporal = _StopwatchNormalList;
                break;
            case TargitResponse.hard:
                temporal = _StopwatchHardList;
                break;
            case TargitResponse.nightmare:
                temporal = _StopwatchNightmareList;
                break;
            default:
                temporal = new List<float>();
                break;
        }

        if (!temporal.Contains(score))
        {
            temporal.Add(score);
        }

        temporal = temporal.OrderBy(number => number).Reverse().ToList();

        for (int i = 5; i < temporal.Count; i++)
        {
            temporal.Remove(temporal[i]);
        }

        string NewText = "";
        int Rank = 1;
        foreach (float item in temporal)
        {
            if (item == score)
            {
                NewText += $"<color=red>{Rank}. {item}</color>\n";
            }
            else
            {
                NewText += $"{Rank}. {item}\n";
            }
            Rank++;
        }

        switch (Difeculty)
        {
            case TargitResponse.easy:
                _StopwatchEasyList = temporal;
                _StopwatchEasyText.text = NewText;
                break;
            case TargitResponse.normal:
                _StopwatchNormalList = temporal;
                _StopwatchNormalText.text = NewText;
                break;
            case TargitResponse.hard:
                _StopwatchHardList = temporal;
                _StopwatchHardText.text = NewText;
                break;
            case TargitResponse.nightmare:
                _StopwatchNightmareList = temporal;
                _StopwatchNightmareText.text = NewText;
                break;
        }
    }

    private void SaveData()
    {
        // timetrail
        DefecultySaveData EasyTimeTraile = new DefecultySaveData
        {
            Scores = _TimeTrailEasyList.ToArray(),
            Text = _TimeTrailEasyText.text,
        };
        DefecultySaveData NormalTimeTraile = new DefecultySaveData
        {
            Scores = _TimeTrailNormalList.ToArray(),
            Text = _TimeTrailNormalText.text,
        };
        DefecultySaveData HardTimeTraile = new DefecultySaveData
        {
            Scores = _TimeTrailHardList.ToArray(),
            Text = _TimeTrailHardText.text,
        };
        DefecultySaveData NightmareTimeTraile = new DefecultySaveData
        {
            Scores = _TimeTrailNightmareList.ToArray(),
            Text = _TimeTrailNightmareText.text,
        };
        // stopwatch
        DefecultySaveData EasyStopwatch = new DefecultySaveData
        {
            Scores = _StopwatchEasyList.ToArray(),
            Text = _StopwatchEasyText.text,
        };
        DefecultySaveData NormalStopwatch = new DefecultySaveData
        {
            Scores = _StopwatchNormalList.ToArray(),
            Text = _StopwatchNormalText.text,
        };
        DefecultySaveData HardStopwatch = new DefecultySaveData
        {
            Scores = _StopwatchHardList.ToArray(),
            Text = _StopwatchHardText.text,
        };
        DefecultySaveData NightmareStopwatch = new DefecultySaveData
        {
            Scores = _StopwatchNightmareList.ToArray(),
            Text = _StopwatchNightmareText.text,
        };
        // all together.
        ScoreBoardSaveData ScoreBoard = new ScoreBoardSaveData
        {
            EasyTimeTraile = EasyTimeTraile,
            NormalTimeTraile = NormalTimeTraile,
            HardTimeTraile = HardTimeTraile,
            NightmareTimeTraile = NightmareTimeTraile,
            EasyStopwatch = EasyStopwatch,
            NormalStopwatch = NormalStopwatch,
            HardStopwatch = HardStopwatch,
            NightmareStopwatch = NightmareStopwatch,

            //EasyTimeTraile = getBytes(EasyTimeTraile),
            //NormalTimeTraile = getBytes(NormalTimeTraile),
            //HardTimeTraile = getBytes(HardTimeTraile),
            //NightmareTimeTraile = getBytes(NightmareTimeTraile),
            //EasyStopwatch = getBytes(EasyStopwatch),
            //NormalStopwatch = getBytes(NormalStopwatch),
            //HardStopwatch = getBytes(HardStopwatch),
            //NightmareStopwatch = getBytes(NightmareStopwatch),
        };

        string JsonData = JsonUtility.ToJson(ScoreBoard, false);
        File.WriteAllText(jsonSavePath, JsonData);

    }

    private void loadData()
    {
        string JsonData = File.ReadAllText(jsonSavePath);
        ScoreBoardSaveData ScoreBoard = (ScoreBoardSaveData)JsonUtility.FromJson(JsonData, typeof(ScoreBoardSaveData));

        // TimeTrail
        DefecultySaveData DefragmentedData = ScoreBoard.EasyTimeTraile;
        //DefecultySaveData DefragmentedData = fromBytes(ScoreBoard.EasyTimeTraile);
        _TimeTrailEasyList = DefragmentedData.Scores.ToList();
        _TimeTrailEasyText.text = DefragmentedData.Text;

        DefragmentedData = ScoreBoard.NormalTimeTraile;
        //DefragmentedData = fromBytes(ScoreBoard.NormalTimeTraile);
        _TimeTrailNormalList = DefragmentedData.Scores.ToList();
        _TimeTrailNormalText.text = DefragmentedData.Text;

        DefragmentedData = ScoreBoard.HardTimeTraile;
        //DefragmentedData = fromBytes(ScoreBoard.HardTimeTraile);
        _TimeTrailHardList = DefragmentedData.Scores.ToList();
        _TimeTrailHardText.text = DefragmentedData.Text;

        DefragmentedData = ScoreBoard.NightmareTimeTraile;
        //DefragmentedData = fromBytes(ScoreBoard.NightmareTimeTraile);
        _TimeTrailNightmareList = DefragmentedData.Scores.ToList();
        _TimeTrailNightmareText.text = DefragmentedData.Text;

        //stopwatch
        DefragmentedData = ScoreBoard.EasyStopwatch;
        //DefragmentedData = fromBytes(ScoreBoard.EasyStopwatch);
        _StopwatchEasyList = DefragmentedData.Scores.ToList();
        _StopwatchEasyText.text = DefragmentedData.Text;

        DefragmentedData = ScoreBoard.NormalStopwatch;
        //DefragmentedData = fromBytes(ScoreBoard.NormalStopwatch);
        _StopwatchNormalList = DefragmentedData.Scores.ToList();
        _StopwatchNormalText.text = DefragmentedData.Text;

        DefragmentedData = ScoreBoard.HardStopwatch;
        //DefragmentedData = fromBytes(ScoreBoard.HardStopwatch);
        _StopwatchHardList = DefragmentedData.Scores.ToList();
        _StopwatchHardText.text = DefragmentedData.Text;

        DefragmentedData = ScoreBoard.NightmareStopwatch;
        //DefragmentedData = fromBytes(ScoreBoard.NightmareStopwatch);
        _StopwatchNightmareList = DefragmentedData.Scores.ToList();
        _StopwatchNightmareText.text = DefragmentedData.Text;
    }

    [Serializable]
    private struct ScoreBoardSaveData
    {
        public DefecultySaveData EasyTimeTraile;
        public DefecultySaveData NormalTimeTraile;
        public DefecultySaveData HardTimeTraile;
        public DefecultySaveData NightmareTimeTraile;
        public DefecultySaveData EasyStopwatch;
        public DefecultySaveData NormalStopwatch;
        public DefecultySaveData HardStopwatch;
        public DefecultySaveData NightmareStopwatch;
        //public byte[] EasyTimeTraile;
        //public byte[] NormalTimeTraile;
        //public byte[] HardTimeTraile;
        //public byte[] NightmareTimeTraile;
        //public byte[] EasyStopwatch;
        //public byte[] NormalStopwatch;
        //public byte[] HardStopwatch;
        //public byte[] NightmareStopwatch;
    }

    private string jsonSavePath
    {
        get 
        {
            return ($"{Application.dataPath}/{_name}.json");
        }
    }

    [Serializable]
    private struct DefecultySaveData
    {
        [MarshalAs(UnmanagedType.ByValArray)]
        public float[] Scores;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 400)]
        public string Text;
    }

    #region From https://stackoverflow.com/questions/3278827/how-to-convert-a-structure-to-a-byte-array-in-c
    DefecultySaveData fromBytes(byte[] arr)
    {
        DefecultySaveData str = new DefecultySaveData();

        int size = Marshal.SizeOf(str);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        str = (DefecultySaveData)Marshal.PtrToStructure(ptr, str.GetType());
        Marshal.FreeHGlobal(ptr);

        return str;
    }

    byte[] getBytes(DefecultySaveData str)
    {
        int size = Marshal.SizeOf(str);
        byte[] arr = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(str, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }
    #endregion From https://stackoverflow.com/questions/3278827/how-to-convert-a-structure-to-a-byte-array-in-c

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
