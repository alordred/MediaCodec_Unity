using UnityEngine;
using System.Collections;

public class AvatarScript : MonoBehaviour
{
	public GameObject VideoPlayback;
	Animator anim;
	int jumpHash = Animator.StringToHash ("Jump");
	int runHash = Animator.StringToHash ("Sprint");
	int walkStateHash = Animator.StringToHash ("Base Layer.Walk");
	private Vector3 currentRotationEuler;
	private Quaternion currentRotation;
	private Vector3 rotationDeltaEuler;
	private Quaternion rotationDelta;



	//AndroidJavaClass jc;
	//AndroidJavaObject jo;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
		VideoPlayback.GetComponent<MediaPlayerCtrl> ().Pause ();
		//jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		//jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Turn();
		//AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
		currentRotation = GvrController.Orientation;
		currentRotationEuler = currentRotation.eulerAngles;
		currentRotationEuler.x = 0.0f;
		currentRotationEuler.z = 0.0f;
		currentRotation.eulerAngles = currentRotationEuler;
		transform.rotation = currentRotation;

		if (GvrController.IsTouching) {
			anim.SetFloat ("Speed", 0.26f);
			VideoPlayback.GetComponent<MediaPlayerCtrl> ().Play ();
		} else {
			anim.SetFloat ("Speed", 0.0f);
			VideoPlayback.GetComponent<MediaPlayerCtrl> ().Pause ();
		}

		if (GvrController.AppButtonDown) {
			anim.SetTrigger (jumpHash);
			//long downloadId = jo.Call<long>("downloadFile", "http://192.168.0.116/video/sunlh_png.mp4", "sunlh_unity.mp4");
		}
        
		if (GvrController.ClickButton) {
			anim.SetBool (runHash, true);
		} else {
			anim.SetBool (runHash, false);
		}
	
	}

	void FixedUpdate ()
	{
		//TouchPadTurn();
		//Debug.Log("The current position is " + GvrController.TouchPos);
	}

	Quaternion getGvrCurrentPose ()
	{
		var rot = GvrViewer.Instance.HeadPose.Orientation;
		Debug.Log ("The current head rotation is " + rot);
		return rot;
	}

	void GvrTurn ()
	{
		//Turn based on the GvrView
		currentRotation = getGvrCurrentPose ();
		currentRotationEuler = currentRotation.eulerAngles;

		rotationDeltaEuler = currentRotationEuler - transform.rotation.eulerAngles;
		rotationDeltaEuler.x = 0.0f;
		rotationDeltaEuler.z = 0.0f;

		currentRotationEuler = transform.rotation.eulerAngles + rotationDeltaEuler;
		currentRotation.eulerAngles = currentRotationEuler;

		transform.rotation = currentRotation;
	}

	void TouchPadTurn ()
	{
		//TODO: Get the knowledge of the touchpad's coordinate!
		Vector2 touchPosition = GvrController.TouchPos;
		Vector3 touchPosition3D = new Vector3 (touchPosition.x, 0f, touchPosition.y);
		Quaternion destRotation = Quaternion.LookRotation (touchPosition3D);

		transform.rotation = destRotation;
	}

}
