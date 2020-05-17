using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBlink : MonoBehaviour {
	private const float BLINK_RATE = 3f;
	private bool state;

	public void enable () {
		InvokeRepeating ("change", BLINK_RATE, BLINK_RATE);	
		state = true;
	}

	void change(){
		state = state ? false : true;
		gameObject.SetActive (state);
		print (state);
	}

    public void disable()
    {
    	CancelInvoke();    
    }
}
