using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System;

public class AR1_detectChangeColor : MonoBehaviour, ITrackableEventHandler, IVirtualButtonEventHandler
{
    public RawImage background;
    public GameObject vbBtn;
    public GameObject clues;
    public GameObject beforePressed;
    public GameObject isPressed;
    public TextMesh text;
    private static bool cluesCollected = false;

    // Start is called before the first frame update
    void Start()
    {
        vbBtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        TrackableBehaviour trackableBehaviour = GetComponent<TrackableBehaviour>();
        if (trackableBehaviour)
        {
            trackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }
    
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if ((newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED) && !cluesCollected)
        {
            background.color = Color.red;
        }
        else
        {
            background.color = Color.white;
        }
    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        beforePressed.SetActive(false);
        isPressed.SetActive(true);
        text.text = "Pressed";
        cluesCollected = true;
        text.color = Color.white;
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        isPressed.SetActive(false);
        beforePressed.SetActive(false);
        clues.SetActive(true);
    }
}
