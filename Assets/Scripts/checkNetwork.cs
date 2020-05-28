using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkNetwork : MonoBehaviour
{
    public GameObject noInternetAlert;

    // Start is called before the first frame update
    void Start()
    {
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            noInternetAlert.SetActive(true);
        }
        else
        {
            noInternetAlert.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            noInternetAlert.SetActive(true);
        }
        else
        {
            noInternetAlert.SetActive(false);
        }
    }
}
