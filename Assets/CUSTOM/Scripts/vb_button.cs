using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class vb_button : MonoBehaviour, IVirtualButtonEventHandler
{
    public GameObject vbBtn;
    public GameObject clues;
    public GameObject beforePressed;
    public GameObject isPressed;
    public TextMesh text;
    private static bool cluesCollected = false;

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        beforePressed.SetActive(false);
        isPressed.SetActive(true);
        text.text = "Pressed";
        cluesCollected = true;
        text.color = Color.white;
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        isPressed.SetActive(false);
        beforePressed.SetActive(false);
        clues.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        vbBtn.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool collected()
    {
        return cluesCollected;
    }
}
