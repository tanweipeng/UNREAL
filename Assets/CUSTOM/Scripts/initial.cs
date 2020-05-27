using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initial : MonoBehaviour
{
    public Animator anim;
    public string anim_name;

    // Start is called before the first frame update
    void Start()
    {
        anim.Play(anim_name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
