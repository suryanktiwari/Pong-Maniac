using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
	public BlockTypes blockType;
	void Start () {
	}
	
	void Update () {
		
	}
	public void SetBlock(int val){
		blockType = (BlockTypes)val;
		if(youdidthistoher.Instance.gameplayType==2)
			GetComponent<Renderer> ().material = GameManager.Instance.blacky;
		else
			GetComponent<Renderer> ().material = GameManager.Instance.blockMaterial [(int)blockType];
		transform.localScale = new Vector3 (0.45f, 0.5f, 0.95f);
		switch (blockType) {
		case BlockTypes.One:
			break;
		case BlockTypes.Two:
			break;
		case BlockTypes.Three:
			break;
		case BlockTypes.Rock:
			break;
		case BlockTypes.Toggle:
			GetComponent<BlockToggle> ().enabled = true;
			break;
		case BlockTypes.Blink:
			GetComponent<BlockBlink> ().enabled = true;
			GetComponent<BlockBlink> ().enable();
			break;
		case BlockTypes.Blast:
			break;
		}
		gameObject.SetActive (true);
		transform.SetParent (GameManager.Instance.BlokeGroup);
		if(PowerUp.Instance.powerVar[(int)PowerTypes.FlareBall].isWorking)
			GetComponent<Collider> ().isTrigger = true;
	}

	public void ResetBlock(bool turn = true){
		
        switch (blockType)
        {
            case BlockTypes.One:
                transform.SetParent(GameManager.Instance.DeadPool);

                Anim.Instance.disapperBloke(transform);
                gameObject.SetActive(false);
                break;
            case BlockTypes.Two:
                SetBlock((int)BlockTypes.One);
                break;
            case BlockTypes.Three:
                SetBlock((int)BlockTypes.Two);
                break;
            case BlockTypes.Rock:
                break;
            case BlockTypes.Toggle:
                //GetComponent<BlockToggle>().enabled = false;
                break;
            case BlockTypes.Blink:
                //GetComponent<BlockBlink>().enabled = false;

                break;
            case BlockTypes.Blast:
                transform.SetParent(GameManager.Instance.DeadPool);

                GameManager.Instance.BlastAnimation(transform.position);
                gameObject.SetActive(false);

                break;
        }
	}

	public void HitBlock(bool turn = true){
		switch (blockType) {
		case BlockTypes.One:
			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a1, 0.2f);
			GameManager.Instance.makePowerUp (gameObject, turn);
			if (turn) {
				GameManager.Instance.makeCoin (gameObject);
			}
			GameManager.Instance.BlokePoint (turn);
			break;
		case BlockTypes.Two:

			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a1, 0.2f);
			GameManager.Instance.makePowerUp (gameObject, turn);
			if (turn) {
				GameManager.Instance.makeCoin (gameObject);
			}
			GameManager.Instance.BlokePoint (turn);

			break;
		case BlockTypes.Three:

			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a1, 0.2f);
			GameManager.Instance.makePowerUp (gameObject, turn);
			if (turn) {
				GameManager.Instance.makeCoin (gameObject);
			}
			GameManager.Instance.BlokePoint (turn);
			break;
		case BlockTypes.Rock:
			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a2, 0.2f);

			break;
		case BlockTypes.Toggle:
			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a2, 0.2f);

			break;
		case BlockTypes.Blink:
			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a2, 0.2f);

			break;
		case BlockTypes.Blast:

			GameManager.Instance.a.PlayOneShot (GameManager.Instance.a1, 0.2f);
			GameManager.Instance.makePowerUp (gameObject, turn);
			if (turn) {
				GameManager.Instance.makeCoin (gameObject);
			}
			GameManager.Instance.BlokePoint (turn);
			GameManager.Instance.Blast (gameObject,turn);
//			print ("blastHere");
			break;
		}
	}
}

