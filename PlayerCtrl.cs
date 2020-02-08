using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour
{
	const float RayCastMaxDistance = 100.0f;
	CharacterStatus status;
	CharaAnimation charaAnimation;
	Transform attackTarget;
	InputManager inputManager;
	public float attackRange = 1.5f;
	GameRuleCtrl gameRuleCtrl;
	public GameObject hitEffect;
	TargetCursor targetCursor;
   // public bool Gurd;
    public bool atk;
    public float Gurd_gage;
    public bool ch;
    public bool chein;
    // ステートの種類.
    enum State
	{
		Walking,
		Attacking,
		Died,
        Gurding,
        
	} ;
	
	State state = State.Walking;		// 現在のステート.
	State nextState = State.Walking;	// 次のステート.

	public AudioClip deathSeClip;
	AudioSource deathSeAudio;
	
	
	// Use this for initialization
	void Start()
	{
		status = GetComponent<CharacterStatus>();
		charaAnimation = GetComponent<CharaAnimation>();
		inputManager = FindObjectOfType<InputManager>();
		gameRuleCtrl = FindObjectOfType<GameRuleCtrl>();
		targetCursor = FindObjectOfType<TargetCursor>();
		

		// オーディオの初期化.
		deathSeAudio = gameObject.AddComponent<AudioSource>();
		deathSeAudio.loop = false;
		deathSeAudio.clip = deathSeClip;
	}
	
	// Update is called once per frame
	void Update()
	{
		switch (state)
		{
		case State.Walking:
			Walking();
			break;
		case State.Attacking:
			Attacking();
			break;
        case State.Gurding:
			Gurding();
			break;

		}
		//状態からステートを決める
		if (state != nextState)
		{
			state = nextState;
			switch (state)
			{
			case State.Walking:
				WalkStart();
				break;
			case State.Attacking:
				AttackStart();
				break;
            case State.Died:
                Died();
                break;
            case State.Gurding:
                GurdStart();
                break;
            }
		}
	}
	
	
	// ステートを変更する.
	void ChangeState(State nextState)
	{
		this.nextState = nextState;
	}
	
	void WalkStart()
	{
		StateStartCommon();
	}

    void Walking()
    {
        if (status.battouNow == false)
        {
            if (Input.GetKeyDown(KeyCode.R)&&status.dodgeActive==false)
            {
                status.battou = !status.battou;
            }
            else  if (status.battou == true)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    atk = true;
                    ChangeState(State.Attacking);
                    status.stamina -= 10;
                    if (status.stamina <= 0){
                        status.stamina = 0;
                    }
                }
                else if (Input.GetButton("Fire2") && status.stamina > 0 && status.guardBreak == false)
                {
                    //ガード中は徐々にスタミナ減る、0以下でガードブレイク
                    ChangeState(State.Gurding);
                    status.stamina -= 0.1f;
                    if (status.stamina <= 0)
                    {
                        status.guardBreak = true;
                        status.stamina = 0;
                        status.guardBreakTime = 100;
                    }

                }
            }
            if (status.guardBreak == false)
            {
                status.stamina += 0.5f;
                if (status.stamina >= status.MAXstamina)
                {
                    status.stamina = status.MAXstamina;
                }               
            }

            if (status.guardBreak == true)
            {//ガードブレイクは時間で回復
                status.guardBreakTime -= 1;
                if (status.guardBreakTime <= 0)
                {
                    status.guardBreakTime = 0;
                    status.guardBreak = false;
                }
            }

        }
    }
    // 攻撃ステートが始まる前に呼び出される.
    void AttackStart()
	{
		StateStartCommon();
	    status.attacking = true;
        SendMessage("StopMove");
	}
    void StartAttackHit()
    {
        chein = true;
    }

    void EndAttackHit()
    {
        chein = false;
    }
    void EndAttack()
    {
    }
    void DodgeEnd() {
         ChangeState(State.Walking);
        atk = false;
    }
    // 攻撃中の処理.
    void Attacking()
	{
        // ChangeState(State.Walking);
        if (Input.GetButtonDown("Fire1")&& chein ==true&& status.chein==false==status.stamina>=10)
        {
            status.chein = true;
        }
        if (charaAnimation.IsAttacked())
        {
            ChangeState(State.Walking);
            atk = false;
         
        }
	}
    void GurdStart()
    {
        StateStartCommon();
        status.guard = true;

        // 敵の方向に振り向かせる.
        //	Vector3 targetDirection = (attackTarget.position - transform.position).normalized;
        //	SendMessage("SetDirection", targetDirection);

        // 移動を止める.
        SendMessage("StopMove");
    }
    void Gurding()
    {
            status.stamina -= 0.1f;
        if (status.stamina <= 0)
        {

            status.guardBreak = true;
            status.stamina = 0;
            status.guardBreakTime = 100;
            ChangeState(State.Walking);

        }
        if (charaAnimation.IsGurded())
        {
            ChangeState(State.Walking);
            
            ch = true;

        }


        // ChangeState(State.Walking);
        /*
        if (charaAnimation.IsAttacked())
        {
            ChangeState(State.Walking);
           // atk = false;

        }*/
    }
    void Died()
	{
		status.died = true;
		gameRuleCtrl.GameOver();

		// オーディオの再生.
		deathSeAudio.Play ();
	}
	
	void Damage(AttackArea.AttackInfo attackInfo)
	{

        if (status.dodgeActive == true)
        {


        }

        else if (state==State.Gurding) {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            effect.transform.localPosition = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
            Destroy(effect, 0.3f);
            double unko= attackInfo.attackPower * 0.1;
            status.HP -= (int)unko;
            status.stamina -= attackInfo.attackPower/10;
            if (status.stamina <= 0)
            {

                status.guardBreak = true;
                status.stamina = 0;
                status.guardBreakTime = 100;

    // 体力０なので死亡ステートへ.
    //   ChangeState(State.Died);
}
            if (status.HP <= 0)
            {
                status.HP = 0;
                // 体力０なので死亡ステートへ.
                ChangeState(State.Died);
            }


        }

        
        else if (status.guard == false)
        {
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity) as GameObject;
            effect.transform.localPosition = transform.position + new Vector3(0.0f, 0.5f, 0.0f);
            Destroy(effect, 0.3f);

            status.HP -= attackInfo.attackPower;
            if (status.HP <= 0)
            {
                status.HP = 0;
                // 体力０なので死亡ステートへ.
                ChangeState(State.Died);
            }
        }
	}
	
	// ステートが始まる前にステータスを初期化する.
	void StateStartCommon()
	{
		status.attacking = false;
		status.died = false;
        status.guard = false;
        status.attacking2 = false;
        status.attacking_num = 0;
    }
}

