using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    float alfa;
    float speed = 0.01f;
    float red, green, blue;

    void Start()
    {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
    }
    //試作の為フェードを二種作った

    void Update()
    {
            GetComponent<Image>().color = new Color(red, green, blue, alfa);
            alfa += speed;
    }
    public void Fade()
    {
        GetComponent<FadeScript>().enabled = true;
    }
}