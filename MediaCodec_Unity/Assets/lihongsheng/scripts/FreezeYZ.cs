using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeYZ : MonoBehaviour {
	Vector3 initPos;
	Quaternion initRot;
	// Use this for initialization
	void Start()
	{
		initPos = new Vector3();
		initPos = this.transform.position;
		initRot = this.transform.rotation;
	}

	// Update is called once per frame
	void Update()
	{
        //transform.position = new Vector3(transform.position.x,initPos.y,initPos.z);
		this.transform.rotation = initRot;
	}
}
