using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour
{

	// 抖动目标的transform(若未添加引用，怎默认为当前物体的transform)
    private Transform camTransform;

	//持续抖动的时长
    private float shake = 0f;

	// 抖动幅度（振幅）
	//振幅越大抖动越厉害
	public float shakeAmount = 0.7f;
	public float decreaseFactor = 1.0f;
    private GameController gameController;
    public playerController pc;
    private bool IsAttack;

	Vector3 originalPos;

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
        gameController = GameController.GetInstance();
	}

    public void ShakeCamera(float time)
	{
        shake = time;
		originalPos = camTransform.localPosition;
	}

	void Update()
	{

		if (shake > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shake -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shake = 0f;
			camTransform.localPosition = originalPos;
		}
	}



    /// <summary>
    /// 向左看
    /// </summary>
	private void LookLeft()
    {
        //键值对儿的形式保存iTween所用到的参数  
        Hashtable args = new Hashtable();
        args.Add("easeType", iTween.EaseType.easeOutBack);
        args.Add("from", 80f);
        args.Add("to", -80f);
        //变化过程中（ValueTo必写参数）  
        args.Add("onupdate", "AnimationUpdata2");
        args.Add("onupdatetarget", gameObject);
        args.Add("oncomplete", "LookRight");
        args.Add("time", 1f);
        iTween.ValueTo(this.gameObject, args);
    }

	/// <summary>
	/// 向右看
	/// </summary>
	private void LookRight()
	{
		//键值对儿的形式保存iTween所用到的参数  
		Hashtable args = new Hashtable();
        args.Add("easeType", iTween.EaseType.easeOutBack);
		args.Add("from", -80f);
		args.Add("to", 0f);
		//变化过程中（ValueTo必写参数）  
		args.Add("onupdate", "AnimationUpdata2");
		args.Add("onupdatetarget", gameObject);
        args.Add("oncomplete", "LookUp");
        args.Add("time", 0.5f);
		iTween.ValueTo(this.gameObject, args);
	}

	/// <summary>
	/// 向上看
	/// </summary>
	private void LookUp()
	{
		//键值对儿的形式保存iTween所用到的参数  
		Hashtable args = new Hashtable();
        args.Add("easeType", iTween.EaseType.easeOutBack);
		args.Add("from", 0f);
		args.Add("to", -60f);
		//变化过程中（ValueTo必写参数）  
		args.Add("onupdate", "AnimationUpdata");
		args.Add("onupdatetarget", gameObject);
        args.Add("oncomplete", "LookDown");
        args.Add("time", 1f);
		iTween.ValueTo(this.gameObject, args);
	}

	/// <summary>
	/// 向下看
	/// </summary>
	private void LookDown()
	{
		//键值对儿的形式保存iTween所用到的参数  
		Hashtable args = new Hashtable();
        args.Add("easeType", iTween.EaseType.easeOutBack);
		args.Add("from", -60f);
		args.Add("to", 0f);
		//变化过程中（ValueTo必写参数）  
		args.Add("onupdate", "AnimationUpdata");
		args.Add("onupdatetarget", gameObject);
        args.Add("oncomplete", "AttackDelay");
        args.Add("time", 1f);
		iTween.ValueTo(this.gameObject, args);
	}

    private void AttackDelay()
    {
        StartCoroutine("AttackDelayCoroutine");
    }

    private void FallDownAction_3(float FallDownTime)
	{
		//键值对儿的形式保存iTween所用到的参数  
		Hashtable args = new Hashtable();
		args.Add("from", -90f);
		args.Add("to", 90f);
		//变化过程中（ValueTo必写参数）  
		args.Add("onupdate", "AnimationUpdata");
		args.Add("onupdatetarget", gameObject);
        args.Add("time", FallDownTime);
		iTween.ValueTo(this.gameObject, args);
	}

	public void AnimationUpdata(object obj)
	{
		float per = (float)obj;
        this.transform.rotation = Quaternion.Euler(new Vector3(per, 0f, 0f));
	}

	public void AnimationUpdata2(object obj)
	{
		float per = (float)obj;
		this.transform.rotation = Quaternion.Euler(new Vector3(0f, per, 0f));
	}

	public IEnumerator AttackDelayCoroutine()
	{
        yield return new WaitForSeconds(0.4f);
        pc.isPlayerAttack = true;
		yield return null;
	}
}