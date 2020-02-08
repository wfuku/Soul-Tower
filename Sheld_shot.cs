using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheld_shot : MonoBehaviour {
    public GameObject bullet;
    public Transform direction;
    public float rate = 1;
    // 弾丸発射点
    public Transform muzzle;

    // 弾丸の速度
    public float speed = 1000;
    public float occurrence = 10;
    Vector3 forward;
	// Use this for initialization
	void Start () {
        GetComponentInParent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
         forward= GetComponentInParent<Transform>().forward;
        if (Input.GetButton("Fire1") && rate <= 0)
        {
            // 弾丸の複製
            GameObject bullets = GameObject.Instantiate(bullet) as GameObject;

            Vector3 force;
            //force = direction.gameObject.transform.forward * speed;
            transform.LookAt(direction);
            force = transform.forward * speed;


            // Rigidbodyに力を加えて発射
            bullets.GetComponent<Rigidbody>().AddForce(force);
            // 弾丸の位置を調整
            bullets.transform.position = muzzle.position;
            rate = occurrence;
        }
        //rateが0になったら弾がでる
        if (rate > 0)
        {
            rate -= 1;
        }
    }
}
