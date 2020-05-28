using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quit_area : MonoBehaviour
{
    public GameObject logInArea;
    public GameObject quitArea;

    void cancel()
    {
        logInArea.SetActive(true);
        quitArea.SetActive(false);
    } 

    public void Exit()
    {
        Application.Quit();
    }

}
