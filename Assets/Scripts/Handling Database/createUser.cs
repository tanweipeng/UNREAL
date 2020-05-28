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

//this script create user
public class createUser : MonoBehaviour
{
    public TMP_InputField NewEmail;
    public TMP_InputField NewPassword;
    
    DataSnapshot NumOfGrp = null;
    DatabaseReference reference;
    int numOfGrp;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser newUser;
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

    //now we hv diff mrt with diff num of MT, so provided the num of MT for each unchanged, we can change the name of MT in below Dict to change the name of MT in database
    //all arrangement follow the file of MT Details

    public static IDictionary<int, string> MTNameInDatabase = new Dictionary<int, string>()
    {
        //TTDI
        //{0,"RGB(E)" },
        {0,"SS20_5" },
        //{1,"Attack(M)" },
        {1,"Plaza" },
        //{2,"MorseCode(E)" },
        {2,"SS20_15" },

        //Muzium
        //{3,"Waze(E)" },
        {3,"Parking Lot" },
        //{4,"Count(H)" },
        {4,"Angkasa" },
        //{5,"Escape(E)" },
        {5,"Stage" },
        //{6,"Fill(H)" },
        {6,"House" },
        //{7,"Barcode(E)" },
        {7,"Indoor" },

        //Semantan
        //{8,"Flip(M)" },
        {8,"Bridge" },
        //{9,"Braille(E)" },
        {9,"Private Parking" },
        //{10,"Picture(E)" },
        {10,"SKM" },
        
        //Pasar Seni
        //{11,"Music(M)" },
        {11,"Dataran" },
        //{12,"Lab(M)" },
        {12,"River" },
    };
    //BELOW TWO DICTIONARY COMMENTED, CLUES' NUM IS UNSURED

    public static IDictionary<int, bool> clueValue = new Dictionary<int, bool>() {
        //TTDI
        {0, false},{1, false},{2, false},{3, false},{4, false},
        {5, false}, {6, false},{7, false},{8, false},{9, false},
        {10, false},{11, false},{12, false},{13, false},{14, false},
        //Muzium
        {15, false},{16, false},{17, false},{18, false},{19, false},
        {20, false},{21, false},{22, false},{23, false},{24, false},
        {25, false},{26, false},{27, false},{28, false},{29, false},
        {30, false},{31, false},{32, false},{33, false},{34, false},
        {35, false},{36, false},{37, false},{38, false},{39, false},
        //Semantan
        {40, false},{41, false},{42, false},{43, false},{44, false},
        {45, false},{46, false},{47, false},{48, false},{49, false},
        {50, false},{51, false},{52, false},{53, false},{54, false},
        //Pasar Seni
        {55, false},{56, false},{57, false},{58, false},{59, false},
        {60, false},{61, false},{62, false},{63, false},{64, false}
        //{65, false},{66, false},{67, false},{68, false},{69, false}
    };

    public static IDictionary<int, int> mentalTaskValue = new Dictionary<int, int>() {
        {0, 0},
        {1, 0},
        {2, 0},
        {3, 0},
        {4, 0},
        {5, 0},
        {6, 0},
        {7, 0},

        {8, 0},
        {9, 0},
        {10, 0},
        {11, 0},
        {12, 0}
    };

    public static IDictionary<int, char> mentalTaskDifficulty = new Dictionary<int, char>() {
        {0, 'E'},
        {1, 'M'},
        {2, 'E'},
        {3, 'E'},
        {4, 'H'},
        {5, 'E'},
        {6, 'H'},
        {7, 'E'},

        {8, 'M'},
        {9, 'E'},
        {10, 'E'},
        {11, 'M'},
        {12, 'M'}
    };

    public static IDictionary<int, bool> MRTvalue = new Dictionary<int, bool>() {
        //TTDI
        {0, false},

        //Muzium
        {1, false},

        //Semantan
        {2, false},

        //Pasar Seni
        {3, false}
    };

    public static IDictionary<int, bool> allCluesSolved = new Dictionary<int, bool>()
    {
        {0, false},
        {1, false},
        {2, false},
        {3, false},
        {4, false},
        {5, false},
        {6, false},
        {7, false},

        {8, false},
        {9, false},
        {10, false},
        {11, false},
        {12, false}
    };

