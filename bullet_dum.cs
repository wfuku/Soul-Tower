using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet_dum : MonoBehaviour
{
    public AudioClip hitSeClip;
    AudioSource hitSeAudio;
    public int LR;//R:0 L:1
    void Start()
    {
        // オーティオの初期化.
        hitSeAudio = gameObject.AddComponent<AudioSource>();
        hitSeAudio.clip = hitSeClip;
        hitSeAudio.loop = false;
    }


    public class AttackInfo
    {
        public int attackPower; // この攻撃の攻撃力.
        public Transform attacker; // 攻撃者.
    }


    // 攻撃情報を取得する.
    AttackArea.AttackInfo GetAttackInfo()
    {
        AttackArea.AttackInfo attackInfo = new AttackArea.AttackInfo();
        // 攻撃力の計算.
        attackInfo.attackPower = 10;

        // 攻撃強化中

        attackInfo.attacker = transform.root;

        return attackInfo;
    }

    // 当たった.
    void OnTriggerEnter(Collider other)
    {
        // 攻撃が当たった相手のDamageメッセージをおくる.
        other.SendMessage("Damage", GetAttackInfo());
        Destroy(this.gameObject);
        // オーディオ再生.
        hitSeAudio.Play();
        if (other.gameObject.tag == "Player")
        {
         
          // Destroy(this.gameObject);

        }
        if (other.gameObject.tag != "Player")
        {
            //            enemy.gameObject.SendMessage("ApplyDamage", 10f, SendMessageOptions.DontRequireReceiver);
          //  Destroy(this.gameObject);

            //            checck = enemy;
        }
    }





}
