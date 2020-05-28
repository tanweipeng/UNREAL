using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using System;

public class Player_Manager : MonoBehaviour
{
    public TMP_Text grpName;
    public TMP_Text marks;
    public GameObject fail_killer;
    public GameObject success_killer;
    public GameObject killer_page;
    public GameObject please_solve_all;
    public TMP_Text deduct;
    public TMP_Text final;

    private void Start()
    {
        SaveSystem.SaveData();
        Debug.Log("Successfully Save Data");
        if (!database.get_isLoggedIn())
        {
            database.set_isLoggedIn(true);
            database.set_LoggedIn_true_onine();
        }
        CheckPlayerSuccess();
    }

    public void DeletePlayer()
    {
        SaveSystem.DeleteData();
        Debug.Log("Successfully Delete Data");
    }

    public void CheckAllSolved()
    {
        if (database.MRTvalue[0] && database.MRTvalue[1] && database.MRTvalue[2] && database.MRTvalue[3])
            killer_page.SetActive(true);
        else
            please_solve_all.SetActive(true);
    }
    public void CheckPlayerSuccess()
    {
        if (database.get_success())
            Destroy(killer_page);
    }

    public void FindKiller(string name)
    {
        if (name.Equals("Tom"))
        {
            database.CorrectKiller();
            marks.text = Convert.ToInt32(database.get_marks()).ToString();
            success_killer.SetActive(true);
            final.text = "FINAL MARKS = " + Convert.ToInt32(database.get_marks()).ToString();
            database.SetAllTrue();
        }
        else
        {
            fail_killer.SetActive(true);
            database.WrongKiller();
            deduct.text = "Marks deducted -> " + Convert.ToInt32(database.get_marks()).ToString();
            marks.text = Convert.ToInt32(database.get_marks()).ToString();
        }
        SaveSystem.SaveData();
    }

    public void closeApplication()
    {
        Application.Quit();
    }
}
