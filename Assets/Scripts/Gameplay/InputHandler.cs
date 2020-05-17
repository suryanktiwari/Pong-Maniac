using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	private static InputHandler instance;
	public static InputHandler Instance{get{ return instance;}}
	private const int NEGATIVE_CORRECT = -1;

	private Ray ray;
	private RaycastHit hit;
	private Vector3 mouseOldPosition = Vector3.zero;
	public GameObject camera3;
	private float relativeSpeedGradient = 1.4f;
	// Use this for initialization
	void Start () {
		instance = this;
		Input.simulateMouseWithTouches = true;
	}

    // Update is called once per frame

    public Vector3 touchPoint;
    void Update () {
		RelativeMouse();
	}

	bool mouseDown= false;
    public Vector3 relative;
	void RelativeMouse(){
		if (Input.GetMouseButton (0)) {

            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity);
            touchPoint = hit.point;
            if (mouseOldPosition != Input.mousePosition && mouseDown) {

				Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity);
				Vector3 newPointInWorld = hit.point;

				Physics.Raycast (Camera.main.ScreenPointToRay (mouseOldPosition),out hit, Mathf.Infinity);
				Vector3 oldPointInWorld = hit.point;

				relative = newPointInWorld - oldPointInWorld;
				Vector3 toMove = Vector3.ClampMagnitude (new Vector3 (relative.x, 0, relative.z), relativeSpeedGradient);
				PlayerS.Instance.Move (toMove);
			}
			mouseDown = true;
			mouseOldPosition = Input.mousePosition;
		} else {
			mouseDown = false;
		}
	}




}