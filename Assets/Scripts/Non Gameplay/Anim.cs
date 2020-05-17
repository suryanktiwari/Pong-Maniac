using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Anim : MonoBehaviour {
	private static Anim instance;
	public static Anim Instance{get{ return instance;}}

	private bool animBloke;
	public Transform AnimBloke;
	void Start () {
		instance = this;
	}

	void Update () {
		
		if(PowerUp.Instance.animPlayerPad){
			changeSize (PlayerS.Instance.transform, PowerUp.Instance.playerPadScale,0.01f);
			if (PowerUp.Instance.playerPadScale == PlayerS.Instance.transform.localScale) {
				PowerUp.Instance.animPlayerPad = false;
			}
		}
		if (PowerUp.Instance.animAIPad) {
			changeSize (AIS.Instance.transform, PowerUp.Instance.AIPadScale,0.01f);
			if (PowerUp.Instance.AIPadScale == AIS.Instance.transform.localScale) {
				PowerUp.Instance.animAIPad = false;
			}
		}

		if (PowerUp.Instance.animBall) {
			bool check=true;

			//foreach (GameObject ball in PowerUp.Instance.ballList) {
				//changeSize (ball.transform, PowerUp.Instance.BallScale,0.01f);

			changeSize (GameManager.Instance.BallContainer, PowerUp.Instance.BallScale,0.01f);
				/*if (ball.transform.parent.name.Contains ("MagContainer")) {
					float size = ball.transform.localScale.x;
					Vector3 temp=new Vector3(1/size,1/size,1/size);
					changeSize (ball.transform.parent, temp,0.01f);

				}*/
			if (GameManager.Instance.BallContainer.localScale != PowerUp.Instance.BallScale)
					check = false;
			//}
			if (check) {
				PowerUp.Instance.animBall = false;
			}
		}
		if (PowerUp.Instance.animMultiBall) {
			bool check=true;
			for (int i=1;i< PowerUp.Instance.ballList.Length;i++) {
				changeSize (PowerUp.Instance.ballList[i].transform, Vector3.zero,0.02f);

				if (PowerUp.Instance.ballList [i].transform.localScale != Vector3.zero) {
					check = false;
				}
			}
			if (check) {
				PowerUp.Instance.animMultiBall = false;
				PowerUp.Instance.ballList [1].SetActive (false);
				PowerUp.Instance.ballList [2].SetActive (false);
			}
		}

	}

	void changeSize(Transform pad, Vector3 padLength,float smooth){

		pad.transform.localScale = Vector3.MoveTowards (pad.localScale, padLength,smooth);
		
	}

	public void disapperBloke(Transform bloke){
		var temp = Instantiate (AnimBloke,bloke.transform.position,Quaternion.identity);
		temp.GetComponent<AnimBloke> ().enabled = true;
	
	}

}
