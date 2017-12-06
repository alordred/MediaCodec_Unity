using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodBox : MonoBehaviour {

    public GameObject bloodBox;
    private float initBloodScaleX;
    private float initBloodScaleY;
    private float initBloodScaleZ;
    void Start(){
        initBloodScaleX = bloodBox.transform.localScale.x;
        initBloodScaleY = bloodBox.transform.localScale.y;
        initBloodScaleZ = bloodBox.transform.localScale.z;
    }

    public void OnBloodChange(float allBlood, float currentBlood)
    {
        bloodBox.transform.localScale = new Vector3(initBloodScaleX*(currentBlood/allBlood), initBloodScaleY, initBloodScaleZ);
    }
}
