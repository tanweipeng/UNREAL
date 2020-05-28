using UnityEngine;
using UnityEngine.SceneManagement;

public class backScene : MonoBehaviour
{
    public void backToMain()
    {
        SceneManager.LoadScene(1);
    }

    public void backToZone()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (this.gameObject.activeInHierarchy)
                SceneManager.LoadScene(1);
        }
    }
}
