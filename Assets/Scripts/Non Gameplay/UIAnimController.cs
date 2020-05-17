using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimController : MonoBehaviour {


	// Use this for initialization
	public GameObject panel;
	public Button button;
	private RectTransform panelRT,buttonRT;
	private Vector3 initPos, finalPos;
	int steps = 15;
	float stepMagnitude;
	void Start () {
		panelRT = panel.GetComponent<RectTransform> ();
		buttonRT = button.GetComponent<RectTransform> ();
		initPos = buttonRT.position;
		finalPos = panelRT.position;
		stepMagnitude = (finalPos - initPos).magnitude / steps;
	}

	public void PanelActive(){
		panel.SetActive (true);
		panelRT.position = initPos;
		StartCoroutine (panelBadao ());
	}
	IEnumerator panelBadao(){
		for (int i = 1; i <= steps; i++) {
			panelRT.position = Vector3.MoveTowards (panelRT.position, finalPos, stepMagnitude);
			panelRT.localScale = (new Vector3 (1f, 1f, 1f)) * i / steps;
			yield return new WaitForSeconds (1/steps);
		}
	}

	public void PanelInactive(){
		panelRT.position = finalPos;
		StartCoroutine (panelGhatao ());
		panel.SetActive (false);
	}
	IEnumerator panelGhatao(){
		for (int i = 1; i <= steps; i++) {
			panelRT.position = Vector3.MoveTowards (panelRT.position, initPos, stepMagnitude);
			panelRT.localScale =new Vector3 (1f, 1f, 1f) * (1f - ((float)i) / steps);
			yield return new WaitForSeconds (1/steps);
		}
	//	print ("mei toh khamt");
		//panel.SetActive (false);
	}

	// Update is called once per frame
	void Update () {

	}
}