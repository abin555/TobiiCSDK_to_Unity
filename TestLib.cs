using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TestLib : MonoBehaviour
{
    [StructLayoutAttribute(LayoutKind.Sequential)]
    public struct TestStruct {
        public int a;
        public int b;
        public float c;
    }

    [DllImport("libtest.so", EntryPoint="testStructFunc", CallingConvention=CallingConvention.Cdecl)]
    public static extern TestStruct testStructFunc(int a, int b, float c);

    [DllImport ("libtest.so", CallingConvention=CallingConvention.Cdecl)]
    private static extern int testFunc();

    // Start is called before the first frame update
    void Start()
    {
        print(testFunc());
        TestStruct test = testStructFunc(4,5,((float) 42.4f));
        print(test.a);
        print(test.b);
        print(test.c);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
