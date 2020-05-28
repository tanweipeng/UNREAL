using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateClue : MonoBehaviour
{
    public void calculate_clue(int clue_index)
    {
        database.calClueMark(clue_index);
    }

    public void LoadData()
    {
        PlayerData data = SaveSystem.LoadData();
    }
}