    private bool sign_up;
    private bool valid;
    private bool wrong_format;
    private bool empty_input;
    private bool cantCreate;
    private bool updateNumOfGrp;

    public GameObject loading;
    public GameObject error;
    public GameObject success;
    public GameObject blank;
    public GameObject format;
    public GameObject password_short;

    public TMP_Text your_acc;
    public TMP_Text your_pass;

    private void Start()
    {
        sign_up = false;
        valid = true;
        wrong_format = false;
        empty_input = false;
        cantCreate = false;
        updateNumOfGrp = false;
    }

    void Update()
    {
        //load the scene
        if (sign_up && valid)
        {
            //change this
            database.set_userID(newUser.UserId);
            database.PullAllData();
            loading.SetActive(false);
            success.SetActive(true);
            your_acc.text = "YOUR ACC : " + NewEmail.text.Replace(" ", "").ToLower() + "@unreal.com";
            your_pass.text = "YOUR PASSWORD : " + NewPassword.text;
            sign_up = false;
        }
        if (cantCreate && !valid)
        {
            loading.SetActive(false);
            if (empty_input)
                blank.SetActive(true);
            else if (NewPassword.text.Length < 6)
                password_short.SetActive(true);
            else if (wrong_format)
                format.SetActive(true);
            else
                error.SetActive(true);
        }
    }

    public void Retry()
    {
        sign_up = false;
        valid = true;
        wrong_format = false;
        empty_input = false;
        cantCreate = false;
    }

