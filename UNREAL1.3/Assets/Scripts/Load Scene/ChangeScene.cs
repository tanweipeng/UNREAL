using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public GameObject first;
    public GameObject second;
    public GameObject firstFadeControl;
    public GameObject loading;

    public void changeCanvas()
    {
        //firstFadeControl.SetActive(false);
        first.SetActive(false);
        second.SetActive(true);
    }

    public void stopLoading()
    {
        loading.SetActive(false);
    }

    public void quit()
    {
        Application.Quit();
    }
}
