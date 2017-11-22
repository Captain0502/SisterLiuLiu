/**
* Copyright (c) 2015-2016 VisionStar Information Technology (Shanghai) Co., Ltd. All Rights Reserved.
* EasyAR is the registered trademark or trademark of VisionStar Information Technology (Shanghai) Co., Ltd in China
* and other countries for the augmented reality technology developed by VisionStar Information Technology (Shanghai) Co., Ltd.
*/

using UnityEngine;

namespace EasyAR
{
	public class EasyImageTargetBehaviour : ImageTargetBehaviour
	{
		protected override void Awake ()
		{
			base.Awake ();
			TargetFound += OnTargetFound;
			TargetLost += OnTargetLost;
			TargetLoad += OnTargetLoad;
			TargetUnload += OnTargetUnload;
		}

		protected override void Start ()
		{
			base.Start ();
			HideObjects (transform);
		}

		void HideObjects (Transform trans)
		{
			for (int i = 0; i < trans.childCount; ++i)
				HideObjects (trans.GetChild (i));
			if (transform != trans)
				gameObject.SetActive (false);
		}

		void HideOtherObjects (Transform trans)
		{
//			for (int i = 0; i < trans.childCount; ++i)
//				HideObjects (trans.GetChild (i));
//			if (transform != trans) {
//				Debug.Log ("xxxxxxxxxxxxxxx");
//				gameObject.SetActive (false);
//			} else {
//				Debug.Log ("================");
//			}
//
			GameObject tag0 = GameObject.FindGameObjectWithTag ("Tag0");
			GameObject tag1 = GameObject.FindGameObjectWithTag ("Tag1");

//			if (tag0.transform != transform) {
//				tag0.SetActive (false);
//			} else {
//				tag0.SetActive (true);
//			}
			if (tag0 != null) {
				tag0.SetActive (tag0.transform == transform);
			}

			if (tag1 != null) {
				tag1.SetActive (tag1.transform == transform);
			}


		}

		void ShowObjects (Transform trans)
		{
			for (int i = 0; i < trans.childCount; ++i)
				ShowObjects (trans.GetChild (i));
			if (transform != trans)
				gameObject.SetActive (true);

//			GameObject rainbowObject = GameObject.Find ("rainbow");
//			GameObject rainbowObject = GameObject.FindGameObjectWithTag ("rainbow");
//
//			if (rainbowObject == null) {
//				Debug.Log ("-------------------- rainbow fail----------------------");
//			} else {
//				Debug.Log ("-------------------- rainbow succes ----------------------");
////				rainbowObject.SetActive (false);
//				ParticleEmitter pe = rainbowObject.GetComponent<ParticleEmitter> ();
//				pe.enabled = false;
//			}
		}

		void OnTargetFound (ImageTargetBaseBehaviour behaviour)
		{

			Debug.Log ("--------------------" + transform.tag);


			ShowObjects (transform);

			HideOtherObjects (transform);
			Debug.Log ("Found: " + Target.Id);
		}

		void OnTargetLost (ImageTargetBaseBehaviour behaviour)
		{
//			HideObjects (transform);
			Debug.Log ("Lost: " + Target.Id);
		}

		void OnTargetLoad (ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
		{
			Debug.Log ("Load target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
		}

		void OnTargetUnload (ImageTargetBaseBehaviour behaviour, ImageTrackerBaseBehaviour tracker, bool status)
		{
			Debug.Log ("Unload target (" + status + "): " + Target.Id + " (" + Target.Name + ") " + " -> " + tracker);
		}
	}
}
