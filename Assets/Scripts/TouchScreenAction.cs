using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScreenAction : MonoBehaviour
{

	public bool hasClickButton = false;

	Animator personAnimator;
	// Use this for initialization

	void Start ()
	{
		personAnimator = this.GetComponent<Animator> ();
		if (personAnimator) {
			Debug.Log ("--------------------------------" + "Success");
		} else {
			Debug.Log ("--------------------------------" + "fail");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (1 == Input.touchCount && Input.GetTouch (0).phase == TouchPhase.Ended) {
			if (false == hasClickButton) {
				Debug.Log ("-----------------------Touch");
				personAnimator.SetInteger ("ActionID", 1);
				showRainbowEffect ();
			} else {
				hasClickButton = false;
			}
		}
	}

	private void showRainbowEffect ()
	{
		GameObject rainbowObject = GameObject.FindGameObjectWithTag ("rainbow");

		if (rainbowObject == null) {
			Debug.Log ("-------------------- rainbow fail----------------------");
		} else {
			Debug.Log ("-------------------- rainbow succes ----------------------");

			ParticleSystem pe = rainbowObject.GetComponent<ParticleSystem> ();
			pe.Play ();

			AudioSource[] items = gameObject.GetComponents<AudioSource> ();
			Debug.Log ("---------" + items.Length);

			AudioSource audio1 = items[0];
			audio1.Stop ();

			AudioSource audio2 = items[1];
			audio2.Play();
		}
	}
		
}
