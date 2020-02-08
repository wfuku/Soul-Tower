using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        variable.Instance.direction = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
