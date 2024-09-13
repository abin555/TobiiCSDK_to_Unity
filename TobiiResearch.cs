using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System;

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
	public struct GazeData{
		
	}
}