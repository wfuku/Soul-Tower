using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeGame : MonoBehaviour
{
    public float fadeSpeed;
    float red, green, blue,alfa;
    public bool fadein,fadeout;

    void Start()
    {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
        alfa = GetComponent<Image>().color.a;
    }

    void FixedUpdate()
    {
        //徐々に画面を覆う画像の不透明度を変えることでフェードを表現できる
        if (fadeout && alfa < 1) {
            FadeOut(fadeSpeed);
        }
        else if (fadein && alfa > 0){
            FadeIn(fadeSpeed);
        }
    }

    public void FadeIn(float speed) {
        if (alfa > 0){
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa -= 1f/speed;
        }
        
    }
    public void FadeOut(float speed)
    {
        if (alfa < 1){
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa += 1f / speed;
        }
        
    }

}