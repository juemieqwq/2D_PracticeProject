using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Drillmaster : MonoBehaviour, IInteraction
{
    public string firstText;
    public string secondText;
    public TextMeshProUGUI triggerText;
    private int pressNum = 1;
    public GameObject dialogGameObject;
    public GameObject dialogBoxGameObject;
    public Color textColor;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void ButtonPress()
    {
        string text = "";
        if (pressNum <= 1)
        {
            dialogGameObject.SetActive(true);
            dialogBoxGameObject.SetActive(true);
            text = firstText;
        }
        else
        {
            text = secondText;
        }
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
        pressNum++;
    }

    //private void Update()
    //{
    //    if (Keyboard.current.pKey.isPressed)
    //    {
    //        animator.Play("Idle");
    //    }

    //    var animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    if (animatorInfo.normalizedTime > 0.99f)
    //    {
    //        Debug.Log("动画播放完成");
    //    }
    //}
}
