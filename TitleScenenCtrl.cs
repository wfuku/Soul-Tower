using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleScenenCtrl : MonoBehaviour {
    public float sceneChangeTime = 3.0f;
    public AudioClip clickSeClip;
    AudioSource clickSeAudio;

    void Update()
    {
        sceneChangeTime -= Time.deltaTime;
        if (sceneChangeTime <= 0.0f)
        {
            SceneManager.LoadScene("Home");
        }
    }
        public void GameStart()
    {
        // オーディオの初期化.
        clickSeAudio = gameObject.AddComponent<AudioSource>();
        clickSeAudio.loop = false;
        clickSeAudio.clip = clickSeClip;
        // オーディオの再生.
        clickSeAudio.Play();
        GetComponent< TitleScenenCtrl>().enabled = true;
    }
}
