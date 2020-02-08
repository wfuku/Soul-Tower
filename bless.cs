using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bless : MonoBehaviour {

    public GameObject flame;//炎のオブジェクトを取得.
    ParticleSystem p_system;
    
    void Start () {
        p_system = flame.GetComponent<ParticleSystem>();//炎のオブジェクトからParticleSystemを代入.
    }
	
    void blessStart() {
        //炎の判定とパーティクルを起動する(パーティクルは再生が終わると停止する).
        flame.SetActive(true);
        p_system.Play();
        
    }
    void blessEnd() {
        //炎の判定を止める.
        flame.SetActive(false);
    }



}
