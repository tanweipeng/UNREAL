using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTwoControl : MonoBehaviour
{
    public Animator secondAnim;
    public GameObject secondFadeControl;

    // Start is called before the first frame update
    void Start()
    {
        secondFadeControl.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void transition()
    {
        secondAnim.Play("second_fade_in");
    }
    
    void changeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
