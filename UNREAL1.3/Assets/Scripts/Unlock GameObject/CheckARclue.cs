using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckARclue : MonoBehaviour
{
    public int clue_index;
    public GameObject image_target;
    //public GameObject vBtn;
    //public GameObject before_pressed;
    //public GameObject is_pressed;
    //public GameObject text;
    public GameObject extra;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(clue_index + " clue = " + database.clueValue[clue_index - 1]);
        if (database.clueValue[clue_index - 1])
        {
            image_target.SetActive(false);
            //vBtn.SetActive(false);
            //before_pressed.SetActive(false);
            //is_pressed.SetActive(false);
            //text.SetActive(false);
            extra.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
