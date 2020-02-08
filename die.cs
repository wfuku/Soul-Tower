using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    //死亡時オブジェクト削除
    void DieCollclear (){
        Destroy(this.gameObject);
    }
}
