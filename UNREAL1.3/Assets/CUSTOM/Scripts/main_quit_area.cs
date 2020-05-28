using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class main_quit_area : MonoBehaviour
{
    public GameObject black;

    public void Exit()
    {
        black.SetActive(true);
        Application.Quit();
    }

}
