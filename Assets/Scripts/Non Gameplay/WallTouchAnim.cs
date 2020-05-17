using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTouchAnim : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		GetComponent<Animation> ().Play ();
		if (gameObject.name.Contains ("East")) {
			GameManager.Instance.player_Score.GetComponent<Animation> ().Play ();
		}else if (gameObject.name.Contains ("West")) {
			GameManager.Instance.AI_Score.GetComponent<Animation> ().Play ();
		}
	}
}
