using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void next_Scene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //MRT part
    public void zone1()
    {
        SceneManager.LoadScene(2);
    }

    public void zone2()
    {
        SceneManager.LoadScene(3);
    }

    public void zone3()
    {
        SceneManager.LoadScene(4);
    }

    public void zone4()
    {
        SceneManager.LoadScene(5);
    }

    //AR Part
    public void zone1_AR()
    {
        SceneManager.LoadScene(6);
    }

    public void zone2_AR()
    {
        SceneManager.LoadScene(7);
    }

    public void zone3_AR()
    {
        SceneManager.LoadScene(8);
    }

    public void zone4_AR()
    {
        SceneManager.LoadScene(9);
    }
}
