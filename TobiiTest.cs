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

    public Camera camera;

    public float left_gazeX;
    public float left_gazeY;

    public float right_gazeX;
    public float right_gazeY;

    public Vector3 worldPoint;
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
        left_gazeX = (TobiiResearch.leftEye.x);
        left_gazeY = (TobiiResearch.leftEye.y);
        right_gazeX = (TobiiResearch.rightEye.x);
        right_gazeY = (TobiiResearch.rightEye.y);

        int numValid = 0;
        if(!float.IsNaN(left_gazeX)) numValid++;
        if(!float.IsNaN(right_gazeX)) numValid++;

        float avg_x = ((float.IsNaN(left_gazeX) ? 0 : left_gazeX) + (float.IsNaN(right_gazeX) ? 0 : right_gazeX)) / numValid;
        float avg_y = ((float.IsNaN(left_gazeX) ? 0 : left_gazeY) + (float.IsNaN(right_gazeX) ? 0 : right_gazeY)) / numValid;

        int ScreenW = Screen.width;
        int ScreenH = Screen.height;

        int ScreenX = (int)(ScreenW * avg_x);
        int ScreenY = (int)(ScreenH - ScreenH * avg_y);

        Vector3 screenPoint;
        screenPoint.x = ScreenX;
        screenPoint.y = ScreenY;
        screenPoint.z = 1;

        worldPoint = camera.ScreenToWorldPoint(screenPoint, camera.stereoActiveEye);

        leftEye.transform.position = new Vector3(worldPoint.x, worldPoint.y, worldPoint.z);
    }

    void OnApplicationQuit(){
        TobiiResearch.UnsubscribeGaze(ref eyeTracker, TobiiResearch.GazeCallback);
        print("Unsubscribe");
        TobiiResearch.FreeEyeTrackers(ref eyeTrackers);
        print("Free");
    }
}
