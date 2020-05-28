using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Threading.Tasks;
using TMPro;
//sign in script settled. It's with initialisation of firebase
public class SignIn : MonoBehaviour
{
    public TMP_InputField Email;
    public TMP_InputField Password;
    public GameObject loading;
    public GameObject first_fade;

    public GameObject error;
    public GameObject short_password;
    public GameObject wrong_format;
    public GameObject someone_loggedIn;

    public Animator change;

    FirebaseUser newUser;
    
    DatabaseReference reference;
    DataSnapshot LoggedInDataSnapshot = null;
    FirebaseApp app;
    FirebaseAuth auth;

    //bool changetextfail;
    bool changeScene;
    bool valid;
    bool isLoggedIn;

    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    void Start()
    {
        changeScene = false;
        valid = true;
        isLoggedIn = false;

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
        //app.SetEditorDatabaseUrl("https://unreal-6279a.firebaseio.com/");
        app.SetEditorDatabaseUrl("https://unreal2020-38e7b.firebaseio.com/");
        if (app.Options.DatabaseUrl != null) app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        //auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);
        Debug.Log("Initialization of firebase ended.(In SignIn Script.)");
    }

    // Update is called once per frame
    void Update()
    {
        //load the scene
        //everthing ok
        if (changeScene && valid && (!isLoggedIn))
        {
            //change this
            database.set_userID(newUser.UserId);
            database.PullAllData();
            loading.SetActive(false);
            first_fade.SetActive(true);
            change.Play("first_fade_in");
        }
        //is Logged In
        if (isLoggedIn)
        {
            loading.SetActive(false);
            someone_loggedIn.SetActive(true);//is Log In
        }
        //no Logged In, but wrong
        //if (changeScene && !valid && (!isLoggedIn))
        //{
        //    loading.SetActive(false);
        //    if (!Email.text.Contains("@unreal.com"))
        //        wrong_format.SetActive(true);
            
        //}
        if(!changeScene && !valid && (!isLoggedIn))
        {
            loading.SetActive(false);
            if (!Email.text.Contains("@unreal.com"))
                wrong_format.SetActive(true);
            else if (Password.text.Length < 6)
                short_password.SetActive(true);
            else
                error.SetActive(true);
        }

    } 

    public void retry()
    {
        isLoggedIn = false;
        changeScene = false;
        valid = true;
    }
    
    public void SignInWithEmailAndPasswordAsync()
    {
        loading.SetActive(true);
        Debug.Log("At the start of sign in");
        FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(Email.text.Replace(" ", "").ToLower(), Password.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                valid = false;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                valid = false;
                return;
            }
            //if no error means successfully login
            newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            
            //even though login successfully we still need to check if the acc login on other devices
            FirebaseDatabase.DefaultInstance
            .GetReference("Groups/" + newUser.UserId + "/isLoggedIn").GetValueAsync().ContinueWith(signIntask =>
             {
                 if (signIntask.IsFaulted)
                 {
                     //Handle the error...
                     Debug.LogError("Get the value of isLoggedIn encounter an error(task.IsFaulted) :" + signIntask.Exception);
                 }
                 else if (signIntask.IsCompleted)
                 {
                     LoggedInDataSnapshot = signIntask.Result;
                     isLoggedIn = Convert.ToBoolean(LoggedInDataSnapshot.GetValue(true).ToString());
                     if (isLoggedIn)
                     {
                         //if there is another device logging into other device
                         changeScene = false;
                         Debug.Log("Someone log in your account...");
                     }
                     else
                     {
                         //reference.Child("Groups").Child(newUser.UserId).Child("isLoggedIn").SetValueAsync(true);
                         //continue with loading scene
                         changeScene = true;
                         Debug.Log("No one logged in into your account, logging in...Enjoy!");
                         //ChangeScene();
                     }
                     //Do something with snapshot...
                 }
                 else if (signIntask.IsCanceled)
                 {
                     Debug.Log("Get the value of isLoggedIn encounter an error(task.IsCanceled) :" + task.Exception);
                 }
                 else
                 {
                     Debug.Log("Enter else...");
                 }
             });
        });
    }
    //all method below for my personal usage
    private void OnDisable()
    {

        if (newUser != null)
        {
            Debug.Log("NewUser is not null.Loading into another scene with a uid token");
            PlayerPrefs.SetString("uidOfUser", newUser.UserId);
            newUser = null;
        }
        else
        {
            Debug.Log("The new User is null! You haven't login the user but end the scene.");
        }
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

