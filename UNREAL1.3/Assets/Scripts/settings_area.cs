using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settings_area : MonoBehaviour
{
    public GameObject track_circlesArea;
    public GameObject groupArea;
    public GameObject settingsArea;

    public void backScene()
    {
        track_circlesArea.SetActive(true);
        groupArea.SetActive(true);
        settingsArea.SetActive(false);
    }
}
