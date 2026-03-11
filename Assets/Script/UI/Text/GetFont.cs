using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GetFont : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textGameObject;
    [SerializeField]
    private string path;
    private void OnEnable()
    {
        textGameObject.font = Resources.Load<TMP_FontAsset>(path);
    }
}
