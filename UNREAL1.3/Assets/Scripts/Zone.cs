using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone 
{
    public string nameOfZone;
    

    public Zone(string nameOfZone)
    {
        this.nameOfZone = nameOfZone;
    }
    public string NameOfZone { get { return nameOfZone; } }
}
