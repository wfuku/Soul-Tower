using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
// キャラクターを移動させる。
// Chapter3
public class CharacterMove : MonoBehaviour {
	// 重力値.
	const float GravityPower = 9.8f; 
	//　目的地についたとみなす停止距離.
	const float StoppingDistance = 0.6f;
	
	// 現在の移動速度.
	Vector3 velocity = Vector3.zero; 
	// キャラクターコントローラーのキャッシュ.
	CharacterController characterController; 
	// 到着したか（到着した true/到着していない false)
	public bool arrived = false; 
	
	// 向きを強制的に指示するか.
	bool forceRotate = false;
	
	// 強制的に向かせたい方向.
	Vector3 forceRotateDirection;
	
	// 目的地.
	public Vector3 destination; 
	
	// 移動速度.
    float walkSpeed = 6.0f;
    public float run = 8.0f;
    public float walk = 6.0f;

    // 回転速度.
    public float rotationSpeed = 360.0f;
    
    // カメラ
    public Transform Cam;

    // 移動方向のアニメーション用
    public int WASD;

    // アニメーション
    Animator animator;

    // 斜面の滑り
    bool isSliding;
    public bool notSliding;
    bool falling;
    // 斜面判別用lay
    RaycastHit slideHit;
    LayerMask layerMask;
    Hashtable table = new Hashtable();//lay用マスク

    //参照用
    public PlayerCtrl playerCtrl;
    CharacterStatus status;
    
