using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StreamVideo : MonoBehaviour
{
    public RawImage video_panel;
    public VideoPlayer video_source;
    public AudioSource audio_source;
    public GameObject play_button;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayVideo());
    }

    IEnumerator PlayVideo()
    {
        video_source.Prepare();
        WaitForSeconds waitForSeconds = new WaitForSeconds(0);
        while (!video_source.isPrepared)
        {
            yield return waitForSeconds;
            break;
        }
    }

    public void play_video()
    {
        video_panel.color = new Color(1, 1, 1, 1);
        video_panel.texture = video_source.texture;
        video_source.Play();
        audio_source.Play();
        play_button.SetActive(false);
    }

    private void Update()
    {
        if (video_source.isPaused)
        {
            play_button.SetActive(true);
            video_panel.color = new Color(1, 1, 1, 0.3921569f);
        }
    }
}
