using UnityEngine;
using System.Collections;

public class AttackArea : MonoBehaviour {
	public CharacterStatus status;

	public AudioClip hitSeClip;
	AudioSource hitSeAudio;
    public int LR;//R:0 L:1
    void Start()
	{
		//status = transform.root.GetComponent<CharacterStatus>();

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
	AttackInfo GetAttackInfo()
	{			
		AttackInfo attackInfo = new AttackInfo();
        // 攻撃力の計算.
        attackInfo.attackPower = status.Power;

		// 攻撃強化中
		if (status.powerBoost)
			attackInfo.attackPower += attackInfo.attackPower;
		
		attackInfo.attacker = transform.root;
		
		return attackInfo;
	}
	
	// 当たった.
	void OnTriggerEnter(Collider other)
	{
		// 攻撃が当たった相手のDamageメッセージをおくる.
		other.SendMessage("Damage",GetAttackInfo());

		// オーディオ再生.
		hitSeAudio.Play();
	}
	
	
	// 攻撃判定を有効にする.
    
	void OnAttack()
	{
		GetComponent<Collider>().enabled = true;
	}
	
	
	// 攻撃判定を無効にする.
	void OnAttackTermination()
	{
		GetComponent<Collider>().enabled = false;
	}
    

    void OnAttack_R()
    {
        if (LR == 0) {
            GetComponent<Collider>().enabled = true;
        }
    }


    // 攻撃判定を無効にする.
    void OnAttackTermination_R()
    {
        if (LR == 0)
        {
            GetComponent<Collider>().enabled = false;
        }
    }


    void OnAttack_L()
    {
        if (LR == 1)
        {
            GetComponent<Collider>().enabled = true;
        }
    }


    // 攻撃判定を無効にする.
    void OnAttackTermination_L()
    {
        if (LR == 1)
        {
            GetComponent<Collider>().enabled = false;
        }
    }
}
