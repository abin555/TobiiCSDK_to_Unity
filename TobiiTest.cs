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
        float left_gazeX = (TobiiResearch.latestGazeData.leftEye.gazePoint.displayPos.x - 0.5f) * 2;
        float left_gazeY = (TobiiResearch.latestGazeData.leftEye.gazePoint.displayPos.y - 0.5f) * 2;
        if(!(float.IsNaN(left_gazeX) || float.IsNaN(left_gazeY))){
            leftEye.transform.position = new Vector3(left_gazeX, -1 * left_gazeY, 0);
        }
        float right_gazeX = (TobiiResearch.latestGazeData.rightEye.gazePoint.displayPos.x - 0.5f) * 2;
        float right_gazeY = (TobiiResearch.latestGazeData.rightEye.gazePoint.displayPos.y - 0.5f) * 2;
        if(!(float.IsNaN(right_gazeX) || float.IsNaN(right_gazeY))){
            rightEye.transform.position = new Vector3(right_gazeX, -1 * right_gazeY, 0);
        }
    }

    void OnApplicationQuit(){
        TobiiResearch.UnsubscribeGaze(ref eyeTracker, TobiiResearch.GazeCallback);
        print("Unsubscribe");
        TobiiResearch.FreeEyeTrackers(ref eyeTrackers);
        print("Free");
    }
}
