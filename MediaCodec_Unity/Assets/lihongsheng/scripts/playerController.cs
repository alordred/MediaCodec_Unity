using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Animator animator;
    public GameObject VideoPlayback;
    MediaPlayerCtrl mediaPlayerCtrl;
    Vector3 ts;
    public skeletonController sc;
    public BloodBox bloodBox;
    private float allHp = 2000f;
    public float currentHp;
    public float playerPower = 1000f;
    private GameController gameController;
    public Transform playerMoveTrans;
    public float playerMoveSpeed;
    public GameObject daoEffect;
    public BarbarianController barbarianController;
    public float playerAttackRange;
    public Transform daoEffectPosition;
    public CameraMove cm;
    public Transform FallDownPosition;
    public float FallDownWalkForwardTime;
    private bool setWalk = false;
    public float FallDownUpRotation;
    public float FallDownUpTime;
    public float FallDownRotation;
    public float FallDownTime;
    public GameObject ShakeCamera;
    public float FallDownShakeTime;
    public float DelayFallDownTime;
    public float DelayPlayFurionTime;
    public bool isPlayerAttack;
    private int attackTimes;
    public HttpManager httpManager;
    private int count;

    private Vector2 current_touchpos;

    //random seek func
    private long currentFrameTime = 0;
    //0 == stop
    //1 == forward
    //2 == left
    //3 == back
    //4 == right
    private int currentDirection = 0;

    void Awake()
    {
        attackTimes = 0;
        currentHp = allHp;
        ts = this.transform.position;
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("Idling", true);//stop moving
        mediaPlayerCtrl = VideoPlayback.GetComponent<MediaPlayerCtrl>();
    }

    void Start()
    {
        currentFrameTime = 0;
        count = 0;
        gameController = GameController.GetInstance();
        if(!gameController.isPlayerBloodVisible)
        {
            bloodBox.gameObject.SetActive(false);
        }
        if (gameController.isRecordMode)
        {
            StartCoroutine("DelayPlayFallDown");
        }

	}

    void Update()
    {
        //remember the On Computer Debug in inspector (GameController)
        if (gameController.isRecordMode)
        {
			if (setWalk)
			{
				animator.SetBool("Idling", false);
				mediaPlayerCtrl.Play();
			}
			else
			{
				animator.SetBool("Idling", true);
				mediaPlayerCtrl.Pause();
			}
			if (isPlayerAttack)
			{
				animator.SetTrigger("Use");
			}
        }
	}

    void LateUpdate()
    {
		//视频录制用
		if(gameController.isRecordMode)
        {
            return;
        }

		//=========================在Android手机VR播放时=========================
        if (!gameController.OnComputerDebug)
        {
            //暂时注释
			//if (GvrController.IsTouching)
			//{
			//	animator.SetBool("Idling", false);
			//	this.transform.position = new Vector3(ts.x, ts.y, ts.z += (Time.deltaTime * playerMoveSpeed));
			//	mediaPlayerCtrl.Play();
   //             if(gameController.IsInternetMode)
   //             {
			//		count++;
			//		//Debug.Log("count:" + count);
			//		if (count % 60 == 0)
			//		{
			//			//Debug.Log("DownLoadViking");
			//			httpManager.DownLoadViking();
			//		}
   //             }
			//}
			//else
			//{
			//	animator.SetBool("Idling", true);
			//	mediaPlayerCtrl.Pause();
			//}
			//if (GvrController.AppButtonDown)
			//{
			//	animator.SetTrigger("Use");
   //             //Debug.Log("GvrController.AppButtonDown");
			//}

           
            if (GvrController.IsTouching)
            {
                animator.SetBool("Idling", false);
				current_touchpos = GvrController.TouchPos;
				//Debug.Log(GvrController.TouchPos);
				//Debug.Log("EasyMovieTextureCsharp, current_touchpos = "+ current_touchpos.x +" , " + current_touchpos.y);

				if ((current_touchpos.y < current_touchpos.x) && (current_touchpos.y < 1 - current_touchpos.x))
				{
					Debug.Log("EasyMovieTextureCsharp, current_touchpos = forward");
					mediaPlayerCtrl.setDirection(1);//1
					return;
					//if(currentFrameTime%60 == 0){
					//    mediaPlayerCtrl.setDirection(currentFrameTime);
					//}
					//currentFrameTime += 20;
				}
				else if ((current_touchpos.y < current_touchpos.x) && (current_touchpos.y > 1 - current_touchpos.x))
				{
					Debug.Log("EasyMovieTextureCsharp, current_touchpos = right");
					mediaPlayerCtrl.setDirection(4);//4
					return;
				}
				else if ((current_touchpos.y > current_touchpos.x) && (current_touchpos.y > 1 - current_touchpos.x))
				{
					Debug.Log("EasyMovieTextureCsharp, current_touchpos = backward");
					mediaPlayerCtrl.setDirection(3);//3
					return;
				}
				else if ((current_touchpos.y > current_touchpos.x) && (current_touchpos.y < 1 - current_touchpos.x))
				{
					Debug.Log("EasyMovieTextureCsharp, current_touchpos = left");
					mediaPlayerCtrl.setDirection(2);//2
					return;
				}
            }
            animator.SetBool("Idling", true);
		}
		//=========================在Android手机VR播放时=========================

		//=========================在Mac调试播放时=========================
		if(gameController.OnComputerDebug)
        {
			if (Input.GetKey(KeyCode.W))
			{
				animator.SetBool("Idling", false);
                this.transform.position = new Vector3(ts.x, ts.y, ts.z+=(Time.deltaTime * playerMoveSpeed));
				mediaPlayerCtrl.Play();
				if (gameController.IsInternetMode)
				{
					count++;
					Debug.Log("count:" + count);
					if (count % 60 == 0)
					{
						Debug.Log("DownLoadViking");
						httpManager.DownLoadViking();
					}
				}
            }else
            {
				animator.SetBool("Idling", true);
				mediaPlayerCtrl.Pause();
            }
			if (Input.GetKeyDown(KeyCode.E))
			{
				animator.SetTrigger("Use");
			}
        }
		//=========================在Mac调试播放时=========================
	}
    public void ScBeHitted()
    {
        if(sc!= null)
        {
           sc.BeHitted(); 
        }
    }

    public void BarnarianBeHitted()
    {
        setWalk = false;
        attackTimes++;
        //击打次数
		if (attackTimes  > 0)
		{
			isPlayerAttack = false;
		}
        if(barbarianController != null)
        {
            float distance = Vector3.Distance(this.transform.position, barbarianController.transform.position);
            //Debug.Log(distance);
            if(distance <playerAttackRange)
            {
                //Debug.Log("BarnarianBeHitted");
                Instantiate(daoEffect,daoEffectPosition);
                barbarianController.BeHitted();
                cm.ShakeCamera(0.1f);
            }
        }
    }



    public void SetBoold(float changeBlood)
    {
        currentHp = currentHp - changeBlood;
        if (currentHp <= 0.0f){
            //死亡
            currentHp = 0.0f;
            animator.SetInteger("Death", 2);
			if (sc != null)
			{
				sc.isPlayerDie = true;
			}
        }
        if(gameController.isPlayerBloodVisible)
        {
            bloodBox.OnBloodChange(allHp, currentHp);
        }
    }

    /// <summary>
    /// 向前
    /// </summary>
    private void FallDownAction_1()
    {
        setWalk = true;
		//键值对儿的形式保存iTween所用到的参数
		Hashtable args = new Hashtable();
		//这里是设置类型，iTween的类型又很多种，在源码中的枚举EaseType中
		//例如移动的特效，先震动在移动、先后退在移动、先加速在变速、等等
        args.Add("easeType", iTween.EaseType.linear);
		// x y z 标示移动的位置。
        args.Add("x", FallDownPosition.position.x);
		args.Add("y", FallDownPosition.position.y);
		args.Add("z", FallDownPosition.position.z);

		//移动的时间
		args.Add("time", FallDownWalkForwardTime);
        if(gameController.isFallDownMode)
        {
            args.Add("oncomplete", "FallDownAction_2");
        }else{
            //args.Add("oncomplete", "LookRight");
        }
        iTween.MoveTo(this.gameObject, args);

    }

    /// <summary>
    /// 被绊倒头朝上
    /// </summary>
    private void FallDownAction_2()
	{
        Debug.Log("FallDownAction_2");
        setWalk = false;
		//键值对儿的形式保存iTween所用到的参数  
		Hashtable args = new Hashtable();
		//args.Add("delay", FallDownShakeTime);
		// x y z 旋转的角度  
		args.Add("x", FallDownUpRotation);
		args.Add("y", 0f);
		args.Add("z", 0f);
        //时间
        args.Add("time", FallDownUpTime);
		args.Add("oncompleteparams", FallDownTime);
		if (gameController.isFallDownMode)
		{
            args.Add("oncomplete", "FallDownAction_3");
			cm.ShakeCamera(FallDownShakeTime);
        }else
        {
            args.Add("oncomplete", "LookRight");
        }
		iTween.RotateTo(ShakeCamera, args);
	}


	/// <summary>
	/// 向右看
	/// </summary>
	private void LookRight()
	{
        //setWalk = false;
        //Debug.Log("LookRight");
		//键值对儿的形式保存iTween所用到的参数
          
		Hashtable args = new Hashtable();
        args.Add("easeType", iTween.EaseType.easeOutBack);
		args.Add("from", 0f);
		args.Add("to", 80f);
		//变化过程中（ValueTo必写参数）  
		args.Add("onupdate", "AnimationUpdata");
		args.Add("onupdatetarget", gameObject);
		args.Add("oncomplete", "LookLeft");
        args.Add("time", 0.7f);
		iTween.ValueTo(ShakeCamera, args);
	}

	public void AnimationUpdata(object obj)
	{
		float per = (float)obj;
        cm.transform.rotation = Quaternion.Euler(new Vector3(0f, per, 0f));
	}

    public IEnumerator DelayPlayFallDown()
    {
        yield return new WaitForSeconds(1f);
        FallDownAction_1();
        yield return new WaitForSeconds(1f);
        LookRight();
        yield return null;
    }
}
