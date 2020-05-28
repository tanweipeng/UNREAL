using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initial_anim : MonoBehaviour
{
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim.Play("option_anim");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
