using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
	private static ObjectPool instance;
	public static ObjectPool Instance{get{ return instance; }}

	private const int poolingObjectNo = 1;

	public GameObject[] referenceObject = new GameObject[poolingObjectNo];

	public int[] poolingQuantity = new int[poolingObjectNo];

	public List<GameObject>[] poolList = new List<GameObject>[poolingObjectNo];

	void Start(){
		instance = this;
		DontDestroyOnLoad (gameObject);

		for (int i = 0; i < poolingObjectNo; i++) {
			poolList[i] = new List<GameObject>();
			for (int j = 0; j < poolingQuantity[i]; j++) {
				GameObject gameObj = Instantiate (referenceObject [i]);
				gameObj.SetActive (false);
				poolList [i].Add (gameObj);
				gameObj.transform.parent = transform;
			}
		}
	}

	//opt : objectPoolType
	public GameObject GetObject(int opt=0){
		for (int i = 0; i < poolList [(int)opt].Count; i++) {
			if (!poolList [(int)opt] [i].activeInHierarchy) {
				poolList [(int)opt] [i].SetActive (true);
				return  poolList [(int)opt] [i];
			}
		}

		GameObject gameObj = Instantiate (referenceObject [(int)opt]);
		poolList [(int)opt].Add (gameObj);
		gameObj.transform.parent = transform;
		return gameObj;

	}

	public void Reset(){
//		print ("Objects are resetting");
		for (int i = 0; i < poolList.Length; i++) {
			for (int j = 0; j < poolList [i].Count; j++) {
				//if (poolList [i] [j].activeInHierarchy) {
					poolList [i] [j].SetActive (false);
                    poolList[i][j].transform.parent = transform;
                poolList[i][j].GetComponent<BlockToggle>().enabled = false;
				poolList [i] [j].GetComponent<BlockBlink> ().disable ();
                poolList[i][j].GetComponent<BlockBlink>().enabled = false;
                //}
            }
		}

	}
}
