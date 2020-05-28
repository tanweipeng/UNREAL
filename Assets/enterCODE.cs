using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class enterCODE : MonoBehaviour
{
    private int RGB = 860208;
    private int Attack = 238940;
    private int MorseCode = 669373;

    private int Waze = 822910;
    private int Count = 875472;
    private int Escape = 877136;
    private int Fill = 889407;
    private int Barcode = 941356;

    private int Flip = 918175;
    private int Braille = 647345;
    private int Picture = 624595;

    private int Music = 344220;
    private int Lab = 063072;

    public GameObject Q_TTDI_SS20_5;
    public GameObject Q_TTDI_Plaza;
    public GameObject Q_TTDI_SS20_15;

    public GameObject Q_MN_parking_lot;
    public GameObject Q_MN_angkasa;
    public GameObject Q_MN_stage;
    public GameObject Q_MN_house_area;
    public GameObject Q_MN_blue_stairs;
    public GameObject Q_MN_indoor;

    public GameObject Q_Semantan_bridge;
    public GameObject Q_Semantan_private_parking;
    public GameObject Q_Semantan_SKM;

    public GameObject Q_PS_dataran;
    public GameObject Q_PS_river;

    public TMP_InputField field;
    public GameObject code;
    public GameObject invalid;

    public void zone1_code()
    {
        if (int.Parse(field.text) == RGB)
        {
            field.text = "";
            code.SetActive(false);
            Q_TTDI_SS20_5.SetActive(true);
        }
        else if(int.Parse(field.text) == Attack)
        {
            field.text = "";
            code.SetActive(false);
            Q_TTDI_Plaza.SetActive(true);
        }
        else if(int.Parse(field.text) == MorseCode)
        {
            field.text = "";
            code.SetActive(false);
            Q_TTDI_SS20_15.SetActive(true);
        }
        else
        {
            field.text = "";
            invalid.SetActive(true);
        }
    }

    public void zone2_code()
    {
        if (int.Parse(field.text) == Waze)
        {
            field.text = "";
            code.SetActive(false);
            Q_MN_parking_lot.SetActive(true);
        }
        else if (int.Parse(field.text) == Count)
        {
            field.text = "";
            code.SetActive(false);
            Q_MN_angkasa.SetActive(true);
        }
        else if (int.Parse(field.text) == Escape)
        {
            field.text = "";
            code.SetActive(false);
            Q_MN_stage.SetActive(true);
        }
        else if(int.Parse(field.text) == Fill)
        {
            field.text = "";
            code.SetActive(false);
            Q_MN_house_area.SetActive(true);
            Q_MN_blue_stairs.SetActive(true);
        }
        else if(int.Parse(field.text) == Barcode)
        {
            field.text = "";
            code.SetActive(false);
            Q_MN_indoor.SetActive(true);
        }
        else
        {
            field.text = "";
            invalid.SetActive(true);
        }
    }

    public void zone3_code()
    {
        if (int.Parse(field.text)  == Flip)
        {
            field.text = "";
            code.SetActive(false);
            Q_Semantan_bridge.SetActive(true);
        }
        else if (int.Parse(field.text) == Braille)
        {
            field.text = "";
            code.SetActive(false);
            Q_Semantan_private_parking.SetActive(true);
        }
        else if (int.Parse(field.text) == Picture)
        {
            field.text = "";
            code.SetActive(false);
            Q_Semantan_SKM.SetActive(true);
        }
        else
        {
            field.text = "";
            invalid.SetActive(true);
        }
    }

    public void zone4_code()
    {
        if (int.Parse(field.text) == Music)
        {
            field.text = "";
            code.SetActive(false);
            Q_PS_dataran.SetActive(true);
        }
        else if (int.Parse(field.text) == Lab)
        {
            field.text = "";
            code.SetActive(false);
            Q_PS_river.SetActive(true);
        }
        else
        {
            field.text = "";
            invalid.SetActive(true);
        }
    }
}
