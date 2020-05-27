using System.Collections;
using UnityEngine;
using Vuforia;

public class cameraFocusController : MonoBehaviour
{
    private bool mVuforiaStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        VuforiaARController vuforia = VuforiaARController.Instance;
        if (vuforia != null)
            vuforia.RegisterVuforiaStartedCallback(StartAfterVuforia);
    }

    private void StartAfterVuforia()
    {
        mVuforiaStarted = true;
        SetAutoFocus();
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            if (mVuforiaStarted)
            {
                SetAutoFocus();
            }
        }
    }

    private void SetAutoFocus()
    {
        if (CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO))
        {
            Debug.Log("AutoFocus set");
        }
        else
        {
            Debug.Log("this device doesn't support auto focus");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
