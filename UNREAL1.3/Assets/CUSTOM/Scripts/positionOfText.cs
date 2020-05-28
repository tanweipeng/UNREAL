using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionOfText : MonoBehaviour
{
    public GameObject bp;
    public Vector3 target;
    private Vector3 origin;

    private void Start()
    {
        origin = bp.transform.position;
    }

    public void backToPostion()
    {
        bp.transform.position = origin;
    }
}
