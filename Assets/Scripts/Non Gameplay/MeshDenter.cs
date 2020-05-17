using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MeshDenter : MonoBehaviour {
	
	Vector3[] originalMesh;
	public float dentFactor;
	public LayerMask collisionMask;
	private MeshFilter meshFilter;
	void Start() {
		meshFilter = GetComponent<MeshFilter>();
		originalMesh = meshFilter.mesh.vertices;
	}

	void OnCollisionEnter(Collision collision) {
		Vector3[] meshCoordinates = originalMesh;
		// Loop through collision points
		foreach (ContactPoint point in collision.contacts) {
			// Index with the closest distance to point.
			int lastIndex = 0;
			// Loop through mesh coordinates
			for (int i = 0; i < meshCoordinates.Length; i++) {
				// Check to see if there is a closer index
				if (Vector3.Distance(point.point, meshCoordinates[i])
					< Vector3.Distance(point.point, meshCoordinates[lastIndex])) {
					// Set the new index
					lastIndex = i;
				}
			}
			// Move the vertex
			meshCoordinates[lastIndex] -= meshCoordinates[lastIndex].normalized * dentFactor;
		}
		meshFilter.mesh.vertices = meshCoordinates;
	}

	void Reset() {
		meshFilter.mesh.vertices = originalMesh;
	}
}