using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uni : MonoBehaviour
{
    public GameObject toClose;

    public void disable()
    {
        toClose.SetActive(false);
    }
}
