using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MRTManagement : MonoBehaviour
{
    public int num_clues;
    public int initial;
    public int final;
    public string zone;
    public string MRT;
    public int MRT_index;
    int count = 0;
    private bool MRT_solved;

    private void Start()
    {
        MRT_solved = true;
        for (int i = initial - 1; i < final; i++)
        {
            if (!database.clueValue[i])
            {
                MRT_solved = false;
                count = num_clues;
                break;
            }
            count++;
        }
        if (count == num_clues && MRT_solved == true)
        {
            database.MRTvalue[MRT_index - 1] = true;
            database.set_MRTValue_online("Zone " + zone, MRT);
        }
    }

    private void Update()
    {
        
    }
}
