using UnityEngine;
using TMPro;

public class EnableCrime : MonoBehaviour
{
    public int MT_index;
    public TMP_Text solved;
    // Start is called before the first frame update
    void Start()
    {
        if (database.allCluesSolved[MT_index - 1])
            solved.text = "Solved";
    }

    void Update()
    {
        if (database.allCluesSolved[MT_index - 1])
            solved.text = "Solved";
    }
}
