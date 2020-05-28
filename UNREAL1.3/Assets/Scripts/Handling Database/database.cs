using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class database
{
    //private DatabaseReference reference;
    //public int teamOut;

    //group Name
    private static string grpName;
    public static string get_grpName() { return grpName; }
    public static void set_grpName(string name) { grpName = name; }

    //marks
    private static double marks;
    public static double get_marks() { return marks; }
    public static void set_marks(double n) { marks = n; }

    //isLogged
    private static bool isLoggedIn;
    public static bool get_isLoggedIn() { return isLoggedIn; }
    public static void set_isLoggedIn(bool n) { isLoggedIn = n; }

    private static bool success;
    public static bool get_success() { return success; }
    public static void set_success(bool n) { success = n; }



    //now we hv diff mrt with diff num of MT, so provided the num of MT for each unchanged, we can change the name of MT in below Dict to refer to the correct name of MT in database
    //all MT follow the sequence of the pdf file
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

    //user ID
    private static string userID;
    public static string get_userID() { return userID; }
    public static void set_userID(string user) { userID = user; }

    //Firebase
    private static FirebaseApp app;
    private static DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    private static DataSnapshot snapshot = null;
    private static Firebase.Auth.FirebaseAuth auth;
    private static Firebase.Auth.FirebaseUser user;
    private static DatabaseReference reference;

    void Start() {
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

    void InitializeFirebase()
    {
        app = FirebaseApp.DefaultInstance;
        // NOTE: You'll need to replace this url with your Firebase App's database
        // path in order for the database connection to work correctly in editor.

        app.SetEditorDatabaseUrl("https://unreal2020-38e7b.firebaseio.com/");
        if (app.Options.DatabaseUrl != null) { app.SetEditorDatabaseUrl(app.Options.DatabaseUrl); }
        //auth.StateChanged += AuthStateChanged;
        //AuthStateChanged(this, null);

        Debug.Log("Initialization of firebase ended.(In SignIn Script.)");
    }

    #region update Method

    public void updateZoneA()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                for (int i = 1; i <= 5; i++)
                {
                    clueValue[i - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A").Child("Muzium").Child(MTNameInDatabase[0]).Child("Clues").Child("Clue " + i).GetValue(true).ToString());
                }
                mentalTaskValue[0] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child(MTNameInDatabase[0]).Child("isSolved").GetValue(true).ToString());
                for (int i = 1; i <= 5; i++)
                {
                    clueValue[i - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child(MTNameInDatabase[1]).Child("Clues").Child("Clue " + i).GetValue(true).ToString());
                }
                mentalTaskValue[1] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child(MTNameInDatabase[1]).Child("isSolved").GetValue(true).ToString());

                clueValue[5] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("ESCAPE(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[2] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("ESCAPE(E)").Child("isSolved").GetValue(true).ToString());

                clueValue[6] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("FILL(H)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[7] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("FILL(H)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[3] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("FILL(H)").Child("isSolved").GetValue(true).ToString());

                clueValue[8] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("WAZE(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[4] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("WAZE(E)").Child("isSolved").GetValue(true).ToString());

                clueValue[9] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("Attack(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[5] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("Attack(M)").Child("isSolved").GetValue(true).ToString());

                clueValue[10] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("MorseCode(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[11] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("MorseCode(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[6] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("MorseCode(E)").Child("isSolved").GetValue(true).ToString());

                clueValue[12] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("RGB(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[13] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("RGB(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[7] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("RGB(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateZoneB()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[14] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Lab(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[8] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Lab(M)").Child("isSolved").GetValue(true).ToString());

                clueValue[15] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Music(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[16] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Music(M)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[9] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Music(M)").Child("isSolved").GetValue(true).ToString());

                clueValue[17] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[18] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                clueValue[19] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 3").GetValue(true).ToString());
                clueValue[20] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 4").GetValue(true).ToString());
                mentalTaskValue[10] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("isSolved").GetValue(true).ToString());

                clueValue[21] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Flip(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[22] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Flip(M)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[11] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Flip(M)").Child("isSolved").GetValue(true).ToString());

                clueValue[23] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[24] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                clueValue[25] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 3").GetValue(true).ToString());
                clueValue[26] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 4").GetValue(true).ToString());
                mentalTaskValue[12] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateMuzium()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[0] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("BARCODE(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[1] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("BARCODE(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                clueValue[2] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("BARCODE(E)").Child("Clues").Child("Clue 3").GetValue(true).ToString());
                mentalTaskValue[0] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("BARCODE(E)").Child("isSolved").GetValue(true).ToString());

                clueValue[3] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("COUNT(H)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[4] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("COUNT(H)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[1] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("COUNT(H)").Child("isSolved").GetValue(true).ToString());

                clueValue[5] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("ESCAPE(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[2] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("ESCAPE(E)").Child("isSolved").GetValue(true).ToString());

                clueValue[6] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("FILL(H)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[7] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("FILL(H)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[3] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("FILL(H)").Child("isSolved").GetValue(true).ToString());

                clueValue[8] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("WAZE(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[4] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("WAZE(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateTTDI()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[9] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("Attack(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[5] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("Attack(M)").Child("isSolved").GetValue(true).ToString());

                clueValue[10] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("MorseCode(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[11] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("MorseCode(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[6] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("MorseCode(E)").Child("isSolved").GetValue(true).ToString());

                clueValue[12] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("RGB(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[13] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("RGB(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[7] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("RGB(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateSemantan()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;

                clueValue[17] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[18] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                clueValue[19] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 3").GetValue(true).ToString());
                clueValue[20] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 4").GetValue(true).ToString());
                mentalTaskValue[10] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("isSolved").GetValue(true).ToString());

                clueValue[21] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Flip(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[22] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Flip(M)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[11] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Flip(M)").Child("isSolved").GetValue(true).ToString());

                clueValue[23] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[24] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                clueValue[25] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 3").GetValue(true).ToString());
                clueValue[26] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 4").GetValue(true).ToString());
                mentalTaskValue[12] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updatePS()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[14] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Lab(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[8] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Lab(M)").Child("isSolved").GetValue(true).ToString());

                clueValue[15] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Music(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[16] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Music(M)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[9] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Music(M)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateBarcode()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[0] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("BARCODE(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[1] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("BARCODE(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                clueValue[2] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("BARCODE(E)").Child("Clues").Child("Clue 3").GetValue(true).ToString());
                mentalTaskValue[0] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("BARCODE(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateCount()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[3] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("COUNT(H)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[4] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("COUNT(H)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[1] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("COUNT(H)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateEscape()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[5] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("ESCAPE(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[2] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("ESCAPE(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateFill()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[6] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("FILL(H)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[7] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("FILL(H)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[3] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("FILL(H)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateWaze()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[8] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("WAZE(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[4] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child("WAZE(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateAttack()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[9] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("Attack(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[5] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("Attack(M)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateMorse()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[10] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("MorseCode(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[11] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("MorseCode(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[6] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("MorseCode(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateRGB()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[12] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("RGB(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[13] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("RGB(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[7] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child("RGB(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateLab()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[14] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Lab(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                mentalTaskValue[8] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Lab(M)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateMusic()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[15] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Music(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[16] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Music(M)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[9] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child("Music(M)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updateBraille()
    {
        clueValue[17] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
        clueValue[18] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
        clueValue[19] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 3").GetValue(true).ToString());
        clueValue[20] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("Clues").Child("Clue 4").GetValue(true).ToString());
        mentalTaskValue[10] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Braille(E)").Child("isSolved").GetValue(true).ToString());
    }

    public void updateFlip()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[21] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Flip(M)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[22] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Flip(M)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                mentalTaskValue[11] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Flip(M)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    public void updatePicture()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                clueValue[23] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 1").GetValue(true).ToString());
                clueValue[24] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 2").GetValue(true).ToString());
                clueValue[25] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 3").GetValue(true).ToString());
                clueValue[26] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("Clues").Child("Clue 4").GetValue(true).ToString());
                mentalTaskValue[12] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child("Picture(E)").Child("isSolved").GetValue(true).ToString());
            }
        }, taskSchedular);
    }

    #endregion

    //do this first for the whole life cycle of the app
    public static void PullAllData()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance.GetReference("Groups/").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                snapshot = task.Result;
                
                grpName = snapshot.Child(userID).Child("Name").GetValue(true).ToString();
                marks = System.Convert.ToDouble(snapshot.Child(userID).Child("marks").GetValue(true).ToString());
                isLoggedIn = System.Convert.ToBoolean(snapshot.Child(userID).Child("isLoggedIn").GetValue(true).ToString());
                success = System.Convert.ToBoolean(snapshot.Child(userID).Child("success").GetValue(true).ToString());

                MRTvalue[0] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child("isSolved").GetValue(true).ToString());
                MRTvalue[1] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child("isSolved").GetValue(true).ToString());
                MRTvalue[2] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child("isSolved").GetValue(true).ToString());
                MRTvalue[3] = System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Pasar Seni").Child("isSolved").GetValue(true).ToString());

                allCluesSolved[0] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[0]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[1] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[1]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[2] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[2]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[3] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[3]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[4] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[4]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[5] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[5]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[6] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[6]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[7] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[7]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[8] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[8]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[9] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[9]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[10] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[10]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[11] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").Child("isSolved").GetValue(true).ToString());
                allCluesSolved[12] =
                System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").Child("isSolved").GetValue(true).ToString());

                for (int i = 1; i <= 5; i++)
                {
                    //TTDI
                    clueValue[i - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[0]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                    clueValue[i + 5 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[1]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                    clueValue[i + 10 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[2]).Child("Clues").Child("clue" + i).GetValue(true).ToString());

                    //Muzium
                    clueValue[i + 15 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[3]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                    clueValue[i + 20 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[4]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                    clueValue[i + 25 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[5]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                    clueValue[i + 30 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[6]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                    clueValue[i + 35 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[7]).Child("Clues").Child("clue" + i).GetValue(true).ToString());

                    //Semantan
                    clueValue[i + 40 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[8]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                    clueValue[i + 45 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[9]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                    clueValue[i + 50 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[10]).Child("Clues").Child("clue" + i).GetValue(true).ToString());

                    //Pasar Seni
                    clueValue[i + 55 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                    clueValue[i + 60 - 1] =
                    System.Convert.ToBoolean(snapshot.Child(userID).Child("Zones/Zone B/Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").Child("clue" + i).GetValue(true).ToString());
                }
                //TTDI
                mentalTaskValue[0] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[0]).Child("isSolved").GetValue(true).ToString());
                mentalTaskValue[1] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[1]).Child("isSolved").GetValue(true).ToString());
                mentalTaskValue[2] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[2]).Child("isSolved").GetValue(true).ToString());
                //Muzium
                mentalTaskValue[3] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[3]).Child("isSolved").GetValue(true).ToString());
                mentalTaskValue[4] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[4]).Child("isSolved").GetValue(true).ToString());
                mentalTaskValue[7] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[7]).Child("isSolved").GetValue(true).ToString());
                //Semantan
                mentalTaskValue[8] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[8]).Child("isSolved").GetValue(true).ToString());
                mentalTaskValue[9] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[9]).Child("isSolved").GetValue(true).ToString());
                mentalTaskValue[10] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[10]).Child("isSolved").GetValue(true).ToString());
                //Pasar Seni
                mentalTaskValue[11] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Pasar Seni").Child(MTNameInDatabase[11]).Child("isSolved").GetValue(true).ToString());
                mentalTaskValue[12] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Pasar Seni").Child(MTNameInDatabase[12]).Child("isSolved").GetValue(true).ToString());
                //Do something with snapshot...
            }
        }, taskSchedular);
    }

    public static void SetAllTrue()
    {
        success = true;

        reference = FirebaseDatabase.DefaultInstance.RootReference;

        string[] MRT_1 = { "TTDI", "Muzium", "Semantan", "Pasar Seni" };
        string[] zone_1 = {"A", "A", "B", "B"};

        for(int i = 0; i < 4; i++)
        {
            MRTvalue[i] = true;
            set_MRTValue_online("Zone " + zone_1[i], MRT_1[i]);
        }

        string[] MRT_2 = { "TTDI", "TTDI", "TTDI", "Muzium", "Muzium", "Muzium", "Muzium", "Muzium", "Semantan", "Semantan", "Semantan", "Pasar Seni", "Pasar Seni" };
        string[] zone_2 = { "A", "A", "A", "A", "A", "A", "A", "A", "B", "B", "B", "B", "B" };

        for(int i = 0; i < 13; i++)
        {
            allCluesSolved[i] = true;
            set_allCluesSolved_online("Zone " + zone_2[i], MRT_2[i], (i + 1));
        }

        for (int i = 0; i < 5; i++)
        {
            //TTDI
            clueValue[i] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child(MTNameInDatabase[0]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
            clueValue[i + 5] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child(MTNameInDatabase[1]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
            clueValue[i + 10] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone A").Child("TTDI").Child(MTNameInDatabase[2]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);

            //Muzium
            clueValue[i + 15] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child(MTNameInDatabase[3]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
            clueValue[i + 20] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child(MTNameInDatabase[4]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
            clueValue[i + 25] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child(MTNameInDatabase[5]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
            clueValue[i + 30] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child(MTNameInDatabase[6]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
            clueValue[i + 35] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone A").Child("Muzium").Child(MTNameInDatabase[7]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);

            //Semantan
            clueValue[i + 40] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child(MTNameInDatabase[8]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
            clueValue[i + 45] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child(MTNameInDatabase[9]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
            clueValue[i + 50] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone B").Child("Semantan").Child(MTNameInDatabase[10]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);

            //Pasar Seni
            clueValue[i + 55] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[11]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
            clueValue[i + 60] = true;
            reference.Child("Groups").Child(userID).Child("Zones").Child("Zone B").Child("Pasar Seni").Child(MTNameInDatabase[12]).Child("Clues").Child("clue" + (i + 1)).SetValueAsync(true);
        }

        SaveSystem.SaveData();

        //for(int i = 0; i < 13; i++)
        //{
            
        //}
        ////TTDI
        //mentalTaskValue[0] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[0]).Child("isSolved").GetValue(true).ToString());
        //mentalTaskValue[1] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[1]).Child("isSolved").GetValue(true).ToString());
        //mentalTaskValue[2] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/TTDI").Child(MTNameInDatabase[2]).Child("isSolved").GetValue(true).ToString());
        ////Muzium
        //mentalTaskValue[3] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[3]).Child("isSolved").GetValue(true).ToString());
        //mentalTaskValue[4] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[4]).Child("isSolved").GetValue(true).ToString());
        //mentalTaskValue[7] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone A/Muzium").Child(MTNameInDatabase[7]).Child("isSolved").GetValue(true).ToString());
        ////Semantan
        //mentalTaskValue[8] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[8]).Child("isSolved").GetValue(true).ToString());
        //mentalTaskValue[9] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[9]).Child("isSolved").GetValue(true).ToString());
        //mentalTaskValue[10] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Semantan").Child(MTNameInDatabase[10]).Child("isSolved").GetValue(true).ToString());
        ////Pasar Seni
        //mentalTaskValue[11] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Pasar Seni").Child(MTNameInDatabase[11]).Child("isSolved").GetValue(true).ToString());
        //mentalTaskValue[12] = System.Convert.ToInt32(snapshot.Child(userID).Child("Zones/Zone B/Pasar Seni").Child(MTNameInDatabase[12]).Child("isSolved").GetValue(true).ToString());
    }

    //calculate Marks

    public const int HARD = 30;
    public const int MEDIUM = 20;
    public const int EASY = 10;

    public const int ATTEMP_FAIL = 2;
    public const int ATTEMP_SUCCESS = 3;

    public const int BASE_MARK = 10;

    //call this when display the total mark
    public static void calClueMark(int clue_index)
    {
        string zone;
        string MRT;
        int MT;
        if (clue_index < 41)
        {
            zone = "Zone A";
            if (clue_index < 16)
            {
                MRT = "TTDI";
                if (clue_index < 3)
                    MT = 1;
                else if (clue_index < 7)
                    MT = 2;
                else
                    MT = 3;
            }
            else
            {
                MRT = "Muzium";
                if (clue_index < 17)
                    MT = 4;
                else if (clue_index < 23)
                    MT = 5;
                else if (clue_index < 27)
                    MT = 6;
                else if (clue_index < 33)
                    MT = 7;
                else
                    MT = 8;
            }
        }
        else
        {
            zone = "Zone B";
            if (clue_index < 56)
            {
                MRT = "Semantan";
                if (clue_index < 43)
                    MT = 9;
                else if (clue_index < 50)
                    MT = 10;
                else
                    MT = 11;
            }
            else
            {
                MRT = "Pasar Seni";
                if (clue_index < 58)
                    MT = 12;
                else
                    MT = 13;
            }
        }
        clueValue[clue_index - 1] = true;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Groups").Child(userID).Child("Zones").Child(zone).Child(MRT).Child(MTNameInDatabase[MT - 1]).Child("Clues").Child("clue" + (((clue_index - 1) % 5) + 1)).SetValueAsync(true);
        marks += BASE_MARK;
        reference.Child("Groups").Child(userID).Child("marks").SetValueAsync(marks.ToString());
        Debug.Log("UserID = " + userID + ", " + zone + ", " + MRT + ", " + MT + ", clue" + (((clue_index - 1) % 5) + 1) + ", Clue Index = " + clue_index);
        Debug.Log(clue_index + " clue = " + database.clueValue[clue_index - 1]);
        Debug.Log("Whole clues = " + database.clueValue);
        if(clueValue[clue_index - 1])
        {
            SaveSystem.SaveData();
        }
    }

    public static void calMTMarks_Success(int MT)
    {
        string zone;
        string MRT;
        if (MT < 9)
        {
            zone = "Zone A";
            if (MT < 4)
                MRT = "TTDI";
            else
                MRT = "Muzium";
        }
        else
        {
            zone = "Zone B";
            if (MT < 12)
                MRT = "Semantan";
            else
                MRT = "Pasar Seni";
        }
        mentalTaskValue[MT - 1] = ATTEMP_SUCCESS;

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Zones").Child(zone).Child(MRT).Child(MTNameInDatabase[MT - 1]).Child("isSolved").SetValueAsync(ATTEMP_SUCCESS);

        int temp = 0;
        if (mentalTaskDifficulty[MT - 1] == 'E') { temp = EASY; }
        else if (mentalTaskDifficulty[MT - 1] == 'M') { temp = MEDIUM; }
        else { temp = HARD; }

        marks += temp;
    }

    public static void calMTMarks_Fail(int MT)
    {
        string zone;
        string MRT;
        if (MT < 9)
        {
            zone = "Zone A";
            if (MT < 4)
                MRT = "TTDI";
            else
                MRT = "Muzium";
        }
        else
        {
            zone = "Zone B";
            if (MT < 12)
                MRT = "Semantan";
            else
                MRT = "Pasar Seni";
        }
        mentalTaskValue[MT - 1] = ATTEMP_FAIL;

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Zones").Child(zone).Child(MRT).Child(MTNameInDatabase[MT - 1]).Child("isSolved").SetValueAsync(ATTEMP_FAIL);

        int temp = 0;
        if (mentalTaskDifficulty[MT - 1] == 'E') { temp = EASY; }
        else if (mentalTaskDifficulty[MT - 1] == 'M') { temp = MEDIUM; }
        else { temp = HARD; }

        marks += (temp / 2);
    }

    public static void set_LoggedIn_true_onine()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Groups").Child(userID).Child("isLoggedIn").SetValueAsync(true);
        Debug.Log("Done Upload");
    }

    public static void set_LoggedIn_false_online()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Groups").Child(userID).Child("isLoggedIn").SetValueAsync(false);
        Debug.Log("Done Upload");
    }

    public static void set_allCluesSolved_online(string zone, string MRT, int index)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Groups").Child(userID).Child("Zones").Child(zone).Child(MRT).Child(MTNameInDatabase[index - 1]).Child("Clues").Child("isSolved").SetValueAsync(true);
        Debug.Log("Done Upload");
    }

    public static void set_MRTValue_online(string zone, string MRT)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Groups").Child(userID).Child("Zones").Child(zone).Child(MRT).Child("isSolved").SetValueAsync(true);
        Debug.Log("Done Upload");
    }

    public static void CorrectKiller()
    {
        marks += 30;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Groups").Child(userID).Child("marks").SetValueAsync(marks.ToString());
        reference.Child("Groups").Child(userID).Child("success").SetValueAsync(true);
        Debug.Log("Done Upload");
    }

    public static void WrongKiller()
    {
        marks -= 5;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        reference.Child("Groups").Child(userID).Child("marks").SetValueAsync(marks.ToString());
    }

    public static void calMark()
    {
        double mark = 0;
        //clue
        for (int i = 0; i < clueValue.Count; i++)
        {
            if (clueValue[i] == true){mark += BASE_MARK;}
        }
        //mental task
        for (int i = 0; i < mentalTaskValue.Count; i++)
        {
            int temp = 0;
            if (mentalTaskDifficulty[i] == 'E'){temp = EASY;}
            else if (mentalTaskDifficulty[i] == 'M'){temp = MEDIUM;}
            else{temp = HARD;}

            if (mentalTaskValue[i] == ATTEMP_FAIL){mark += temp / 2;}
            else if (mentalTaskValue[i] == ATTEMP_SUCCESS){mark += temp;}
            else{temp = 0;}
        }
        //DatabaseHandler1.setMarks(mark);
    }
}