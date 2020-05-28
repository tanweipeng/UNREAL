using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneOneControl : MonoBehaviour
{
    public GameObject logInArea;
    public GameObject quitArea;
    public GameObject firstFadeControl;

    // Start is called before the first frame update
    void Start()
    {
        firstFadeControl.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && logInArea.activeInHierarchy)
        {
            logInArea.SetActive(false);
            quitArea.SetActive(true);
        }
    }

}
