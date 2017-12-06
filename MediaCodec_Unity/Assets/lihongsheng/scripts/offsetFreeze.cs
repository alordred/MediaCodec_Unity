using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class offsetFreeze : MonoBehaviour {
    Vector3 initPos;
    Quaternion initRot;
	// Use this for initialization
	void Start () {
        initPos = new Vector3();
        initPos = this.transform.position;
        initRot = this.transform.rotation;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        this.transform.position = initPos;
        this.transform.rotation = initRot; 
	}
}
