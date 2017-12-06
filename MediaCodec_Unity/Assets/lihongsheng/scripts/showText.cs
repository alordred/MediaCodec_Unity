using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showText : MonoBehaviour
{
	public Button btnStartMovie;
	public Button btnStopMovie;
	public Text textShow;
	public GameObject MovieObj;

	public MediaPlayerCtrl mediaPlayerCtrl;

	void Awake ()
	{
		//btnStartMovie.onClick.AddListener (delegate()
		//{
		//	StartMovie ();
		//});
		//btnStopMovie.onClick.AddListener (delegate()
		//{
		//	StopMovie ();
		//});
	}

	//void StartMovie ()
	//{
	//	textShow.text = "StartMovie Yes";
	//	//mediaPlayerCtrl.isMovieCanPlay = true;
	//	//MovieObj.SetActive (true);
	//}

	//void StopMovie ()
	//{
	//	textShow.text = "StopMovie No";
	//	//mediaPlayerCtrl.isMovieCanPlay = false;
	//	//MovieObj.SetActive (false);
	//}
}