    public void CreateUserWithEmailAndPasswordAsync()
    {
        loading.SetActive(true);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskScheduler2 = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(NewEmail.text.Replace(" ","").ToLower() + "@unreal.com", NewPassword.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync was canceled.");
                valid = false;
                cantCreate = true;
                updateNumOfGrp = false;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                valid = false;
                cantCreate = true;
                updateNumOfGrp = false;
                return;
            }
            if(NewEmail.text.Equals("") || NewPassword.text.Equals(""))
            {
                valid = false;
                empty_input = true;
                cantCreate = true;
                updateNumOfGrp = false;
                return;
            }
            if (NewEmail.text.Contains("@") || NewEmail.text.Contains(".com"))
            {
                valid = false;
                wrong_format = true;
                cantCreate = true;
                updateNumOfGrp = false;
                return;
            }
            else
            {
                sign_up = true;
                updateNumOfGrp = true;
            }
            // Firebase user has been created.
            newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        }, taskScheduler2);

        //temporary sign in the user to initialise database
        var taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(NewEmail.text.Replace(" ", "").ToLower() + "@unreal.com", NewPassword.text).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync was canceled.");
                valid = false;
                updateNumOfGrp = false;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.Log("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                valid = false;
                updateNumOfGrp = false;
                return;
            }

            newUser = task.Result;
            
            Debug.LogFormat("User signed in successfully: {0} ({1})",newUser.DisplayName, newUser.UserId);
            //initialize user in database

            DatabaseReference reference2;
            auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            newUser = auth.CurrentUser;
            reference2 = reference.Child("Groups").Child(newUser.UserId);
            reference2.Child("marks").SetValueAsync("0");
            reference2.Child("Name").SetValueAsync(NewEmail.text);
            reference2.Child("isLoggedIn").SetValueAsync(false);
            reference2.Child("success").SetValueAsync(false);

            if (reference.Child("Groups").Child("Total Num Of Grp").GetValueAsync() == null)
            {
                reference.Child("Groups").Child("Total Num Of Grp").SetValueAsync(0);
            }
            //initialize zones
            DatabaseReference tempRef = reference2.Child("Zones");
            //create clue templates 
            Clue c = new Clue();
            string json = JsonUtility.ToJson(c);

            //isSolved has 4 values: 0 = NotAttempted, 1 = AttemptFailed, 2 = Completed
            //TTDI
            tempRef.Child("Zone A").Child("TTDI").Child("isSolved").SetValueAsync(false);
            //SS20.5
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[0]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[0]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[0]).Child("Clues").Child("clue2").SetValueAsync(false);
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[0]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[0]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[0]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[0]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[0]).Child("Clues").SetRawJsonValueAsync(json);

            //Plaza
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[1]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[1]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[1]).Child("Clues").Child("clue2").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[1]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[1]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[1]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[1]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[1]).Child("Clues").SetRawJsonValueAsync(json);

            //SS20.15
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[2]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[2]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[2]).Child("Clues").Child("clue2").SetValueAsync(false);
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[2]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[2]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[2]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[2]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone A").Child("TTDI").Child(MTNameInDatabase[2]).Child("Clues").SetRawJsonValueAsync(json);

            //Muzium
            tempRef.Child("Zone A").Child("Muzium").Child("isSolved").SetValueAsync(false);
            //Parking Lot
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[3]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[3]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[3]).Child("Clues").Child("clue2").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[3]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[3]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[3]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[3]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[3]).Child("Clues").SetRawJsonValueAsync(json);

            //Angkasa
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[4]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[4]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[4]).Child("Clues").Child("clue2").SetValueAsync(false);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[4]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[4]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[4]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[4]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[4]).Child("Clues").SetRawJsonValueAsync(json);

            //Stage
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[5]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[5]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[5]).Child("Clues").Child("clue2").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[5]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[5]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[5]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[5]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[5]).Child("Clues").SetRawJsonValueAsync(json);

            //House Area
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[6]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[6]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[6]).Child("Clues").Child("clue2").SetValueAsync(false);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[6]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[6]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[6]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[6]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[6]).Child("Clues").SetRawJsonValueAsync(json);

            //Indoor
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[7]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[7]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[7]).Child("Clues").Child("clue2").SetValueAsync(false);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[7]).Child("Clues").Child("clue3").SetValueAsync(false);
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[7]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[7]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[7]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone A").Child("Muzium").Child(MTNameInDatabase[7]).Child("Clues").SetRawJsonValueAsync(json);

            //Semantan
            tempRef.Child("Zone B").Child("Semantan").Child("isSolved").SetValueAsync(false);
            //Bridge
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[8]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[8]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[8]).Child("Clues").Child("clue2").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[8]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[8]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[8]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[8]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[8]).Child("Clues").SetRawJsonValueAsync(json);

            //Private Parking
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[9]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[9]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[9]).Child("Clues").Child("clue2").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[9]).Child("Clues").Child("clue3").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[9]).Child("Clues").Child("clue4").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[9]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[9]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[9]).Child("Clues").SetRawJsonValueAsync(json);

            //SKM
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[10]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[10]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[10]).Child("Clues").Child("clue2").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[10]).Child("Clues").Child("clue3").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[10]).Child("Clues").Child("clue4").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[10]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[10]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone B").Child("Semantan").Child(MTNameInDatabase[10]).Child("Clues").SetRawJsonValueAsync(json);

            //Pasar Seni
            tempRef.Child("Zone B").Child("Pasar Seni").Child("isSolved").SetValueAsync(false);
            //Dataran
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[11]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").Child("clue2").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").SetRawJsonValueAsync(json);

            //River
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[12]).Child("isSolved").SetValueAsync(0);
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").Child("clue1").SetValueAsync(false);
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").Child("clue2").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").Child("clue3").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").Child("clue4").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").Child("clue5").SetValueAsync(true);//no exist
            tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").Child("isSolved").SetValueAsync(false);
            //tempRef.Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").SetRawJsonValueAsync(json);

            //finally, logout the user
            //auth.SignOut();
        }, taskScheduler);
        //update the num of group
        FirebaseDatabase.DefaultInstance
        .GetReference("Groups/Total Num Of Grp").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted && updateNumOfGrp)
            {
                NumOfGrp = task.Result;
                Debug.Log("Update become true.");
                numOfGrp = 0;
                string currentNumOfGroup = "0";
                currentNumOfGroup = NumOfGrp.GetValue(true).ToString();
                try
                {
                    numOfGrp = System.Convert.ToInt32(currentNumOfGroup) + 1;
                    Debug.Log("The num of group become " + numOfGrp);
                }
                catch (FormatException)
                {
                    Debug.LogError("FormatException");
                    // the FormatException is thrown when the string text does 
                    // not represent a valid integer.
                }
                catch (OverflowException)
                {
                    Debug.LogError("OverflowException");
                    // the OverflowException is thrown when the string is a valid integer, 
                    // but is too large for a 32 bit integer.  Use Convert.ToInt64 in
                    // this case.
                }
                reference.Child("Groups").Child("Total Num Of Grp").SetValueAsync(numOfGrp);
                //Do something with snapshot...
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("task.IsCanceled:" + task.Exception);
            }
        });
    }
}
