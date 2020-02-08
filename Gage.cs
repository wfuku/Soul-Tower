using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gage : MonoBehaviour {
    public Image life;
    public Image stamina;
    public CharacterStatus status;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //ゲージの表記用
        life.fillAmount = (float)status.HP/ (float)status.MaxHP;
        stamina.fillAmount = (float)status.stamina / (float)status.MAXstamina;
    }
}
