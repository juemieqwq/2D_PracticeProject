using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.Mathematics;
using System.Linq.Expressions;

public class PackageItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private GameObject BGC_Common;
    [SerializeField]
    private GameObject BGC_Uncommon;
    [SerializeField]
    private GameObject BGC_Rare;
    [SerializeField]
    private GameObject BGC_Epic;
    [SerializeField]
    private GameObject BGC_Legendary;
    [SerializeField]
    private GameObject BGC_Mythic;
    [SerializeField]
    public GameObject beChecked;
    [SerializeField]
    private GameObject image;
    [SerializeField]
    private GameObject nameText;
    [SerializeField]
    private GameObject numText;

    private bool isInit = false;
    private bool isShow = false;
    //鼠标是否在物体上
    private bool mouseIsOnItem;

    private Sprite sprite;
    public ItemRarity itemRarity { get; private set; }
    private string itemName;
    [HideInInspector]
    [Range(0, 999)]
    private int num;
    private PackagePanel hostPanel;

    private void Start()
    {

    }

    private void Update()
    {
        if (!isInit)
            return;

    }

    public void Init(ItemName itemName, ItemRarity itemRarity, PackagePanel packagePanel)
    {
        var itemStaticInfo = UIManager.Instance.packageItemTable.GetPackageItemInfo(itemName);
        this.sprite = itemStaticInfo.sprite;
        this.itemRarity = itemRarity;
        this.itemName = itemStaticInfo.IdAndName.ToString();
        hostPanel = packagePanel;
        num = 1;
        mouseIsOnItem = false;
        isInit = true;
        InitShow();
    }

    private void InitShow()
    {
        if (!isInit && !isShow)
            return;
        switch (itemRarity)
        {
            case ItemRarity.Common:
                BGC_Common.SetActive(true); break;
            case ItemRarity.Uncommon:
                BGC_Uncommon.SetActive(true); break;
            case ItemRarity.Rare:
                BGC_Rare.SetActive(true); break;
            case ItemRarity.Epic:
                BGC_Epic.SetActive(true); break;
            case ItemRarity.Legendary:
                BGC_Legendary.SetActive(true); break;
            case ItemRarity.Mythic:
                BGC_Mythic.SetActive(true); break;
        }
        image.GetComponent<Image>().sprite = sprite;
        nameText.GetComponent<TextMeshProUGUI>().text = itemName;
        numText.GetComponent<TextMeshProUGUI>().text = string.Format("X{0}", num);
        isShow = true;
    }

    public void AddItemNum(int addNum = 1)
    {
        this.num += addNum;
        numText.GetComponent<TextMeshProUGUI>().text = string.Format("X{0}", num);
    }

    public void SetItemNum(int Num)
    {
        this.num = Num;
        numText.GetComponent<TextMeshProUGUI>().text = string.Format("X{0}", num);
    }

    public void BeChecked()
    {
        hostPanel.CancelBeChecked();
        this.beChecked.SetActive(true);
    }

    public PackageItemStaticInfo CreateCanSerializableInfo()
    {
        PackageItemStaticInfo info = new PackageItemStaticInfo();
        info.itemRarity = this.itemRarity;
        info.itemNum = this.num;
        info.itemName = this.itemName;
        return info;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOnItem = false;
        StopCoroutine("SetScale");
        StartCoroutine(SetScale(1, Vector3.one));
        UnityEngine.Debug.Log("鼠标退出:" + this.itemName);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOnItem = true;
        StopCoroutine("SetScale");
        StartCoroutine(SetScale(1, new Vector3(1.5f, 1.5f, 1.5f)));
        UnityEngine.Debug.Log("鼠标进入:" + this.itemName);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BeChecked();
        UnityEngine.Debug.Log(this.itemName + "被点击");
    }

    public IEnumerator SetScale(float duration, Vector3 targetScale)
    {
        float scaleTime = 0;
        Vector3 originScale = transform.localScale;
        while (scaleTime < duration)
        {
            scaleTime += 0.0625f;
            transform.localScale = Vector3.Lerp(originScale, targetScale, scaleTime / duration);
            UnityEngine.Debug.Log("当前缩放：" + scaleTime);
            yield return null;
        }
        transform.localScale = targetScale;
        //StopCoroutine("MouseStop");
        //StartCoroutine("MouseStop");
    }

    public IEnumerator MouseStop()
    {
        UnityEngine.Debug.Log("开始浮动");
        // 浮动参数
        float hoverHeight = 2f;    // 浮动高度
        float hoverSpeed = 2.0f;     // 浮动速度
        float rotationAngle = 15f;   // 旋转角度

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float hoverTimer = 0f;
        if (mouseIsOnItem)
        {
            // 更新计时器
            hoverTimer += 0.0625f * hoverSpeed;

            // 使用正弦波计算Y轴偏移（-1到1之间）
            float yOffset = Mathf.Sin(hoverTimer) * hoverHeight;

            // 应用位置浮动
            transform.position = startPosition + new Vector3(0, yOffset, 0);

            //// 可选：添加轻微旋转
            //float rotationOffset = Mathf.Sin(hoverTimer * 0.5f) * rotationAngle;
            //transform.rotation = startRotation * Quaternion.Euler(0, rotationOffset, 0);
            yield return null;
        }
    }

}

[System.Serializable]
public class PackageItemStaticInfo
{
    public string itemName;
    public ItemRarity itemRarity;
    public int itemNum;
}
