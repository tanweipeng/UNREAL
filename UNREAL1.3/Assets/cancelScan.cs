using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cancelScan : MonoBehaviour
{
    public GameObject ImageTarget;

    public void collect_off()
    {
        ImageTarget.SetActive(false);
    }
}
