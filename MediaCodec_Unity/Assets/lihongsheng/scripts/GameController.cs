using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameController : MonoBehaviour {
    private static GameController instance;
    public GameObject skObj;
    public GameObject playerObj;
    public float createSkDelay;
    public bool OnComputerDebug;

    public Transform viewTrans;
    public GameObject[] swordOthers;
    public GameObject handObj;
    public bool isPlayerBloodVisible;
    public bool isFallDownMode;
    private Transform skTrans;
	private Transform playerTrans;
	private Vector3 skPos;
	private Vector3 playerPos;
    private Vector3 offsetPos;
    private Quaternion skQuaternion;
    private Transform newSkTrans;
	public bool isRecordMode;
	public delegate void tempChange(object sender, EventArgs e);
	public event tempChange OntempChange;
	//由于Unity设计问题所以这样写IsFirstPersonViewInspector
	public bool IsFirstPersonViewInspector;
    public bool IsInternetMode;
	//[SerializeField] 私有变量写法
	private bool isFirstPersonView;
    public bool IsFirstPersonView{
        get
        {
            return isFirstPersonView;
        }
        set
        {
            if (isFirstPersonView != value) 
            {
                OntempChange(this, new EventArgs());
            }
            isFirstPersonView = value;
        }
    }

    public static GameController GetInstance()
	{
        if(instance == null)
        {
            instance = new GameController();
        }
        return instance;
    }

    void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        //机智如我，记得检查效率就行
        IsFirstPersonView = IsFirstPersonViewInspector;
    }

    void Start(){
        OntempChange += IsFirstPersonViewChanged;
		skTrans = skObj.transform;
        playerTrans = playerObj.transform;
        skPos = skTrans.position;
        playerPos = playerTrans.position;
        offsetPos = new Vector3(skPos.x-playerPos.x,skPos.y - playerPos.y,skPos.z - playerPos.z);
        skQuaternion = skTrans.rotation;
        skObj.gameObject.SetActive(false);
        //DelayCreateSk();
        //初始化放在其他地方
        //IsFirstPersonView = IsFirstPersonViewInspector;
    }

    private void IsFirstPersonViewChanged(object sender, EventArgs e)
    {
        if(!IsFirstPersonView)
        {
			viewTrans.position += new Vector3(0.0f, 0.0f, 1.1f);
			foreach (GameObject go in swordOthers)
			{
				go.SetActive(false);
                handObj.SetActive(true);
			}
        }else{
			viewTrans.position -= new Vector3(0.0f, 0.0f, 1.1f);
			foreach (GameObject go in swordOthers)
			{
                go.SetActive(true);
                handObj.SetActive(false);
			}
        }
    }

    public void CreateSk()
    {
        Vector3 currentPlayerPos = playerObj.transform.position;
        newSkTrans = Instantiate(skTrans,new Vector3(currentPlayerPos.x+offsetPos.x,currentPlayerPos.y + offsetPos.y,currentPlayerPos.z + offsetPos.z),skQuaternion);
        playerObj.GetComponent<playerController>().sc = newSkTrans.gameObject.GetComponent<skeletonController>();
        newSkTrans.gameObject.SetActive(true);
    }

    public void DelayCreateSk(){
        StartCoroutine("CreateSkCoroutine");
    }

    private IEnumerator CreateSkCoroutine()
	{
        yield return new WaitForSeconds(createSkDelay);
        CreateSk();
        StopCoroutine("CreateSkCoroutine");
        yield return null;
	}


}
