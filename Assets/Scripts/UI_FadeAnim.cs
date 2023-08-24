using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FadeAnim : MonoBehaviour
{
    public Image img;

    void Start()
    {
        img.color = new Color(1, 1, 1, 0);
    }

    IEnumerator FadeIn()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(1, 1, 1, i);
            yield return null;
        }
    }

    public void FadingIn()
    {
        StartCoroutine("FadeIn");
    }

    public void FadingOut()
    {
        StartCoroutine("FadeOut");
    }
}
