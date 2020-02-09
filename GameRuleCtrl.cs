using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameRuleCtrl : MonoBehaviour {
    // 残り時間
    public float timeRemaining = 5.0f * 60.0f;
	// ゲームオーバーフラグ
	public bool gameOver = false;
	// ゲームクリア
    public bool gameClear = false;
	// シーン移行時間
	public float sceneChangeTime = 6.0f;

	public AudioClip clearSeClip;
	AudioSource clearSeAudio;

    // フェード用
    public FadeGame fade;
    public float fadeSpeed;
    //FPS制限
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    void Start()
	{
        // オーディオの初期化.
        clearSeAudio = gameObject.AddComponent<AudioSource>();
		clearSeAudio.loop = false;
		clearSeAudio.clip = clearSeClip;
	}

	void Update()
	{
        // ゲーム終了条件成立後、シーン遷
        if ( gameOver || gameClear ){
            sceneChangeTime -= Time.deltaTime;
			if( sceneChangeTime <= 0.0f ){
                SceneManager.LoadScene("Home");
			}
			return;
		}

		timeRemaining -= Time.deltaTime;
		// 残り時間が無くなったらゲームオーバー
		if(timeRemaining<= 0.0f ){
			GameOver();
		}

	}
	
	public void GameOver()
	{
        fade.fadeSpeed = fadeSpeed;
        fade.fadeout=true;
		gameOver = true;
	}
	public void GameClear()
	{
        fade.fadeSpeed = fadeSpeed;
        fade.fadeout = true;

        gameClear = true;

		// オーディオ再生.
		clearSeAudio.Play ();
	}
}
