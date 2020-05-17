using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBackRotator: MonoBehaviour {

	private const float timeRateForIncrement = 0.5f;
	private const float incrementRate = 3f;
	public float rotationRate = 1f;

	void Start () {
		
	}
	
	void Update () {
		transform.Rotate (new Vector3 (0f,0f,rotationRate));
	}

	public void rateIncrease()
	{
		InvokeRepeating ("incr", 0f, timeRateForIncrement);
	}

	void incr()
	{
		rotationRate += incrementRate*Mathf.Sign(rotationRate);
	}
}
