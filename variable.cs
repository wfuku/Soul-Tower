using UnityEngine;
using System.Collections;

public class variable : MonoBehaviour
{
    //コピペ　グローバル変数を作るスクリプト、ここで宣言したものは他で使える(非常時以外使用は控える)
    private static variable instance;
    
    public Transform direction;

    
    void Start()
    {

    }
    public static variable Instance
    {
        get
        {
            if (null == instance)
            {
                instance = (variable)FindObjectOfType(typeof(variable));
                if (null == instance)
                {
                    Debug.Log(" DataManager Instance Error ");
                }
            }
            return instance;
        }
    }
    void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("DataManager");
        if (1 < obj.Length)
        {
            // 既に存在しているなら削除
            Destroy(gameObject);
        }
        else {
            // シーン遷移では破棄させない
            DontDestroyOnLoad(gameObject);
        }
    }

}
