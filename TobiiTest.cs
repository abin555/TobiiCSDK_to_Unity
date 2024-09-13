using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TobiiTest : MonoBehaviour
{
    TobiiResearch.TobiiResearchEyeTrackers eyeTrackers;
    TobiiResearch.EyeTracker eyeTracker;
    public GameObject leftEye;
    public GameObject rightEye;

    public float left_gazeX;
    public float left_gazeY;

    public float right_gazeX;
    public float right_gazeY;
    // Start is called before the first frame update
    void Start()
    {
        eyeTrackers = new TobiiResearch.TobiiResearchEyeTrackers {};
        _ = TobiiResearch.FindEyeTrackers(ref eyeTrackers);
        print(eyeTrackers.count);

        if(eyeTrackers.count.ToUInt64() == 0) return;

        eyeTracker = TobiiResearch.GetEyeTracker(ref eyeTrackers, 0);
        string name = TobiiResearch.GetDeviceName(ref eyeTracker);
        print(name);
        //TobiiResearch.FreeEyeTrackers(ref eyeTrackers);
        TobiiResearch.SubscribeGaze(ref eyeTracker, TobiiResearch.GazeCallback);
        print("Subscribed?\n");
    }

    // Update is called once per frame
    void Update()
    {
        left_gazeX = (TobiiResearch.leftEye.x - 0.5f) * 2;
        left_gazeY = (TobiiResearch.leftEye.y - 0.5f) * 2;
        if(!(float.IsNaN(left_gazeX) || float.IsNaN(left_gazeY))){
            leftEye.transform.position = new Vector3(left_gazeX, 0, -1 * left_gazeY);
        }
        right_gazeX = (TobiiResearch.rightEye.x - 0.5f) * 2;
        right_gazeY = (TobiiResearch.rightEye.y - 0.5f) * 2;
        if(!(float.IsNaN(right_gazeX) || float.IsNaN(right_gazeY))){
            rightEye.transform.position = new Vector3(right_gazeX, 0, -1 * right_gazeY);
        }
    }

    void OnApplicationQuit(){
        TobiiResearch.UnsubscribeGaze(ref eyeTracker, TobiiResearch.GazeCallback);
        print("Unsubscribe");
        TobiiResearch.FreeEyeTrackers(ref eyeTrackers);
        print("Free");
    }
}
