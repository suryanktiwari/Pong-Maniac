using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class starter : MonoBehaviour {

	void Start () {
		Invoke ("start",1.60f);
	}
		
	void start()
	{
		SceneManager.LoadScene ("Main Scene");
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
			start ();
	}
}
