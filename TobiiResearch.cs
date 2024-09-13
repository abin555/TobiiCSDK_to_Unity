using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using Tobii.Research;
using Unity.VisualScripting;

public class TobiiResearch {

	[StructLayoutAttribute(LayoutKind.Sequential)]
	public struct TobiiResearchEyeTrackers{
		public System.IntPtr eyetrackers;
		public System.UIntPtr count;
	}

	public struct EyeTracker {
		public System.IntPtr tracker;
	};

	[DllImport ("libtobii_research", EntryPoint="tobii_research_find_all_eyetrackers")]
	private static extern int tobii_research_find_all_eyetrackers(ref System.IntPtr eyetrackers);

	[DllImport ("libtobii_research", EntryPoint="tobii_research_free_eyetrackers")]
	private static extern int tobii_research_free_eyetrackers(System.IntPtr eyetrackers);

	[DllImport ("libtobii_research", EntryPoint="tobii_research_get_device_name")]
	private static extern int tobii_research_get_device_name(System.IntPtr eyetracker, ref System.IntPtr device_name);

	public static int FindEyeTrackers(ref TobiiResearchEyeTrackers eyeTrackers){
		System.IntPtr ptr = System.IntPtr.Zero;

		int status = tobii_research_find_all_eyetrackers(ref ptr);

		eyeTrackers = Marshal.PtrToStructure<TobiiResearchEyeTrackers>(ptr);

		return status;
	}

	public static EyeTracker GetEyeTracker(ref TobiiResearchEyeTrackers eyeTrackers, int index){
		if(eyeTrackers.eyetrackers == System.IntPtr.Zero){
			throw new ArgumentNullException(nameof(eyeTrackers.eyetrackers));
		}
		if(index < 0 || (uint) index >= eyeTrackers.count.ToUInt32()){
			throw new ArgumentOutOfRangeException(nameof(index));
		}

		IntPtr ptr = Marshal.ReadIntPtr(eyeTrackers.eyetrackers, index * IntPtr.Size);

		EyeTracker tracker = new EyeTracker {};
		tracker.tracker = ptr;
		return tracker;
	}

	public static void FreeEyeTrackers(ref TobiiResearchEyeTrackers eyeTrackers){
		if(eyeTrackers.eyetrackers == System.IntPtr.Zero) return;
		System.IntPtr ptr = System.IntPtr.Zero;
		Marshal.StructureToPtr(eyeTrackers, ptr, false);
		tobii_research_free_eyetrackers(ptr);		
	}

	public static string GetDeviceName(ref EyeTracker eyeTracker){
		System.IntPtr ptr = System.IntPtr.Zero;
		tobii_research_get_device_name(eyeTracker.tracker, ref ptr);
		string devName = Marshal.PtrToStringAnsi(ptr);
		return devName;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vec2{
		public float x;
		public float y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct Vec3{
		public float x;
		public float y;
		public float z;
	}
	
	
	[StructLayout(LayoutKind.Sequential)]
	public struct GazePoint{
		public Vec2 displayPos;
		public Vec3 userCoord;
		public int validity;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct PupilData{
		public float diameter;
		public int validity;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct GazeOrigin{
		public Vec3 userPos;
		//Depricated
		public Vec3 boxPos;

		public int validity;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct EyeData{
		public GazePoint gazePoint;
		public PupilData pupilData;
		public GazeOrigin gazeOrigin;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct GazeData{
		public EyeData leftEye;
		public EyeData rightEye;
		long device_time_stamp;
		long system_time_stamp;
	}
	public static GazeData latestGazeData;
	public static Vec2 leftEye;
	public static Vec2 rightEye;

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void GazeCallbackType(System.IntPtr gaze_data, System.IntPtr user_data);

	[DllImport("libtobii_research", CallingConvention = CallingConvention.Cdecl)]
	public static extern int tobii_research_subscribe_to_gaze_data(System.IntPtr eyeTracker, GazeCallbackType callback, System.IntPtr user_data);
	[DllImport("libtobii_research", CallingConvention = CallingConvention.Cdecl)]
	public static extern int tobii_research_unsubscribe_from_gaze_data(System.IntPtr eyeTracker, GazeCallbackType callback);
	
	public static void GazeCallback(System.IntPtr gaze_data, System.IntPtr user_data){
		GazeData gazeData = Marshal.PtrToStructure<GazeData>(gaze_data);
		latestGazeData = gazeData;
		leftEye.x = gazeData.leftEye.gazePoint.displayPos.x;
		leftEye.y = gazeData.leftEye.gazePoint.displayPos.y;

		rightEye.x = gazeData.rightEye.gazePoint.displayPos.x;
		rightEye.y = gazeData.rightEye.gazePoint.displayPos.y;
	}

	public static void SubscribeGaze(ref EyeTracker eyeTracker, GazeCallbackType callback){
		IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
		tobii_research_subscribe_to_gaze_data(eyeTracker.tracker, callback, IntPtr.Zero);
	}

	public static void UnsubscribeGaze(ref EyeTracker eyeTracker, GazeCallbackType callback){
		IntPtr callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
		tobii_research_unsubscribe_from_gaze_data(eyeTracker.tracker, callback);
	}
}