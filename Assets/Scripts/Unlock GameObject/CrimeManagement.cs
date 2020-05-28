using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CrimeManagement : MonoBehaviour
{
    public int num_clues;
    public int index;
    public string zone;
    public string MRT;
    public int[] clues = new int[5];
    int count = 0;
    private bool solve;

    private void Start()
    {
        solve = true;
        for (int i = 0; i < num_clues; i++)
        {
            if (database.clueValue[clues[i] - 1] == false)
            {
                solve = false;
                count = num_clues;
                break;
            }
            count++;
        }
        if (count == num_clues && solve == true)
        {
            database.allCluesSolved[index - 1] = true;
            database.set_allCluesSolved_online("Zone " + zone, MRT, index);
        }
    }

    private void Update()
    {
        
    }
}
