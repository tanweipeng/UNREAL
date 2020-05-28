using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneTwoCamera : MonoBehaviour
{
    public Animator secondAnim;

    void transition()
    {
        secondAnim.Play("second_fade_in");
    }
}
