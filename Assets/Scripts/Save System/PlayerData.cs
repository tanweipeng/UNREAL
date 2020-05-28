using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerData
{
    public string grpName;
    public double marks;
    public string userID;
    public bool isLoggedIn;
    public bool success;
    public IDictionary<int, bool> MRTvalue;

    //public static bool[] MRTvalue = new bool[4];
    public IDictionary<int, bool> get_MRTValue()
    {
        return MRTvalue;
    }
    public bool getCertain_MRTValue(int index)
    {
        return MRTvalue[index];
    }

    public IDictionary<int, int> mentalTaskValue;

    //public static int[] mentalTaskValue = new int[13];
    public IDictionary<int, int> get_mentalTaskValue()
    {
        return mentalTaskValue;
    }
    public int getCertain_mentalTaskValue(int index)
    {
        return mentalTaskValue[index];
    }

    public IDictionary<int, bool> clueValue;

    //public static bool[] clueValue = new bool[65];
    public IDictionary<int, bool> get_clueValue()
    {
        return clueValue;
    }
    public bool getCertain_clueValue(int index)
    {
        return clueValue[index];
    }

    public IDictionary<int, bool> allCluesSolved;

    //public static bool[] allCluesSolved = new bool[13];
    public IDictionary<int, bool> get_allCluesSolved()
    {
        return allCluesSolved;
    }
    public bool getCertain_allCluesSolved(int index)
    {
        return allCluesSolved[index];
    }

    public PlayerData()
    {
        grpName = database.get_grpName();
        marks = database.get_marks();
        userID = database.get_userID();
        isLoggedIn = database.get_isLoggedIn();
        success = database.get_success();

        //for (int a = 0; a < 4; a++)
        //    MRTvalue[a] = database.MRTvalue[a];
        //for (int b = 0; b < 13; b++)
        //    mentalTaskValue[b] = database.mentalTaskValue[b];
        //for(int c = 0; c < 65; c++)
        //    clueValue[c] = database.clueValue[c];
        //for (int d = 0; d < 13; d++)
        //    allCluesSolved[d] = database.allCluesSolved[d];
        MRTvalue = database.MRTvalue;
        mentalTaskValue = database.mentalTaskValue;
        clueValue = database.clueValue;
        allCluesSolved = database.allCluesSolved;
    }
}
