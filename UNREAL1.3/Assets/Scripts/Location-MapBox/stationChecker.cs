//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Mapbox.Unity.Location;
//using UnityEngine;
//using UnityEngine.UI;

//public class stationChecker : MonoBehaviour {

//    private Location currloc;
//    private double dist;

//    public IDictionary<int, double> latitude = new Dictionary<int, double>()
//        {
//            {1, 3.129903f},
//            {2, 3.133044f},
//            {3, 3.137386f},
//            {4, 3.135707f},
//            {5, 3.139557f},
//            {6, 3.139936f},
//            {7, 3.137867f},
//            {8, 3.137878f},
//            {9, 3.139502f},
//            {10, 3.139459f},

//            {11, 3.151437f},
//            {12, 3.152205f},
//            {13, 3.153686f},
//            {14, 3.149354f},
//            {15, 3.147747f},
//            {16, 3.13608f},
//            {17, 3.137345f},
//            {18, 3.151037f},
//            {19, 3.14238f}
//        };
//    public IDictionary<int, double> longtitude = new Dictionary<int, double>()
//        {
//            {1, 101.649363f},
//            {2, 101.626859f},
//            {3, 101.631036f},
//            {4, 101.628286f},
//            {5, 101.687343f},
//            {6, 101.687889f},
//            {7, 101.686813f},
//            {8, 101.688123f},
//            {9, 101.68921f},
//            {10, 101.689494f},

//            {11, 101.66661f},
//            {12, 101.664182f},
//            {13, 101.666245f},
//            {14, 101.69373f},
//            {15, 101.695238f},
//            {16, 101.630874f},
//            {17, 101.687463f},
//            {18, 101.665308f},
//            {19, 101.695854f}
//        };

//    public IDictionary<int, string> locationName = new Dictionary<int, string>()
//        {
//            {1, "KK8"},
//            {2, "SS20/5 Playground (Park) "},
//            {3, "TTDI Plaza"},
//            {4, "Damansara Kim Basketball Court"},
//            {5, "Planetarium: white pondok at parking lot"},
//            {6, "Plantetarium: pondok to the left of Agensi Angkasa Negara entrance"},
//            {7, "Muzium Negara : Stage"},
//            {8, "Muzium Negara: Pondok next to sheltered walkway at traditional house area"},
//            {9, "Planetarium: area outside entrance underneath blue stairs"},
//            {10, "Planetarium: indoor exhibition"},

//            {11, "Overhead Bridge"},
//            {12, "Private Parking Lot"},
//            {13, "Bangunan SKM"},
//            {14, "Dataran Merdeka Plaza"},
//            {15, "River of Life: Viewing Point"},
//            {16, "TTDI MRT Station"},
//            {17, "Muzium Negara MRT Station"},
//            {18, "Semantan MRT Station"},
//            {19, "Pasar Seni MRT Station"}
//        };

//    public IDictionary<int, bool> locationCheck = new Dictionary<int, bool>()
//    {
//            {1, false},
//            {2, false},
//            {3, false},
//            {4, false},
//            {5, false},
//            {6, false},
//            {7, false},
//            {8, false},
//            {9, false},
//            {10, false},

//            {11, false},
//            {12, false},
//            {13, false},
//            {14, false},
//            {15, false},
//            {16, false},
//            {17, false},
//            {18, false},
//            {19, false}
//        };

//    [SerializeField]
//    Text _distanceText;
//    [SerializeField]
//    Text _areaText;

//    private void Awake() {
//    }

//    // Start is called before the first frame update
//    void Start() {
//    }

//    // Update is called once per frame
//    void Update() {
//        double min = Double.MaxValue;
//        bool nearest = false;
//        string nearLocationName = null;
//        string distText = null;
//        currloc = Mapbox.Examples.LocationStatus._locationProvider.CurrentLocation;

//        for (int i = 1; i <= 15; i++) {

//            dist = calDist(toRad(latitude[i]), toRad(longtitude[i]));

//            if (dist < 0.1f && dist < min) {
//                nearest = true;
//                min = dist;
//                nearLocationName = locationName[i];
//                locationCheck[i] = true;
//                distText = min.ToString();

//            }
//            else if (dist < min && dist < 0.20f) {
//                min = dist;
//                locationCheck[i] = false;
//            }
//            else {
//                locationCheck[i] = false;
//            }

//        }

//        if (nearest) {
//            _areaText.text = nearLocationName;
//            _distanceText.text = distText;
//        }
//        else {
//            _areaText.text = "-";
//            _distanceText.text = min.ToString();
//        }


//    }

//    public static double toRad(double degrees) {
//        double radians = (Math.PI / 180) * degrees;
//        return (radians);
//    }

//    double calDist(double latitude, double longitude) {
//        return 6371 * Math.Acos(
//            Math.Sin(toRad(currloc.LatitudeLongitude.x)) * Math.Sin(latitude)
//            + Math.Cos(toRad(currloc.LatitudeLongitude.x)) * Math.Cos(latitude) * Math.Cos(longitude - toRad(currloc.LatitudeLongitude.y)));
//    }
//}
