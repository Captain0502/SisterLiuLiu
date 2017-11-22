using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class ButtonAction : MonoBehaviour
{

	// Use this for initialization
	[DllImport ("__Internal")]
	private static extern void saveImageAlbum (string path);

	GameObject girObjectModel;
	Animator personAnimator;
	GameObject rainbowObject;
	GameObject helpText;
	string[] parameters = { "SingActionID", "HotCall" };
	private GameObject m_Camera;
	GameObject canvas;

	bool hasInitHelpText = false;

	void Start ()
	{
//		HidelHelpText ();		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (false == hasInitHelpText) {
			hasInitHelpText = true;
			if (null != QueryHelpText ()) {
				Invoke ("HidelHelpText", 2);
			}

		}
		
	}

	//	唱歌
	public void PlaySing ()
	{
		ActionMethod (1, "SingActionID");
	}

	//	热线
	public void HotCall ()
	{
		ActionMethod (2, "HotCall");
	}

	//	特效

	public void Effect ()
	{

		Debug.Log ("---------Effect1----------");

		ActionMethod (3, "Effect");

		Debug.Log ("---------Effect2----------");


		if (null != QueryRainbowObject ()) {

			Debug.Log ("---------Effect3----------");

			ParticleSystem pe = rainbowObject.GetComponent<ParticleSystem> ();
			if (pe.isPlaying == true) {
				Debug.Log ("xxxxxxxxxxxxxxxx");
//				pe.Pause ();
				pe.Stop ();
//				rainbowObject.SetActive (false);
			} else {
				Debug.Log ("yyyyyyyyyyyyyyyyyy");
				pe.Play ();
//				rainbowObject.SetActive (true);
			}
		}

	}

	//	School

	public void SchoolAction ()
	{

		ActionMethod (4, "School");
	}

	//	帮助
	public void HelpAction ()
	{
		if (null != QueryHelpText ()) {
			if (helpText.activeSelf == true) {
				helpText.SetActive (false);
			} else {
				helpText.SetActive (true);

				Invoke ("HidelHelpText", 2);
			}
		}
	}

	void HidelHelpText ()
	{

		if (null != QueryHelpText ()) {
			helpText.SetActive (false);
		}

	}

	void ActionMethod (int index, string parameter)
	{
		if (null != QueryHelpText ()) {
			helpText.SetActive (false);
		}

		if (null != QueryFanModel ()) {

			AudioSource[] audioSources = girObjectModel.GetComponents<AudioSource> ();

			Debug.Log (audioSources.Length);

			for (int i = 0; i < audioSources.Length; ++i) {
				AudioSource audioSource = audioSources [i];
				if (i == index) {
					if (audioSource.isPlaying == false) {
						audioSource.Play ();
					}

					if (null != QueryAnimator ()) {

						if (parameter == "SingActionID") {
							personAnimator.SetInteger ("HotCall", 0);	
							personAnimator.SetInteger ("Effect", 0);	
							personAnimator.SetInteger ("School", 0);	
						}

						if (parameter == "HotCall") {
							personAnimator.SetInteger ("SingActionID", 0);	
							personAnimator.SetInteger ("Effect", 0);	
							personAnimator.SetInteger ("School", 0);	
						}

						if (parameter == "Effect") {
							personAnimator.SetInteger ("HotCall", 0);	
							personAnimator.SetInteger ("SingActionID", 0);	
							personAnimator.SetInteger ("School", 0);	
						}

						if (parameter == "School") {
							personAnimator.SetInteger ("HotCall", 0);	
							personAnimator.SetInteger ("SingActionID", 0);	
							personAnimator.SetInteger ("Effect", 0);	
						}

						personAnimator.SetInteger (parameter, 1);

						//	暂时不用
//						ValidParameters (parameter);
					}
				} else {
					audioSource.Stop ();
				}
			}
		}

	}

	//TODO: 没有达到预期效果
	void ValidParameters (string parameter)
	{

		for (int i = 0; i < parameters.Length; ++i) {

			if (parameter == parameters [i]) {
				Debug.Log ("===================" + parameter);
//				personAnimator.SetInteger (parameter, 1);	
			} else {
				personAnimator.SetInteger (parameter, 0);	
			}
		}

		personAnimator.SetInteger (parameter, 1);	
	}

	GameObject QueryFanModel ()
	{
//		if (null == girObjectModel) {
		girObjectModel = GameObject.FindGameObjectWithTag ("GirlModel");
//		} 

		Debug.Log (girObjectModel);

		return girObjectModel;
	}

	Animator QueryAnimator ()
	{

//		if (null == personAnimator) {
		if (null != QueryFanModel ()) {
			personAnimator = girObjectModel.GetComponent<Animator> ();
		}
//		}

		return personAnimator;
	}

	GameObject QueryRainbowObject ()
	{
//		if (null == rainbowObject) {
		rainbowObject = GameObject.FindGameObjectWithTag ("Effect");

		Debug.Log ("---rainbowObject-----" + rainbowObject);
//		}

		return rainbowObject;
	}

	GameObject QueryHelpText ()
	{

		if (null == helpText) {
		helpText = GameObject.FindGameObjectWithTag ("HelpText");
		}

		return helpText;

	}

	// 拍照
	public void TakeShot ()
	{
		Debug.Log ("Take photo");

#if UNITY_ANDROID
		Debug.Log("这里是安卓设备^_^");
//		ScreenCapture.CaptureScreenshot ("screenshot.png", 0); 
//		Debug.Log ("Take shot action" + Application.persistentDataPath);
//
//		savePhotoForAndroid ();
//
//		Invoke ("showTakeShotButton", 0.03f);
#endif

#if UNITY_IPHONE
		if (null != QueryCanvas ()) {

			canvas.SetActive (false);
		}

		string fileName = System.DateTime.Now.ToString ("yyyyMMddHHmmss") + ".png";
		
		ScreenCapture.CaptureScreenshot (fileName, 0); 

		saveImageAlbum (fileName);

		Invoke ("ShowCanvas", 1);
#endif

#if UNITY_STANDALONE_WIN
		Debug.Log("我是从Windows的电脑上运行的T_T");
#endif      

	}

	private void ShowCanvas ()
	{
		canvas.SetActive (true);
	}

	//	private void savePhotoForAndroid ()
	//	{
	//		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
	//		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
	//		jo.Call ("aMethod", fileName);
	//	}

	GameObject QueyMainCamera ()
	{

//		if (null == m_Camera) {
		m_Camera = GameObject.FindGameObjectWithTag ("MainCamera");
//		}

		return m_Camera;
	}

	GameObject QueryCanvas ()
	{

//		if (null == canvas) {
		canvas = GameObject.FindGameObjectWithTag ("Canvas");
//		}

		return canvas;
	}


	//	旋转

	public void RotateAction ()
	{
		if (null != QueryFanModel ()) {

			GameObject tag0 = GameObject.FindGameObjectWithTag ("Tag0");
			GameObject tag1 = GameObject.FindGameObjectWithTag ("Tag1");

			//			if (tag0.transform != transform) {
			//				tag0.SetActive (false);
			//			} else {
			//				tag0.SetActive (true);
			//			}
			if (tag0 != null) {
				girObjectModel.transform.Rotate (Vector3.up * 10.0f, Space.World);
			}

			if (tag1 != null) {
				girObjectModel.transform.Rotate (Vector3.forward * 10.0f, Space.World);
			}
		}

	}
}
