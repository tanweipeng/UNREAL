using System.Collections;
using PullToRefresh;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;
using System;

public class RefreshScene: MonoBehaviour
{
    [SerializeField] private UIRefreshControl m_UIRefreshControl;
    [SerializeField] private TMP_Text player_marks;
    [SerializeField] private TMP_Text player_name;
    public GameObject mainFadeControl;

    private void Start()
    {
        // Register callback
        // This registration is possible even from Inspector.
        Debug.Log("Marks = " + database.get_marks());
        player_marks.text = Convert.ToInt32(database.get_marks()).ToString();
        Debug.Log("Name = " + database.get_grpName());
        player_name.text = database.get_grpName();
        Debug.Log(database.clueValue[0]);
        Debug.Log(database.clueValue[1]);
        mainFadeControl.SetActive(true);
        m_UIRefreshControl.OnRefresh.AddListener(RefreshItems);
    }
    

    private void RefreshItems()
    {
        //to get data from firebase
        StartCoroutine(FetchDataDemo());
        getData();
    }

    private IEnumerator FetchDataDemo()
    {
        // Instead of data acquisition.
        yield return new WaitForSeconds(1f);
        // Call EndRefreshing() when refresh is over.
        m_UIRefreshControl.EndRefreshing();
    }

    //to get data from firebase and update
    public void getData()
    {
        Debug.Log("Marks = " + database.get_marks());
        player_marks.text = Convert.ToInt32(database.get_marks()).ToString();
        Debug.Log("Name = " + database.get_grpName());
        player_name.text = database.get_grpName();
    }

    // Register the callback you want to call to OnRefresh when refresh starts.
    public void OnRefreshCallback()
    {
        Debug.Log("OnRefresh called.");
    }
}
