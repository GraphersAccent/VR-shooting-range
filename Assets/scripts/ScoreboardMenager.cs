using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardMenager : MonoBehaviour
{
    [SerializeField] private ScoreBoard _5MeterBoard;
    [SerializeField] private ScoreBoard _10MeterBoard;
    [SerializeField] private ScoreBoard _15MeterBoard;
    [SerializeField] private ScoreBoard _20MeterBoard;

    public void NewScoreInput(TargitResponse Distance, TargitResponse Difeculty, TargitResponse GameType, float score)
    {
        switch (Distance)
        {
            case TargitResponse.M5:
                _5MeterBoard.NewScoreInput(Difeculty, GameType, score);
                break;
            case TargitResponse.M10:
                _10MeterBoard.NewScoreInput(Difeculty, GameType, score);
                break;
            case TargitResponse.M15:
                _15MeterBoard.NewScoreInput(Difeculty, GameType, score);
                break;
            case TargitResponse.M20:
                _20MeterBoard.NewScoreInput(Difeculty, GameType, score);
                break;
        }
    }
}