    void OnTriggerEnter(Collider other)
    {
        //橋に触れた際に落下の挙動が始まる為、落下の処理を停止させることにより一応動作可能(修正予定)
        if (other.gameObject.tag == "Hashi")
        {
            notSliding = true;
        }
        if (other.gameObject.tag == "UnderGround"&&tag=="Player") {
            animator.SetBool("Fall", true);
            isSliding = true;
            falling = true;
            RenderSettings.fogDensity = 0.22f;
            RenderSettings.fogColor=new Color32(170,192,229,255);

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hashi")
        {
            notSliding = false;
        }

    }

    void DodgeStart()
    {
        //回避はキャラクタを後ろに下げる(プレイヤー、敵両方に付けるスクリプトの為回避力設定の処理は保留)
        velocity.x = -transform.forward.x * 20;
        velocity.z = -transform.forward.z * 20;
    }

    void Start () {
        //初期化
		characterController = GetComponent<CharacterController>();
		destination = transform.position;
        playerCtrl = GetComponent<PlayerCtrl>();
        status = GetComponent<CharacterStatus>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update () {
        //抜刀時で移動速度を変える
        if (status.battou) {
            walkSpeed = walk;
        }
        else 
        {
            walkSpeed = run;
        }
        // 地面のレイヤーを判別する
        int layerMask = 1 << LayerMask.NameToLayer("Ground");

        //橋以外の斜面では滑らせる
        if (Physics.Raycast(transform.position, Vector3.down, out slideHit, 30, layerMask)&&notSliding == false&&tag!="Hashi"&&falling==false)
        {
            if (notSliding == false)
            {              
                    //衝突した際の面の角度とが滑らせたい角度以上かを調べる
                    if (Vector3.Angle(slideHit.normal, Vector3.up) > characterController.slopeLimit)
                    {
                        isSliding = true;

                    }
                    else {
                        isSliding = false;
                    }
                    if (tag == "Player") {
                    animator.SetBool("Fall", isSliding);
                    }                
            }
        }
        if (isSliding)
        {//滑るフラグが立ってたら
            Vector3 hitNormal = slideHit.normal;
            velocity.x = hitNormal.x * 10;
            velocity.y -= 9.8f * Time.deltaTime;//重力落下
            velocity.z = hitNormal.z * 10;
        }
        // 移動速度velocityを更新する
        if (characterController.isGrounded&&isSliding ==false) {
			//　水平面での移動を考えるのでXZのみ扱う.
			Vector3 destinationXZ = destination; 
			destinationXZ.y = transform.position.y;// 高さを目的地と現在地を同じにしておく.
			
			//********* ここからXZのみで考える. ********
			// 目的地までの距離と方角を求める.
			Vector3 direction = (destinationXZ - transform.position).normalized;
			float distance = Vector3.Distance(transform.position,destinationXZ);
			
			// 現在の速度を退避.
			Vector3 currentVelocity = velocity;
			
			//　目的地にちかづいたら到着..
			if (arrived || distance < StoppingDistance)
				arrived = true;
			
			
			// 移動速度を求める.
			if (arrived)
				velocity = Vector3.zero;
			else 
				velocity = direction * walkSpeed;
			
			
			// スムーズに補間.
			velocity = Vector3.Lerp(currentVelocity, velocity,Mathf.Min (Time.deltaTime * 5.0f ,1.0f));
			velocity.y = 0;

            // 移動用基準の角度割り出し
            float rad = Mathf.Deg2Rad * variable.Instance.direction.transform.rotation.eulerAngles.y;
            float x = Mathf.Sin((rad)) * walkSpeed;
            float z = Mathf.Cos((rad)) * walkSpeed;

            if (gameObject.tag == "Player"&&status.battouNow==false)
            {
                //回避
                if (Input.GetKey(KeyCode.Space) && status.guardBreak == false && status.dodge == false&&status.battou==true)
                {
                    status.dodge = true;
                }
                //納刀時の移動
                if (gameObject.tag == "Player" && status.guard == false && playerCtrl.atk == false && status.died == false&&status.battou==false)
                {
                    
                    float cam_y = variable.Instance.direction.transform.rotation.eulerAngles.y;
                    //入力で移動するベクトルを決める
                    WASD = 0;//アニメーション番号(switch文で書くか検討中)
                    if (Input.GetKey(KeyCode.W))
                    {
                        velocity.x = x;
                        velocity.z = z;
                        transform.rotation = Quaternion.Euler(0, cam_y, 0);
                        WASD = 1;
                    }

                    else if (Input.GetKey(KeyCode.S))
                    {
                        velocity.x = -x;
                        velocity.z = -z;
                        transform.rotation = Quaternion.Euler(0, cam_y - 180, 0);
                        WASD = 1;
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        velocity.x = z;
                        velocity.z = x * -1;
                        transform.rotation = Quaternion.Euler(0, cam_y - 270, 0);
                        WASD = 1;
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        velocity.x = z * -1;
                        velocity.z = x;
                        transform.rotation = Quaternion.Euler(0, cam_y - 90, 0);
                        WASD = 1;
                    }

                    if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
                    {
                        float deg = 315 * Mathf.Deg2Rad;/// (Mathf.PI * 180);
                        velocity.x = Mathf.Cos(deg) * x + Mathf.Sin(deg) * -1 * z;
                        velocity.z = Mathf.Sin(deg) * x + Mathf.Cos(deg) * z;
                        
                        transform.rotation = Quaternion.Euler(0, cam_y - 315, 0);
                        WASD = 1;
                    }
                    else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
                    {
                        float deg = 45 * Mathf.Deg2Rad;// (Mathf.PI*180);
                        velocity.x = Mathf.Cos(deg) * x + Mathf.Sin(deg) * -1 * z;
                        velocity.z = Mathf.Sin(deg) * x + Mathf.Cos(deg) * z;
                        transform.rotation = Quaternion.Euler(0, cam_y - 45, 0);
                        WASD = 1;
                    }
                    else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
                    {
                        float deg = 135 * Mathf.Deg2Rad;// (Mathf.PI * 180);
                        velocity.x = Mathf.Cos(deg) * x + Mathf.Sin(deg) * -1 * z;
                        velocity.z = Mathf.Sin(deg) * x + Mathf.Cos(deg) * z;
                        transform.rotation = Quaternion.Euler(0, cam_y - 135, 0);
                        WASD = 1;
                    }
                    else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
                    {
                        float deg = 225 * Mathf.Deg2Rad;// (Mathf.PI * 180);
                        velocity.x = Mathf.Cos(deg) * x + Mathf.Sin(deg) * -1 * z;
                        velocity.z = Mathf.Sin(deg) * x + Mathf.Cos(deg) * z;
                        transform.rotation = Quaternion.Euler(0, cam_y - 225, 0);
                        WASD = 1;
                    }
                    
                    animator.SetInteger("WASD", WASD);
                    //  direction = new Vector3(0,45,0);
                }
                //納刀時の移動
                else if (status.guard == false && playerCtrl.atk == false && status.died == false && status.dodge == false)
                {
                    transform.rotation = Quaternion.Euler(0, Cam.rotation.eulerAngles.y, 0);

                        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.W))
                    {
                        float deg = 315 * Mathf.Deg2Rad;/// (Mathf.PI * 180);
                        velocity.x = Mathf.Cos(deg) * x + Mathf.Sin(deg) * -1 * z;
                        velocity.z = Mathf.Sin(deg) * x + Mathf.Cos(deg) * z;
                        WASD = 9;
                    }
                    else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
                    {
                        float deg = 45 * Mathf.Deg2Rad;// (Mathf.PI*180);
                        velocity.x = Mathf.Cos(deg) * x + Mathf.Sin(deg) * -1 * z;
                        velocity.z = Mathf.Sin(deg) * x + Mathf.Cos(deg) * z;                    
                        WASD = 7;
                    }
                    else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
                    {
                        float deg = 135 * Mathf.Deg2Rad;// (Mathf.PI * 180);
                        velocity.x = Mathf.Cos(deg) * x + Mathf.Sin(deg) * -1 * z;
                        velocity.z = Mathf.Sin(deg) * x + Mathf.Cos(deg) * z;                     
                        WASD = 1;
                    }
                    else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
                    {
                        float deg = 225 * Mathf.Deg2Rad;// (Mathf.PI * 180);
                        velocity.x = Mathf.Cos(deg) * x + Mathf.Sin(deg) * -1 * z;
                        velocity.z = Mathf.Sin(deg) * x + Mathf.Cos(deg) * z;
                        WASD = 3;
                    }
                   else  if (Input.GetKey(KeyCode.W))
                    {
                        velocity.x = x;
                        velocity.z = z;
                        WASD = 8;
                    }

                    else if (Input.GetKey(KeyCode.S))
                    {
                        velocity.x = -x;
                        velocity.z = -z;                       
                        WASD = 2;
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        velocity.x = z;
                        velocity.z = x * -1;
                        WASD = 6;
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        velocity.x = z * -1;
                        velocity.z = x;
                        WASD = 4;
                    }
                    else { WASD = 0; }
                    animator.SetInteger("WASD", WASD);
                }
            }
            

            if (!forceRotate) {
				// 向きを行きたい方向に向ける.
				if (velocity.magnitude > 0.1f && !arrived) { // 移動してなかったら向きは更新しない.
					Quaternion characterTargetRotation = Quaternion.LookRotation(direction);
					transform.rotation = Quaternion.RotateTowards(transform.rotation,characterTargetRotation,rotationSpeed * Time.deltaTime);
				}
			} else {
				// 強制向き指定.
				Quaternion characterTargetRotation = Quaternion.LookRotation(forceRotateDirection);
				transform.rotation = Quaternion.RotateTowards(transform.rotation,characterTargetRotation,rotationSpeed * Time.deltaTime);
			}
			
		}
		
		// 重力.
		velocity += Vector3.down * GravityPower * Time.deltaTime;
		
		// 接地していたら思いっきり地面に押し付ける.
		// (UnityのCharactorControllerの特性のため）
		Vector3 snapGround = Vector3.zero;
		if (characterController.isGrounded)
			snapGround = Vector3.down;
		
		// CharacterControllerを使って動かす.
		characterController.Move(velocity * Time.deltaTime+snapGround);
		
		if (characterController.velocity.magnitude < 0.1f)
			arrived = true;
		
		// 強制的に向きを変えるを解除.
		if (forceRotate && Vector3.Dot(transform.forward,forceRotateDirection) > 0.99f)
			forceRotate = false;
		
		
	}
	
	// 目的地を設定する.引数destinationは目的地.
	public void SetDestination(Vector3 destination)
	{
		arrived = false;
		this.destination = destination;
	}
	
	// 指定した向きを向かせる.
	public void SetDirection(Vector3 direction)
	{
		forceRotateDirection = direction;
		forceRotateDirection.y = 0;
		forceRotateDirection.Normalize();
		forceRotate = true;
	}
	
	// 移動をやめる.
	public void StopMove()
	{
		destination = transform.position; // 現在地点を目的地にしてしまう.
	}
	
	// 目的地に到着したかを調べる. true　到着した/ false 到着していない.
	public bool Arrived()
	{
		return arrived;
	}
	
	
}
