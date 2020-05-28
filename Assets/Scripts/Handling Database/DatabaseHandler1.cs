using Firebase.Database;
//using Mapbox.Unity.Location;
//using Mapbox.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DatabaseHandler1 : MonoBehaviour
{
    static DatabaseReference reference;
    static DatabaseReference grouprRef;
    static Firebase.Auth.FirebaseAuth auth;
    static Firebase.Auth.FirebaseUser user;
    static bool multiplierUsed = false;
    //all these are constants
    private static Dictionary<string, int> muzium = new Dictionary<string, int>();
    private static Dictionary<string, int> ttdi = new Dictionary<string, int>();
    private static Dictionary<string, int> pasarSeni = new Dictionary<string, int>();
    private static Dictionary<string, int> semantan = new Dictionary<string, int>();
    private static Dictionary<string, Dictionary<string, int>> zoneA = new Dictionary<string, Dictionary<string, int>>();
    private static Dictionary<string, Dictionary<string, int>> zoneB = new Dictionary<string, Dictionary<string, int>>();
    private static string endTime = "";

    static double multiplier;
    //public static Location currLoc;

    // Start is called before the first frame update

    //[SerializeField]
    //Text _lati;
    //[SerializeField]
    //Text _longi;

    //public static AbstractLocationProvider _locationProvider = null;

    void Start()
    {
        //if (null == _locationProvider)
        //{
        //    _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        //}
        ////categories them into diff zone
        zoneA.Add("TTDI", ttdi);
        zoneA.Add("Muzium", muzium);
        zoneB.Add("Semantan", semantan);
        zoneB.Add("Pasar Seni", pasarSeni);
        //add key value into each place
        ttdi.Add("Attack(M)", 20);
        ttdi.Add("MorseCode(E)", 10);
        ttdi.Add("RGB(E)", 10);
        muzium.Add("Barcode(E)", 10);
        muzium.Add("Count(H)", 30);
        muzium.Add("Escape(E)", 10);
        muzium.Add("Fill(H)", 30);
        muzium.Add("Waze(E)", 10);
        semantan.Add("Braille(E)", 10);
        semantan.Add("Flip(M)", 20);
        semantan.Add("Picture(M)", 30);
        pasarSeni.Add("Lab(M)", 20);
        pasarSeni.Add("Music(M)", 20);
    }
    //completion can be 2 or 3, then decide the user get half or max marks from a MT
    public void calcMark(int completion, int maxMark)
    {
        if (completion == 2)
        {
            int temp = maxMark / 2;
            addMarks(temp);
        }
        else if (completion == 3)
        {
            addMarks(maxMark);
        }

    }
    //call this when the endGame button is clicked
    public async void setEndTimeAsync()
    {

        reference = FirebaseDatabase.DefaultInstance.RootReference;
        grouprRef = reference.Child("Groups").Child(user.UserId);
        //need chee king's time method
        StartCoroutine(GetInternetTime());
        await Task.Delay(1000);
        //yield return new WaitForSeconds(5);
        await reference.Child("Groups").Child(user.UserId).Child("EndTime").SetValueAsync(endTime);
        //local
    }
    ////dont call this as well
    public IEnumerator GetInternetTime()
    {
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("http://www.google.com");
        yield return myHttpWebRequest.Send();

        string netTime = myHttpWebRequest.GetResponseHeader("date");
        Debug.Log(netTime + " was response");
        int startIn = netTime.IndexOf("2020") + 5;
        Debug.Log(startIn);
        endTime = (netTime.Substring(startIn, 8));
    }
    //public void setLocationXandY()
    //{
    //    Debug.Log("Enter set location");
    //    double x = currLoc.LatitudeLongitude.x;
    //    double y = currLoc.LatitudeLongitude.y;
    //    Debug.Log(x);
    //    Debug.Log(y);
    //    //_lati.text = x.ToString();
    //    //_longi.text = y.ToString();
    //    //grouprRef.Child("Location").Child("x").SetValueAsync(x);
    //    //grouprRef.Child("Location").Child("y").SetValueAsync(y);
    //    //local
    //}

    public void setMultiplier(double mul)
    {
        //the name of attr in database in wrong, typo
        grouprRef.Child("Multipier").SetValueAsync(mul);
        //local
        //database.set_multiplier(mul);
    }
    void Update()
    {
        //currLoc = _locationProvider.CurrentLocation;

        //if (currLoc.IsLocationServiceInitializing) {
        //    Debug.Log("location initialising.");
        //}
        //else {
        //    if (!currLoc.IsLocationServiceEnabled) {
        //        _locationProvider = LocationProviderFactory.Instance.DefaultLocationProvider as AbstractLocationProvider;
        //        Debug.Log("location service not enabled.");
        //    }
        //    else {
        //        if (currLoc.LatitudeLongitude.Equals(Vector2d.zero)) {
        //            Debug.Log("Waiting for location.");
        //        }
        //        else {

        //            setLocationXandY();
        //        }
        //    }
        //}

    }
    //these is for local
    //public void setClueValue(bool value,int i) { database.clueValue[i] = value; }
    public void setClueValue(string path, bool value, int index)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        grouprRef = reference.Child("Groups").Child(user.UserId);
        //set true or false
        grouprRef.Child(path).SetValueAsync(value);
        //local
        database.clueValue[index] = value;
    }
    //these is for local
    //0:Not Opened 1:Opened 2:Failed 3:Passed
    public void setMTValue(int state, int i) {
        if (state < 4 && state > -1) { 
            database.mentalTaskValue[i] = state;
        }
        else
        {
            Debug.Log("Wrong state input for MT");
        }
    }
    //use these 2 method carefully, they start from root
    public void setAny(string path, string value)
    {
        //exp of path:"Location/x"
        reference.Child(path).SetValueAsync(value);
    }
    public string getAny(string path)
    {
        //how to get userId from outside?
        DataSnapshot Any;
        string any = "";
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        //here is very important!
        FirebaseDatabase.DefaultInstance
        .GetReference(path).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Any = task.Result;
                Debug.Log("Update become true.");
                any = Any.GetValue(true).ToString();
                Debug.Log("We get " + any);
                //Do something with snapshot...
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("task.IsCanceled:" + task.Exception);
            }
        }, taskSchedular);
        return any;
    }
    //useless method, maybe used to display group name
    public string getGroupName()
    {
        return database.get_grpName();
    }
    //don't call this
    public double getMultiplierFromRank(int rank)
    {
        if (rank >= 1 && rank <= 3)
        {
            return 3;
        }
        else if (rank >= 4 && rank <= 8)
        {
            return 2.5;
        }
        else if (rank >= 9 && rank <= 18)
        {
            return 2;
        }
        else if (rank >= 19 && rank <= 33)
        {
            return 1.5;
        }
        else
        {
            return 1;
        }
    }
    //call this when user input the correct answer for the first MT
    public void correctForFirstMT()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        grouprRef = reference.Child("Groups").Child(user.UserId);
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;
        DataSnapshot currentRank;
        int rank = -10;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        FirebaseDatabase.DefaultInstance
        .GetReference("Groups/" + user.UserId + "/marks").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                currentRank = task.Result;
                Debug.Log("Update become true.");
                string markx = currentRank.GetValue(true).ToString();
                Debug.Log("We get " + markx);
                rank = System.Convert.ToInt32(markx);
                //Do something with snapshot...
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("task.IsCanceled:" + task.Exception);
            }
        }, taskSchedular);
        rank++;
        reference.Child("Groups").Child("Out").SetValueAsync(rank);
        //database.set_multiplier(getMultiplierFromRank(rank));
    }
    //dont call this
    public static void setMarks(double markToSet)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        grouprRef = reference.Child("Groups").Child(user.UserId);
        grouprRef.Child("marks").SetValueAsync(markToSet);
        database.set_marks(markToSet);
    }
    //for MTs, every time user input the correct value for each MT, should call this method
    public static void addMarks(double markToAdd)
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        grouprRef = reference.Child("Groups").Child(user.UserId);
        //get current mark
        DataSnapshot currentMark;
        double mark = -999999;
        var taskSchedular = TaskScheduler.FromCurrentSynchronizationContext();
        //here is very important!
        FirebaseDatabase.DefaultInstance
        .GetReference("Groups/" + user.UserId + "/marks").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                //Handle the error...
                Debug.LogError("task.IsFaulted:" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                currentMark = task.Result;
                Debug.Log("Update become true.");
                string markx = currentMark.GetValue(true).ToString();
                Debug.Log("We get " + markx);
                mark = System.Convert.ToDouble(markx);
                //Do something with snapshot...
            }
            else if (task.IsCanceled)
            {
                Debug.LogError("task.IsCanceled:" + task.Exception);
            }
        }, taskSchedular);
        double total = mark;
        if (!multiplierUsed)
        {
            //multiplier = database.get_multiplier();//get multiplier from object
            //markToAdd = markToAdd * multiplier;
            multiplierUsed = true;
        }
        total += markToAdd;
        grouprRef.Child("marks").SetValueAsync(total);
        //local
        database.set_marks(total);
    }

    //call while user click "collect"
    public void setMentalTaskIsSolved(string path)
    {
        //set true or false

        //local
    }
}
