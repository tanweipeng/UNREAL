using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class checkAnswer : MonoBehaviour
{
    public TMP_InputField field;
    public GameObject wrong;
    public GameObject self;
    public GameObject open;

    public void RGB()
    {
        if (field.text.Equals("38594abda793"))
        {

        }
        else
        {
            field.text = "";
            wrong.SetActive(true);
        }
    }

    public void Attack()
    {
        
    }

    public void MorseCode()
    {

    }

    public void Waze()
    {

    }

    public void Count()
    {

    }

    public void Escape()
    {

    }

    public void Fill()
    {

    }

    public void Barcode()
    {

    }

    public void Flip()
    {
        if (field.text.Equals("2143654"))
        {
            field.text = "";
            self.SetActive(false);
            open.SetActive(true);
        }
        else
        {
            field.text = "";
            wrong.SetActive(true);
        }
    }

    public void Braille()
    {
        if (field.text.Equals("812100905"))
        {
            field.text = "";
            self.SetActive(false);
            open.SetActive(true);
        }
        else
        {
            field.text = "";
            wrong.SetActive(true);
        }
    }

    public void Picture()
    {
        if (field.text.Equals("RSA"))
        {
            field.text = "";
            self.SetActive(false);
            open.SetActive(true);
        }
        else
        {
            field.text = "";
            wrong.SetActive(true);
        }
    }

    public void Music()
    {
        if (field.text.Equals("A6EF4"))
        {
            field.text = "";
            self.SetActive(false);
            open.SetActive(true);
        }
        else
        {
            field.text = "";
            wrong.SetActive(true);
        }
    }

    public void Lab()
    {
        if(field.text.StartsWith("121") || field.text.StartsWith("151"))
        {
            field.text = "";
            self.SetActive(false);
            open.SetActive(true);
        }
        else
        {
            field.text = "";
            wrong.SetActive(true);
        }
    }
}

