using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG;
using DG.Tweening;

public class TestTrigger : MonoBehaviour
{
    [Header("被控制的文本")]
    public TextMeshProUGUI triggerText;
    public static bool isShow;
    [Header("所需显示文本")]
    public string text;
    [Header("字体资产路径")]
    public string path;
    public GameObject textGameObject;
    [Header("文本颜色")]
    [SerializeField]
    private Color textColor = Color.clear;
    private bool isExit = false;
    private float time;
    public bool isEnterTrigger = true;


    private void Start()
    {
        textColor = triggerText.color;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isEnterTrigger)
        {
            isShow = true;
            isExit = false;
            textGameObject.SetActive(true);
            triggerText.font = Resources.Load<TMP_FontAsset>(path);
            triggerText.color = textColor;
            // 使用DOTween.To进行插值
            DOTween.To(() => 0, (x) =>
            {
                // 根据当前进度x，截取字符串的一部分
                int length = (int)(text.Length * x);
                triggerText.text = text.Substring(0, length);
            },
                1f, // 目标值为1（100%的文本）
                1f) // 持续3秒
              .SetEase(Ease.Linear);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isEnterTrigger)
        {
            triggerText.color = textColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isShow = false;
            isExit = true;
            triggerText.DOFade(0, .3f).OnComplete(() => { if (!isShow) textGameObject.SetActive(false); });
        }
    }
}
