using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTransitioner : MonoBehaviour {

	private float COLOR_CHANGE_SPEED_FACTOR = 1f;
	private float COLOR_CHANGE_TIME = 0.1f;
	private float RED_VELOCITY, BLUE_VELOCITY, GREEN_VELOCITY;
	private int SIZE;
	private Color32 cur, target;

	public Color[] colors;

	void Start () {
		SIZE = colors.Length;
		cur = GetComponent<Image> ().color = colors [Random.Range (0, SIZE)];
	//	changeColor ();
	//	InvokeRepeating ("updater", 0f, COLOR_CHANGE_TIME);
	}
	
	void updater () {
//		byte r = (byte)(cur.r + COLOR_CHANGE_SPEED_FACTOR * RED_VELOCITY);
//		byte g = (byte)(cur.g + COLOR_CHANGE_SPEED_FACTOR * GREEN_VELOCITY);
//		byte b = (byte)(cur.b + COLOR_CHANGE_SPEED_FACTOR * BLUE_VELOCITY);
//		cur = new Color32(r, g, b,(byte) cur.a);

		GetComponent<Image> ().color = Color.Lerp(cur, target, COLOR_CHANGE_SPEED_FACTOR);
		if(cur.r==target.r&&cur.g==target.g&&cur.b==target.b)
			changeColor();
	}

	void changeColor()
	{
		target = colors [Random.Range (0, SIZE)];
		RED_VELOCITY = (target.r-cur.r)/cur.r;
		BLUE_VELOCITY = (target.b-cur.b)/cur.b;
		GREEN_VELOCITY = (target.g-cur.g)/cur.g;
	}
}
