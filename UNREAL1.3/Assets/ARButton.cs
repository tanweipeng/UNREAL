using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Vuforia;
using UnityEngine.UI;

public class ARButton : MonoBehaviour, ITrackableEventHandler
{
    public RawImage background;
    //public GameObject vbBtn;
    //public GameObject beforePressed;
    //public GameObject isPressed;
    //public GameObject text_object;
    //public TextMesh text;
    public GameObject clues;
    public GameObject imageTarget;
    public GameObject extra;
    public GameObject clue_btn;

    // Start is called before the first frame update
    void Start()
    {
        //vbBtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        TrackableBehaviour trackableBehaviour = GetComponent<TrackableBehaviour>();
        if (trackableBehaviour)
        {
            trackableBehaviour.RegisterTrackableEventHandler(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if ((newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED) && imageTarget.activeInHierarchy)
        {
            clue_btn.SetActive(true);
            background.color = Color.red;
        }
        else
        {
            clue_btn.SetActive(false);
            background.color = Color.white;
        }
    }

    public void Click()
    {
        clues.SetActive(true);
        imageTarget.SetActive(false);
        extra.SetActive(false);
    }
}
