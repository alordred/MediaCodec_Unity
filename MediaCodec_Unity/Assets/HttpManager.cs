using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class HttpManager : MonoBehaviour {
    private string urlServerFirst = "http://10.11.12.7:81";
    private string vikingStr = "/viking";
    private string urlServerEnd = ".mp4";
    private int currenrNum;
    public GameController gc;
    //public string urlPath = "http://47.92.83.234/viking1.mp4";//资源网络路径(自己写)
															  //资源保路径
	FileInfo file;
	void Awake()
	{
        if(gc.IsInternetMode)
        {
			currenrNum = 1;
			DownLoadViking();
        }
	}

    public void DownLoadViking()
    {
		file = new FileInfo(Application.persistentDataPath + vikingStr + currenrNum.ToString() + urlServerEnd);
		StartCoroutine(DownFile(urlServerFirst + vikingStr + currenrNum.ToString() + urlServerEnd));
        currenrNum++;
		if (currenrNum == 4)
		{
            currenrNum = 1;
		}
    }

	/// <summary>
	/// 下载文件
	/// </summary>
	IEnumerator DownFile(string url)
	{
		WWW www = new WWW(url);
		yield return www;
		if (www.isDone)
		{
			Debug.Log("下载完成");
			byte[] bytes = www.bytes;
			CreatFile(bytes);
		}
	}

	/// <summary>
	/// 创建文件
	/// </summary>
	/// <param name="bytes"></param>
	void CreatFile(byte[] bytes)
	{
		Stream stream;
		stream = file.Create();
		stream.Write(bytes, 0, bytes.Length);
		stream.Close();
		stream.Dispose();
	}
}
