using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TakeShotAction : MonoBehaviour
{

	[DllImport ("__Internal")]
	private static extern void saveImageAlbum (string path);

	public bool hasClickDown;
	private GameObject m_Camera;
	private GameObject takeShotButton;
	private GameObject rainbowObject;

	private string fileName;

	// Use this for initialization
	void Start ()
	{
		m_Camera = GameObject.Find ("RenderCamera");
		if (m_Camera == null) {
			Debug.Log ("Can't find RenderCamera");
		} else {
			Debug.Log ("Find RenderCamera");
		}

		takeShotButton = GameObject.Find ("Button");

		fileName = Application.persistentDataPath + "/" + "screenshot.png";

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void TakeShot ()
	{
		// iOS 

		Debug.Log ("Take shot action" + Application.persistentDataPath);

		abortTouchScreenEvent ();

		//		CaptureCamera(m_Camera.GetComponent<Camera>(),new Rect(0,0,Screen.width,Screen.height));
		//		Application.CaptureScreenshot("Screenshot.png", 0); 
		takeShotButton.SetActive (false);
		//		CaptureCamera(m_Camera.GetComponent<Camera>(),new Rect(0,0,Screen.width,Screen.height));

		fileName = System.DateTime.Now.ToString ("yyyyMMddHHmmss") + ".png";

		//		Application.CaptureScreenshot(fileName, 0); 
		ScreenCapture.CaptureScreenshot (fileName, 0); 

		saveImageAlbum (fileName);

		Invoke ("showTakeShotButton", 0.03f);

		//  Android 
//		Debug.Log ("Take shot action" + Application.persistentDataPath);
//
//		abortTouchScreenEvent ();
//
//		takeShotButton.SetActive (false);
//		Application.CaptureScreenshot ("screenshot.png", 0); 
//
//		savePhotoForAndroid ();
//
//		Invoke ("showTakeShotButton", 0.03f);
//
//		Debug.Log ("图片路径 ：>>>>>>>>>>>>>>>>>>>>>>>>>>>" + fileName);
	}


	private void savePhotoForAndroid ()
	{
		AndroidJavaClass jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
		jo.Call ("aMethod", fileName);
	}

	private void showTakeShotButton ()
	{
		takeShotButton.SetActive (true);
	}

	private void abortTouchScreenEvent ()
	{
		GameObject girlObject = GameObject.Find ("GirlObject");
		if (girlObject == null) {
			Debug.Log ("----------------屏幕截图 null -----------------");
		}

		TouchScreenAction touchScreenAction = girlObject.GetComponent<TouchScreenAction> ();
		touchScreenAction.hasClickButton = true;
	}

	Texture2D CaptureCamera (Camera camera, Rect rect)
	{  
		Debug.Log ("----------------屏幕截图-----------------");

		if (camera == null) {
			Debug.Log ("Can't find RenderCamera");
		} else {
			//			Debug.Log ("Find RenderCamera");
		}
		// 创建一个RenderTexture对象  
		RenderTexture rt = new RenderTexture ((int)rect.width, (int)rect.height, 0);  
		// 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
		camera.targetTexture = rt;  
		camera.Render ();  
		//ps: --- 如果这样加上第二个相机，可以实现只截图某几个指定的相机一起看到的图像。  
		//ps: camera2.targetTexture = rt;  
		//ps: camera2.Render();  
		//ps: -------------------------------------------------------------------  

		// 激活这个rt, 并从中中读取像素。  
		RenderTexture.active = rt;  
		Texture2D screenShot = new Texture2D ((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);  
		screenShot.ReadPixels (rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
		screenShot.Apply ();  

		// 重置相关参数，以使用camera继续在屏幕上显示  
		camera.targetTexture = null;  
		//ps: camera2.targetTexture = null;  
		RenderTexture.active = null; // JC: added to avoid errors  
		GameObject.Destroy (rt);  
		// 最后将这些纹理数据，成一个png图片文件  
		byte[] bytes = screenShot.EncodeToPNG ();  
		//		string filename = Application.persistentDataPath + "/Screenshot.png";  
		string filename = Application.persistentDataPath + "/ARFutureLeader" + System.DateTime.Now.ToString ("yyyyMMddHHmmss") + ".png";  
		System.IO.File.WriteAllBytes (filename, bytes);  
		//		Debug.Log (string.Format ("截屏了一张照片: {0}", filename)); 

		saveImageAlbum (filename);

		return screenShot;  
	}
}
