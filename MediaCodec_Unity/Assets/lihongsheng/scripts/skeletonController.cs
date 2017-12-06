using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skeletonController : MonoBehaviour {
    private Animation animationSkeleton;
    private bool isBeAttacked = false;
	private float distance;
    private AnimationEvent attackEvent;
    private AnimationEvent beHittedEvent;
    public Transform playerTrans;
    public float activationDistance;
    public float canBeAttackedDistance;
    public playerController pc;
    public float skPerPower;
    public bool isPlayerDie = false;
    public BloodBox bloodBox;
    public CameraMove cm;
    public GameObject DieParticle;
    private float DieEffectTime = 2.5f;
    //血量
	private float allHp = 20000f;
	public float currentHp;
	// Use this for initialization
	void Start () {
        currentHp = allHp;
        distance = Vector3.Distance(playerTrans.position, this.transform.position);
        animationSkeleton = this.gameObject.GetComponent<Animation>();
        animationSkeleton.Play("Idle1");
        StartCoroutine("SearchForAttack");

        //为read-Only的动画加事件因为模型是老动画系统
        attackEvent = new AnimationEvent();
        attackEvent.time = animationSkeleton["Attack1h1"].length * 0.6f;
        attackEvent.functionName = "AttackEffectFunc";
        animationSkeleton["Attack1h1"].clip.AddEvent(attackEvent);

		beHittedEvent = new AnimationEvent();
		beHittedEvent.time = animationSkeleton["Hit1"].length * 0.6f;
		beHittedEvent.functionName = "SetBoold";
		animationSkeleton["Hit1"].clip.AddEvent(beHittedEvent);
	}

    void LateUpdate()
    {
        distance = Vector3.Distance(playerTrans.position, this.transform.position);
    }

    IEnumerator SearchForAttack(){
        while(!isPlayerDie){
            if (isBeAttacked){
					isBeAttacked = false;
					animationSkeleton.CrossFade("Idle1", 1.0f);
            }else{
				if (distance < activationDistance)
				{
					AttackPlay();
				}
            }
            yield return new WaitForSeconds(0.3f);
        }
        animationSkeleton.Play("Idle1");
        StopCoroutine("SearchForAttack");
        yield return null;
    }

    public void BeHitted(){
        if(distance < canBeAttackedDistance)
        {
			animationSkeleton.CrossFade("Hit1", 0.2f);
			isBeAttacked = true;
        }
	}

	public void AttackPlay()
    {
		animationSkeleton.CrossFade("Attack1h1");
	}

    public void AttackEffectFunc()
    {
        pc.SetBoold(skPerPower);
        cm.ShakeCamera(0.2f);
    }

	public void SetBoold()
	{
        currentHp = currentHp - pc.playerPower;
		if (currentHp <= 0.0f)
		{
			//死亡
			currentHp = 0.0f;
            StopCoroutine("SearchForAttack");
            StartCoroutine("Die");
		}
        bloodBox.OnBloodChange(allHp, currentHp);
	}

	IEnumerator Die()
	{
        DieParticle.SetActive(true);
        animationSkeleton.CrossFade("Hit1");
        yield return new WaitForSeconds(DieEffectTime);
        GameController gc = GameController.GetInstance();
        gc.DelayCreateSk();
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, DieEffectTime);
        yield return new WaitForSeconds(DieEffectTime);
        yield return null;
	}
}
