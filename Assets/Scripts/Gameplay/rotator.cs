using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class rotator : MonoBehaviour {
	private GameObject ball;
	private float dirX;
	public bool turn;
	private GameObject player;
	private GameObject AI;
	private int playerPad;
	private int AIPad;
	void Start () {
		if (turn) {
			dirX = -1f;
		} else {
			dirX = 1f;
		};
	
	}
	void BulletTurnSetter(GameObject tempBloke){
		if (tempBloke.name.Contains ("player")) {
			dirX = -1f;
		} else if (tempBloke.name.Contains ("AI")) {
			dirX = 1f;
		}
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
		transform.position=new Vector3(transform.position.x+Time.deltaTime*dirX,transform.position.y,transform.position.z);
	
	}
	void OnTriggerEnter(Collider other){
		 
		if ((other.gameObject.CompareTag ("player") && turn) || (other.gameObject.CompareTag ("AI") && !turn)) {

			if (this.gameObject.name.Contains ("PadLong")) {
				if (turn) {
					PowerUp.Instance.PU (PowerTypes.PlayerLong);
				} else {
					PowerUp.Instance.PU (PowerTypes.AILong);
				}
			} else if (this.gameObject.name.Contains ("PadShort")) {
				if (turn) {
					PowerUp.Instance.PU (PowerTypes.PlayerShort);
				} else {
					PowerUp.Instance.PU (PowerTypes.AIShort);
				}
			} else if (this.gameObject.name.Contains ("BigBall")) {
				PowerUp.Instance.PU (PowerTypes.BigBall);

			} else if (this.gameObject.name.Contains ("SpeedUp")) {
				PowerUp.Instance.PU (PowerTypes.FastBall);
			} else if (this.gameObject.name.Contains ("SpeedDown")) {
				if (turn) {
					PowerUp.Instance.PU (PowerTypes.PlayerSlowBall);
				} else {
					PowerUp.Instance.PU (PowerTypes.AISlowBall);
				}
			} else if (this.gameObject.name.Contains ("FlareBall")) {
				PowerUp.Instance.PU (PowerTypes.FlareBall,turn);
			} else if (this.gameObject.name.Contains ("MultiBall")) {
				PowerUp.Instance.PU (PowerTypes.MultiBall);

			} else if (this.gameObject.name.Contains ("GunPad")) {
				if (turn) {
					PowerUp.Instance.PU (PowerTypes.PlayerGun);
				} else {
					PowerUp.Instance.PU (PowerTypes.AIGun);
				}
			} else if (this.gameObject.name.Contains ("MagnetPad")) {
				if (turn) {
					PowerUp.Instance.PU (PowerTypes.PlayerMagnet);
				} else {
					PowerUp.Instance.PU (PowerTypes.AIMagnet);
				}
			} else if (this.gameObject.name.Contains ("VIPBall")) {
				PowerUp.Instance.PU (PowerTypes.VipBall,turn);

			}
		Destroy (this.gameObject);
		}

        //if powerUps outside the wall then destroy powerUps
		if (other.gameObject.name == "EastWall"||other.gameObject.name == "WestWall") {
			Destroy (this.gameObject);
		}
	}
}
