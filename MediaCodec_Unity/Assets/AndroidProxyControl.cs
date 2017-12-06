using UnityEngine;
using System.Collections;

public class AndroidProxyControl : MonoBehaviour {

    private AndroidJavaClass jc;
    private AndroidJavaObject jo;

	// Use this for initialization
	void Start () {
        jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
	
	}
	
	// Update is called once per frame
	void Update () {
        if (GvrController.AppButtonDown)
        {
            //long downloadId = jc.Call<long>("downloadFile", new object[] { "http://192.168.0.116/video/sunlh_png.mp4", "sunlh_unity.mp4" });
            long testNum = jo.Call<long>("testMethod");
            Debug.Log("The test number is " + testNum);
        }

    }
}
