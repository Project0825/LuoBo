using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round
{
    [System.Serializable]
    public struct RoundInfo
    {
        public int[] mMonsterIDList;
    }
    public RoundInfo roundInfo;
    protected Round mNextRound;
    protected int mRoundID;
    protected Level mLevel;

    public Round(int[] monsterIdList,int roundID,Level level){
        roundInfo.mMonsterIDList = monsterIdList;
        mRoundID = roundID;
        mLevel = level;
    }
    public void SetNextRound(Round nextRound)
    {
        mNextRound = nextRound;
    }
    public void Handle(int roundID)
    {
        if (mRoundID<roundID)
        {
            mNextRound.Handle(roundID);

        }
        else
        {
            //产生怪物

        }
    }
}
