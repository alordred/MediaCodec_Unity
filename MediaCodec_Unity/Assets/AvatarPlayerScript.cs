using UnityEngine;
using System.Collections;

public class AvatarPlayerScript : MonoBehaviour {

    private Vector3 currentRotationEuler;
    private Quaternion currentRotation;
    private Vector3 rotationDeltaEuler;
    private Quaternion rotationDelta;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Turn();

    }

    Quaternion getCurrentPose()
    {
        var rot = GvrViewer.Instance.HeadPose.Orientation;
        Debug.Log("The current head rotation is " + rot);
        return rot;
    }

    void Turn()
    {
        currentRotation = getCurrentPose();
        currentRotationEuler = currentRotation.eulerAngles;

        rotationDeltaEuler = currentRotationEuler - transform.rotation.eulerAngles;
        rotationDeltaEuler.x = 0.0f;
        rotationDeltaEuler.z = 0.0f;

        currentRotationEuler = transform.rotation.eulerAngles + rotationDeltaEuler;
        currentRotation.eulerAngles = currentRotationEuler;

        transform.rotation = currentRotation;
    }
}
