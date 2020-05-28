using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitFromMain : MonoBehaviour
{
    public GameObject quit_Area;
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !quit_Area.activeInHierarchy)
        {
            quit_Area.SetActive(true);
        }
    }
}
