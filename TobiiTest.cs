using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TobiiTest : MonoBehaviour
{
    TobiiResearch.TobiiResearchEyeTrackers eyeTrackers;
    TobiiResearch.EyeTracker eyeTracker;
    public GameObject testObject;
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
        float gazeX = TobiiResearch.latestGazeData.leftEye.gazePoint.displayPos.x;
        float gazeY = TobiiResearch.latestGazeData.leftEye.gazePoint.displayPos.y;
        if(float.IsNaN(gazeX) || float.IsNaN(gazeY)) return;
        testObject.transform.position = new Vector3(gazeX, -1 * gazeY, 0);
    }

    void OnApplicationQuit(){
        TobiiResearch.UnsubscribeGaze(ref eyeTracker, TobiiResearch.GazeCallback);
        print("Unsubscribe");
        TobiiResearch.FreeEyeTrackers(ref eyeTrackers);
        print("Free");
    }
}
