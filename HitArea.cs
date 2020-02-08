using UnityEngine;
using System.Collections;

public class HitArea : MonoBehaviour {

    public GameObject rootBody;
    //攻撃されたらメッセージが送られてくるのでそれを本体のスクリプトに伝える
	void Damage(AttackArea.AttackInfo attackInfo)
	{
		rootBody.SendMessage ("Damage",attackInfo);
	}
}
