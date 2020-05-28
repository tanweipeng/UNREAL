using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PushAndPull
{
    DatabaseReference reference;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser newUser;
    DataSnapshot Name = null;
    FirebaseApp app;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Start.(In Sign In)");
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
        app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.
        app.SetEditorDatabaseUrl("https://unreal-6279a.firebaseio.com/");
        if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        //auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);
        Debug.Log("Initialization of firebase ended.(In SignIn Script.)");
    }
    public void Pull()
    {
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        //here is very important!
        FirebaseDatabase.DefaultInstance
        .GetReference("Groups/Total Num Of Grp").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Name = task.Result;
                Debug.Log("Update become true.");
                string GroupName = Name.GetValue(true).ToString();
                Debug.Log("We get " + GroupName);
                //Do something with snapshot...
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("task.IsCanceled:" + task.Exception);
            }
        }, taskSchedular);
    }
    public void Push()
    {
        string newEmail = "";
        string newPassword = "";
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(newEmail + "@unreal.com", newPassword).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            newUser = task.Result;
            //changetextsuccess = true;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            ///start updating
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        });
    }
}
