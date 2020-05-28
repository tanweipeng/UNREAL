using System.Collections;
using UnityEngine;

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class timer : MonoBehaviour
{
    public Text timeLeft;
    public Text realTime;
    private int startTime;
    private int startIn;
    private string time;
    private int hour;
    private int timePassed;

    public IEnumerator GetInternetTime()
    {
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("http://www.google.com");
        yield return myHttpWebRequest.Send();

        string netTime = myHttpWebRequest.GetResponseHeader("date");
        startIn = netTime.IndexOf("2020") + 5;
        time = (netTime.Substring(startIn, 8));
        Debug.Log(timePassed);
        Debug.Log(time);
    }

    public async void countdown()
    {
        StartCoroutine(GetInternetTime());
        await Task.Delay(3000);
        string temp = time.Substring(0, 2);
        hour = int.Parse(temp);
        temp = time.Substring(3, 2);
        int min = int.Parse(temp);
        temp = time.Substring(6, 2);
        int sec = int.Parse(temp);
        startTime = (hour * 60 * 60) + (min * 60) + sec;
        do
        {
            hour = int.Parse(time.Substring(0, 2));
            min = int.Parse(time.Substring(3, 2));
            sec = int.Parse(time.Substring(6, 2));
            int currentTime = hour * 60 * 60 + min * 60 + sec;

            timePassed = currentTime - startTime;
            int tempTime = timePassed;
            sec = tempTime % 60;
            tempTime -= sec;
            min = (tempTime % 3600) / 60;
            tempTime -= min * 60;
            hour = tempTime / 3600;
            string toPrint = hour.ToString() + ":" + min.ToString() + ":" + sec.ToString();
            // timeLeft = GetComponent<Text>();                        //display time left
            timeLeft.text = toPrint;
            Debug.Log(toPrint + " is toPrint");
            // realTime = GetComponent<Text>();               //display time currently
            realTime.text = time;
            Debug.Log(time + " is time");
            Debug.Log(timePassed);
            StartCoroutine(GetInternetTime());
        } while (timePassed < 600); //change time limit here
        Debug.Log("Time is up");
        //insert what happens afterthe time is up

        //await Task.Delay(1000);
    }

}
