using UnityEngine;
using UnityEngine.SceneManagement;

public class secondChangeScene : MonoBehaviour
{
    void change()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
