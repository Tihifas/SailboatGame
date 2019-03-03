using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandMover : MonoBehaviour {
    public Vector3 velocity;
	// Use this for initialization
	void Start () {
        Rigidbody islandRB = this.GetComponent<Rigidbody>();
        islandRB.velocity = velocity;
    }
    
}
