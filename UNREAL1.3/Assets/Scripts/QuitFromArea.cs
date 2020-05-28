using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitFromArea : MonoBehaviour
{
    public Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            anim.Play("zoom_in");
        }
    }
}
