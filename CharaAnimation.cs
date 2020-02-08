using UnityEngine;

public class CharaAnimation : MonoBehaviour
{
	Animator animator;
	CharacterStatus status;
    public Transform Cam;
    Vector3 prePosition;//前フレームの座標を格納


    bool isDown = false;//落下中
    public bool guardFlag;//ガード中
    //行動後の処理用フラグ.
    public bool attacked = false; 
    public bool defenceed = false;

    

    public bool IsAttacked()
	{
		return attacked;
	}
    public bool IsGurded()
    {
        if (defenceed == true) {
            status.guard = false;
            guardFlag = false;
        }
        return defenceed;

    }
    void DodgeStart()
    {
        //回避以外のアニメーションのフラグを偽にする.
        status.guard = false;
        status.attacking = false;
        status.chein = false;
        animator.SetBool("Attacking", false);
        animator.SetBool("Chein", false);
        attacked=true;
        status.dodgeActive = true;

        
        status.stamina -= 40;
        //スタミナ消費が0以下になったらガードブレイク状態に移行
        if (status.stamina <= 0)
        {

            status.guardBreak = true;
            status.stamina = 0;
            status.guardBreakTime = 100;

        }

    }
    void DodgeEnd()
    {
        //回避のアニメーションのフラグを偽にする.
        animator.SetBool("Dodge",false);
        status.dodge = false;
        status.dodgeActive = false;
    }

    void StartAttackHit()
	{
        //攻撃開始、スタミナ消費後が0でもガードブレイクにはならない.
        status.stamina -= 10;
        if (status.stamina <= 0)
        {
            status.stamina = 0;

        }
        animator.applyRootMotion = true;
        
    }

    void EndAttackHit()
	{
        //攻撃判定終了時に連続攻撃を続けるかの判定を行う.
        if (tag == "Player"){
            if (status.chein == true)
            {
                animator.SetBool("Chein", true);
            }
            else {
                animator.SetBool("Chein", false);
            }
            status.chein = false;
        }
	}
    void FinishAttack() {
        //攻撃の終了処理を行う
        animator.SetBool("Chein", false);
        attacked = true;
        status.attacking = false;

    }

    void EndAttack()
	{

        if (tag != "Player") {
            attacked = true;
        }

     
        else {
                
            if (animator.GetBool("Chein") == false) {
                attacked = true;
                status.attacking = false;
            }
        }
        animator.applyRootMotion = false;
    }
    void set()
    {
        animator.speed = 0;
    }
    void EndGurd()
    {
        defenceed = true;
    }
    void BattouNow()
    {
        status.battouNow = true;
    }
    void BattouEnd()
    {
        status.battouNow = false;
    }
    void Start ()
	{
		animator = GetComponent<Animator>();
		status = GetComponent<CharacterStatus>();
		prePosition = transform.position;
	}

    void Update ()
	{
        //前フレームの座標の差分を時間の差分で割ることにより滑らかなアニメーションができる
		Vector3 delta_position = transform.position - prePosition;
		animator.SetFloat("Speed", delta_position.magnitude / Time.deltaTime);
        //回避のフラグが真なら回避アニメーションのフラグを真にする.
        if (status.dodge == true) {
            animator.SetBool("Dodge", true);
        }
        //攻撃後のフラグが偽なら攻撃アニメーションのフラグも偽にする.
        if (attacked &&animator.GetBool("Attacking")==false)
		{
			attacked = false;
		}
        //ガードを入力してないならガードを解除する
        if (defenceed && !status.guard)
        {
            defenceed = false;
        }
        //攻撃中ではなく、攻撃をしようとしたら攻撃アニメーション開始
        animator.SetBool("Attacking", (!attacked && status.attacking) && status.attacking ==true);
        //プレイヤーでないなら
        if (tag != "Player") {
            if (status.longAttackHave==true) {
                animator.SetInteger("AttackNumber",status.attackNumber);
                    }
        }
        else if (tag == "Player")
        {
            animator.SetBool("Battou", status.battou);//抜刀の状態に合わせてアニメーションをセット

            //抜刀中は画面の向きにキャラクターを向かせる
            if(status.battou) {  transform.rotation = Quaternion.Euler(0, Cam.rotation.eulerAngles.y, 0); }
            //ガード中はアニメーションスピードを変える
            animator.SetBool("Gurd", (!defenceed && status.guard));
            if (guardFlag == true)
            {
                animator.speed = 1;
            }
            if (Input.GetButtonUp("Fire2") || status.stamina <= 0 && defenceed == false)
            {
                guardFlag = true;

            }
        }
        //落下中
        if (!isDown && status.died)
		{
			isDown = true;
			animator.SetTrigger("Down");
		}
		//差分の座標を保存
		prePosition = transform.position;
	}
}