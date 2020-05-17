using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathBlokerBehaviour : MonoBehaviour {
	
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
