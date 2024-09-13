using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TobiiTest : MonoBehaviour
{
    TobiiResearch Tobii = new TobiiResearch();
    // Start is called before the first frame update
    void Start()
    {
        TobiiResearch.TobiiResearchEyeTrackers eyeTrackers = new TobiiResearch.TobiiResearchEyeTrackers {};
        _ = TobiiResearch.FindEyeTrackers(ref eyeTrackers);
        print(eyeTrackers.count);

        if(eyeTrackers.count.ToUInt64() == 0) return;

        TobiiResearch.EyeTracker eyeTracker = TobiiResearch.GetEyeTracker(ref eyeTrackers, 0);
        string name = TobiiResearch.GetDeviceName(ref eyeTracker);
        print(name);
        //TobiiResearch.FreeEyeTrackers(ref eyeTrackers);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
