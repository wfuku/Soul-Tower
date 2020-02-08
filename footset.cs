using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footset : MonoBehaviour {

    public GameObject footPrintPrefab;
    float time = 0;
    
    void Update()
    {
        //一定時間置きに足跡を作成
        this.time += Time.deltaTime;
        if (this.time > 0.35f)
        {
            this.time = 0;
            Instantiate(footPrintPrefab, transform.position, transform.rotation);
        }
    }

}
