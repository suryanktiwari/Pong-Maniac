using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class MenuItems : MonoBehaviour {
	[MenuItem("Tools/Clear PlayerPrefs")]
	private static void NewMenuOption(){
		PlayerPrefs.DeleteAll ();
		Debug.Log ("Reseting");
	}

}
