using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDetail : MonoBehaviour
{
    public Text groupName;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    // Start is called before the first frame update
    void Start()
    {
        //FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        //{
        //    dependencyStatus = task.Result;
        //    if (dependencyStatus == DependencyStatus.Available)
        //    {
        //        Debug.Log("Start.");
        //        InitializeFirebase();
        //    }
        //    else
        //    {
        //        Debug.LogError(
        //          "Could not resolve all Firebase dependencies: " + dependencyStatus);
        //    }
        //});
    }

    // Initialize the Firebase database:
    protected virtual void InitializeFirebase()
    {
        FirebaseApp app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.
        app.SetEditorDatabaseUrl("https://unreal-6279a.firebaseio.com/");
        if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        Debug.Log("Before Display.");
        Display();
    }
    private DataSnapshot snapshot;
    private bool update = false;
    public void Display()
    {
        FirebaseDatabase.DefaultInstance
        .GetReference("users/Groups/Group1/GroupName").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                update = true;
                //Do something with snapshot...
            }
        });
        if (update)
        {
            Debug.Log("Inside Display, the value of snapshot.Value.ToString() is " + snapshot.GetValue(true));
            groupName.text = snapshot.GetValue(true).ToString();

        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
