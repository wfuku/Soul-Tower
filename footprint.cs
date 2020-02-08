using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footprint : MonoBehaviour {

    void Start(){
        StartCoroutine(Disappearing());
    }
    //コルーチンで足跡を自動削除
    IEnumerator Disappearing()
    {
        int step = 300;
        for (int i = 0; i < step; i++)
        {
            GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, 1 - 1.0f * i / step);
            yield return null;
        }
        Destroy(gameObject);
    }

}
