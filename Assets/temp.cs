using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    public GameObject crime1;
    public GameObject zone1;
    
    public void backZone()
    {
        crime1.SetActive(false);
        zone1.SetActive(true);
    }
}
