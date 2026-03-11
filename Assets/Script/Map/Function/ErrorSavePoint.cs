using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorSavePoint : MonoBehaviour, IInteraction
{
    [SerializeField]
    private GameObject light;
    private bool isGlint = false;
    private float time;
    [Header("ÉÁË¸ÆµÂÊ")]
    [SerializeField]
    private float frequency;
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        if (isGlint && time >= frequency)
        {
            time = 0;
            light.SetActive(!light.activeSelf);
        }
    }

    public void ButtonPress()
    {
        isGlint = true;
        time = 0;

    }
}
