using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level  {
    public int TotalRound;
    public Round[] RoundsList;
    public int CurrentRound;

    public Level(int roundNum,List<Round.RoundInfo> roundInfoList)
    {
        TotalRound = roundNum;
        RoundsList = new Round[TotalRound];
        //对数组的赋值
        for (int i = 0; i < TotalRound; i++)
        {
            RoundsList[i] = new Round(roundInfoList[i].mMonsterIDList,i,this);
        }
        for (int i = 0; i < TotalRound; i++)
        {
            if (i==TotalRound-1)
            {
                break;
            }
            RoundsList[i].SetNextRound(RoundsList[i + 1]);
        }
    }

    public void HandleRound()
    {
        if (CurrentRound>=TotalRound)
        {
            //波次全部结束
        }
        else if(CurrentRound == TotalRound-1)
        {
            //最后一波怪
        }
        else
        {
            RoundsList[CurrentRound].Handle(CurrentRound);
        }
    }

    public void HandleLastRound()
    {
        RoundsList[CurrentRound].Handle(CurrentRound);
    }
    public void AddRoundNum()
    {
        CurrentRound++;
    }

}
