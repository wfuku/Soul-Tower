using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour
{
    
    // 体力.
    public int HP = 100;
    public int MaxHP = 100;

    // 攻撃力.
    public int Power = 10;
    //スタミナ
    public float stamina = 100;
    public float MAXstamina = 100;
    //ガード
    public bool guard = false;
    public bool guardBreak;
    public float guardBreakTime = 100;
    //回避
    public bool dodge;
    public bool dodgeActive;
    public float dodge_count = 100;
    

    // 最後に攻撃した対象.
    public GameObject lastAttackTarget = null;
    //連撃
    public bool chein;

    // プレイヤー名.
    public string characterName = "Player";

    //状態.
    public bool attacking = false;
    public bool attacking2 = false;
    public int attacking_num = 0;

    public bool died = false;
  //  public bool gurding = false;


    // 攻撃力強化
    public bool powerBoost = false;
    // 攻撃強化時間
    float powerBoostTime = 0.0f;

    // 攻撃力強化エフェクト
    ParticleSystem powerUpEffect;

    public int attackNumber;
    //遠距離攻撃をもっているか
    public bool longAttackHave;
    //抜刀
    public bool battou;
    public bool battouNow;

    // アイテム取得
    public void GetItem(DropItem.ItemKind itemKind)
    {
        switch (itemKind)
        {
            case DropItem.ItemKind.Attack:
                powerBoostTime = 7.0f;
    			powerUpEffect.Play ();
                break;
            case DropItem.ItemKind.Heal:
                // MaxHPの半分回復
                HP = Mathf.Min(HP + MaxHP / 3, MaxHP);
                break;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        //エフェクトを代入
        if (gameObject.tag == "Player")
        {
            powerUpEffect = transform.Find("PowerUpEffect").GetComponent<ParticleSystem>();
        }
    }
 
    void Update()
    {
        if (gameObject.tag != "Player")
        {
            return;
        }
        //強化時の時間の処理
        powerBoost = false;
        if (powerBoostTime > 0.0f)
        {
            powerBoost = true;
            powerBoostTime = Mathf.Max(powerBoostTime - Time.deltaTime, 0.0f);
        }
        else
        {
            powerUpEffect.Stop();
        }
    }

}
