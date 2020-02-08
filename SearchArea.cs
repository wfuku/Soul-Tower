using UnityEngine;
using System.Collections;

public class SearchArea : MonoBehaviour {
	public EnemyCtrl enemyCtrl;

	void Start()
	{
    }
	
	void OnTriggerStay( Collider other )
	{
        // Playerタグをターゲットにする
        if (other.tag == "Player")
        {
            enemyCtrl.SetAttackTarget(other.transform);
        }
	}
}
