using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboradAnimator : MonoBehaviour
{
    public Sprite[] sprites;
    [Header("多少秒播放下一张图片")]
    public float playFrame;
    private Image image;
    private bool isPlay;
    private float time;
    private int currentIndex;

    private void Start()
    {
        image = GetComponent<Image>();
        image.sprite = sprites[0];
        currentIndex = 0;
        time = 0;
        isPlay = true;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= playFrame && isPlay)
        {
            if (currentIndex >= sprites.Length - 1)
            {
                currentIndex = 0;
            }
            else
                currentIndex++;
            time = 0;
            image.sprite = sprites[currentIndex];
        }
    }

    public void SetSprite(int index, bool isPlay = false)
    {
        image.sprite = sprites[index];
        this.isPlay = isPlay;
    }
}
