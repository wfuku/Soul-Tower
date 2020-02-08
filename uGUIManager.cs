using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class uGUIManeger : MonoBehaviour
{
    GameRuleCtrl gameRuleCtrl;
    public Text Timer;

    void Awake()
    {
        gameRuleCtrl = GameObject.FindObjectOfType(typeof(GameRuleCtrl)) as GameRuleCtrl;
    }
    void Update()
    {
        Timer.text = (gameRuleCtrl.timeRemaining.ToString("0"));
    }
}
