using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throws : MonoBehaviour {
    public GameObject stone;
    public float speed;
    public GameObject hand;
    public GameObject target;
    public GameObject targetPos;

    void throwsPso() {
        target= GameObject.Find("knight");
        targetPos.transform.position = target.transform.position;
    }
    void throwsAttack (){
        //弾を生成
        GameObject bullets = GameObject.Instantiate(stone) as GameObject;
        bullets.transform.position = hand.transform.position;
        //弾の方向を指定
        bullets.transform.LookAt(targetPos.transform.position);
        Vector3 force;
        force = bullets.transform.forward * speed;
        // Rigidbodyに力を加えて発射
        bullets.GetComponent<Rigidbody>().AddForce(force);
        
    }
}
