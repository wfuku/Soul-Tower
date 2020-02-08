using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveWorld : MonoBehaviour {

    public Scene Home,Stage1,Stage2,Stage3;
    public FadeGame fade;
    public float fadeSpeed;

    public bool Movepoint = false;
    // シーン移行時間
    public float sceneChangeTime = 1.0f;
    public int sceneNumber;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // ゲーム終了条件成立後、シーン遷移
        if (Movepoint)
        {
            sceneChangeTime -= Time.deltaTime;
            if (sceneChangeTime <= 0.0f)
            {
                SceneManager.LoadScene(sceneNumber);
                
            }
            return;
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {      
            Movepoint = true;
            fade.fadeout=true;
            fade.fadeSpeed = fadeSpeed;
        }
    }
}
