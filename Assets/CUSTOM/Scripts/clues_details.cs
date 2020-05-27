using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clues_details : MonoBehaviour
{
    public GameObject toClose;

    public void disable()
    {
        toClose.SetActive(false);
    }
}
