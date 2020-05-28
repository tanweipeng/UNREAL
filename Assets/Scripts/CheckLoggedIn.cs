using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckLoggedIn : MonoBehaviour
{
    static database d = new database();
    bool change;

    void Start()
    {
        change = false;
        PlayerData data = SaveSystem.LoadData();
        if (data != null)
        {
            //group Name
            database.set_grpName(data.grpName);

            //marks
            database.set_marks(data.marks);

            //user ID
            database.set_userID(data.userID);

            //is Logged In
            database.set_isLoggedIn(data.isLoggedIn);
            
            //is Logged In
            database.set_success(data.success);

            //MRT value
            //for (int a = 0; a < 4; a++)
            //    database.MRTvalue[a] = data.getCertain_MRTValue(a);
            database.MRTvalue = data.get_MRTValue();

            //Mental Task Value
            //for (int b = 0; b < 13; b++)
            //    database.mentalTaskValue[b] = data.getCertain_mentalTaskValue(b);
            database.mentalTaskValue = data.get_mentalTaskValue();

            //Clue Value
            //for (int c = 0; c < 65; c++)
            //    database.clueValue[c] = data.getCertain_clueValue(c);
            database.clueValue = data.get_clueValue();

            Debug.Log("Load clue = " + data.getCertain_clueValue(5));

            //All Clues Solved's variable
            //for (int d = 0; d < 13; d++)
            //    database.allCluesSolved[d] = data.getCertain_allCluesSolved(d);
            database.allCluesSolved = data.get_allCluesSolved();

            database.PullAllData();
            change = true;
        }
    }

    private void Update()
    {
        if (change)
        {
            SceneManager.LoadScene(1);
        }
    }
}
