using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{

	private GameObject fanModel;

//	private Vector2 beginPostionOfFinger;
//	// Use this for initialization
//	private Vector2 currentPostionOfFinger;

	//上次触摸点1(手指1)
	private Touch oldTouch1;

	//上次触摸点2(手指2)
	private Touch oldTouch2;

	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.touchCount == 1) {

			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				Debug.Log ("Touch begin");
			} else if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				Vector2 deltaPos = Input.GetTouch (0).deltaPosition;
				transform.Translate (Vector3.right * deltaPos.x * 0.005f, Space.World);
				transform.Translate (Vector3.forward * deltaPos.y * 0.005f, Space.World);
			}
		} else if (Input.touchCount == 2) {
			LookControl ();
		}
		
	}

	void LookControl ()
	{
		//多点触摸, 放大缩小  
		Touch newTouch1 = Input.GetTouch (0);
		Touch newTouch2 = Input.GetTouch (1);

		//第2点刚开始接触屏幕, 只记录，不做处理  
		if (newTouch2.phase == TouchPhase.Began) {
			oldTouch2 = newTouch2;
			oldTouch1 = newTouch1;
			return;
		}

		//计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
		float oldDistance = Vector2.Distance (oldTouch1.position, oldTouch2.position);
		float newDistance = Vector2.Distance (newTouch1.position, newTouch2.position);

		//两个距离之差，为正表示放大手势， 为负表示缩小手势  
		float offset = newDistance - oldDistance;

		//放大因子， 一个像素按 0.01倍来算(100可调整)  
		float scaleFactor = offset / 1000f;
		Vector3 localScale = transform.localScale;
		Vector3 scale = new Vector3 (localScale.x + scaleFactor, localScale.y + scaleFactor, localScale.z + scaleFactor);

		//最小缩放到 0.2 倍  
		if (scale.x > 0.1f && scale.y > 0.1f && scale.z > 0.1f) {
			transform.localScale = scale;
		}

		//记住最新的触摸点，下次使用  
		oldTouch1 = newTouch1;
		oldTouch2 = newTouch2;
	}

	public void MoveLeft ()
	{

		if (null != QueryFanModel ()) {
			Debug.Log ("Move Left");
			fanModel.transform.Translate (Vector3.back * 1.0f, Space.World);
		}
	}

	public void MoveRight ()
	{
		Debug.Log ("Move Right");
		if (null != QueryFanModel ()) {
			fanModel.transform.Translate (Vector3.forward * 1.0f, Space.World);
		}
	}

	public void MoveUp ()
	{
		Debug.Log ("Move Up");
		if (null != QueryFanModel ()) {
			fanModel.transform.Translate (Vector3.left * 1.0f, Space.World);
		}

	}

	public void MoveDown ()
	{
		Debug.Log ("Move Down");
		if (null != QueryFanModel ()) {
			fanModel.transform.Translate (Vector3.right * 1.0f, Space.World);
		}
	}

	public void Enlarge ()
	{
		if (null != QueryFanModel ()) {
			float scaleFactor = 1.0f;
			Vector3 localScale = fanModel.transform.localScale;
			Vector3 scale = new Vector3 (localScale.x + scaleFactor, localScale.y + scaleFactor, localScale.z + scaleFactor);

			//最小缩放到 0.2 倍  
			if (scale.x > 0.1f && scale.y > 0.1f && scale.z > 0.1f) {
				fanModel.transform.localScale = scale;
			}
		}
	}

	public void Shrink ()
	{
		if (null != QueryFanModel ()) {
			float scaleFactor = 1.0f;
			Vector3 localScale = fanModel.transform.localScale;
			Vector3 scale = new Vector3 (localScale.x - scaleFactor, localScale.y - scaleFactor, localScale.z - scaleFactor);

			//最小缩放到 0.2 倍  
			if (scale.x > 0.1f && scale.y > 0.1f && scale.z > 0.1f) {
				fanModel.transform.localScale = scale;
			}
		}
	}

	GameObject QueryFanModel ()
	{
		if (null == fanModel) {
			fanModel = GameObject.FindGameObjectWithTag ("GirlModel");
		} 

		return fanModel;
	}
}
