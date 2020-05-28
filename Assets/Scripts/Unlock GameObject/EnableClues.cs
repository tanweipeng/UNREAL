using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableClues : MonoBehaviour
{
    public GameObject clue;
    public int i;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log((i) + " clue = " + database.clueValue[i - 1]);
        if (database.clueValue[i - 1] == true)
        {
            clue.SetActive(true);
        }
    }

    void Update()
    {

    }
}
