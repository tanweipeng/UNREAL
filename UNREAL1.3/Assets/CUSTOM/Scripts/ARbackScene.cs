using UnityEngine;
using UnityEngine.SceneManagement;

public class ARbackScene : MonoBehaviour
{
    public int index;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(index);
    }

    public void backScene()
    {
        SceneManager.LoadScene(index);
    }
}
