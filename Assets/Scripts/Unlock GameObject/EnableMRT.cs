using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnableMRT : MonoBehaviour
{
    public int index;
    public GameObject tick;
    public GameObject cross;
    public TMP_Text solve;

    // Start is called before the first frame update
    void Start()
    {
        if(database.MRTvalue[index - 1])
        {
            tick.SetActive(true);
            cross.SetActive(false);
            solve.text = "SOLVED";
        }
    }

    private void Update()
    {
        if (database.MRTvalue[index - 1])
        {
            tick.SetActive(true);
            cross.SetActive(false);
            solve.text = "SOLVED";
        }
    }
}
