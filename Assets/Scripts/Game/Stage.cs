using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage  {
    public int[] mTowerIDList;
    public int mTowerIDListLength;
    public bool mAllClear;
    public int mCarrotState;
    public int mLevelID;
    public int mBigLevelID;
    public bool unLocked;
    public bool mIsRewardLevel;
    public int mToTalRound;

    public Stage(int totalRound,int towerIDListLength,int[] towerIDList,bool allClear,int carrotState,
        int levelID,int bigLevelID,bool locked,bool isRewardLevel)
    {
        mToTalRound = totalRound;
        mTowerIDListLength = towerIDListLength;
        mTowerIDList = towerIDList;
        mAllClear = allClear;
        mCarrotState = carrotState;
        mLevelID = levelID;
        unLocked = locked;
        mIsRewardLevel = isRewardLevel;
    }   
}
