using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToScene : SingletonMono<FadeToScene>
{
    // CanvasGroup canvasGroup;
    // //���뽥��ʱ��
    // public float fadeInDuration;
    // public float fadeOutDuration;
    // private void Awake()
    // {
    //     canvasGroup = GetComponent<CanvasGroup>();
    //     DontDestroyOnLoad(gameObject);

    // }
    // //���뵭��             �ڳ�����������е���
    // public IEnumerator FadeInOut()
    // {
    //     yield return FadeOut(fadeOutDuration);
    //     yield return FadeIn(fadeInDuration);

    // }
    // //����
    // public IEnumerator FadeOut(float time)
    // {
    //     while (canvasGroup.alpha < 1)
    //     {
    //         canvasGroup.alpha += Time.deltaTime / time;
    //         yield return null;
    //     }
    // }
    // //����
    // public IEnumerator FadeIn(float time)
    // {
    //     while (canvasGroup.alpha != 0)
    //     {
    //         canvasGroup.alpha -= Time.deltaTime / time;
    //         yield return null;
    //     }
    // }
}
