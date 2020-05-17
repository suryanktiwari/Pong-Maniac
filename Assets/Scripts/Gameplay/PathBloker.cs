using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBloker : MonoBehaviour {
    private const float MAX_TIME = 5f;
    private Rigidbody ballRb;
    public GameObject pathBloker;
    public Transform playerMagnet, aiMagnet;
    public float timeElapsed;
    private Vector3 spawnPosition;
    void Start () {
        ballRb = GetComponent<Rigidbody>();
	}
	
	void Update () {
		if ((playerMagnet.childCount==0 && aiMagnet.childCount==0)) {
			if (Mathf.Abs (ballRb.velocity.x) <= 0.05 || Mathf.Abs (ballRb.velocity.z) <= 0.05) {
				timeElapsed += Time.deltaTime;
            
			} else {
				timeElapsed = 0f;
			}
			if (timeElapsed >= MAX_TIME) {
				timeElapsed = 0f;
				spawnPosition = transform.position;
				if (Random.Range (0, 2) % 2 == 0) {
					spawnPosition.x -= 0.1f;
					spawnPosition.z -= 0.1f;
				} else {
					spawnPosition.x += 0.1f;
					spawnPosition.z += 0.1f;

				}
				Invoke ("spawnPathBloker", 0.5f);
			}
		}
	}
    void spawnPathBloker()
    {
        GameObject obj = Instantiate(pathBloker, spawnPosition, Quaternion.Euler(new Vector3(0f,45f,0f)));
		Destroy (obj, 10f);
	}
}
