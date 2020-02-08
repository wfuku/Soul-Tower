using UnityEngine;
using System.Collections;

public class DropItem : MonoBehaviour {
	public enum ItemKind
	{
		Attack,
		Heal,
	};
	public ItemKind kind;

	public AudioClip itemSeClip;
	
	void OnTriggerEnter(Collider other)
	{	
		// Playerか判定
		if( other.tag == "Player" ){
			// アイテム取得
			CharacterStatus aStatus = other.GetComponent<CharacterStatus>();
			aStatus.GetItem(kind);
			// 取得したらアイテムを消す
			Destroy(gameObject);

			// オーディオ再生
			AudioSource.PlayClipAtPoint(itemSeClip, transform.position);
		}
	}

	// Use this for initialization
	void Start () {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        //発生時に浮かせる
        Vector3 velocity = Random.insideUnitSphere * 2.0f +Vector3.up * 8.0f;
		GetComponent<Rigidbody>().velocity = velocity;
	}
	
	// Update is called once per frame
	void Update () {
        //角度固定しないと倒れる為
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
