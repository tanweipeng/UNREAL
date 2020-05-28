using System.Collections;
using UnityEngine;
using Vuforia;

public class CameraImageAccess : MonoBehaviour
{
    private PIXEL_FORMAT mPixelFormat = PIXEL_FORMAT.UNKNOWN_FORMAT;
    private bool mAccessCameraImage = true;
    private bool mFormatRegistered = false;

    // Start is called before the first frame update
    void Start()
    {
        mPixelFormat = PIXEL_FORMAT.GRAYSCALE;
        mPixelFormat = PIXEL_FORMAT.RGB888;

        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnPause);
    }
    
    void OnVuforiaStarted()
    {
        if(CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            mFormatRegistered = true;
        }
        else
        {
            mFormatRegistered = false;
        }
    }

    void OnTrackablesUpdated()
    {
        if (mFormatRegistered)
        {
            if (mAccessCameraImage)
            {
                Vuforia.Image image= CameraDevice.Instance.GetCameraImage(mPixelFormat);
            }
        }
    }

    void OnPause(bool paused)
    {
        if (paused)
        {
            UnregisterFormat();
        }
        else
        {
            RegisterFormat();
        }
    }

    void RegisterFormat()
    {
        if(CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            mFormatRegistered = true;
        }
        else
        {
            mFormatRegistered = false;
        }
    }

    void UnregisterFormat()
    {
        CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
        mFormatRegistered = false;
    }
}
