using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;
using System;

public class PackagePanel : BasePanel
{


    private GameObject colsePanelButton;
    [SerializeField]
    public GameObject content;
    [SerializeField]
    private GameObject itemPrefab;

    [HideInInspector]
    public List<PackageItem> packageItemsList = new List<PackageItem>();
    [HideInInspector]
    public Dictionary<int, PackageItem[]> packageItemsDic = new Dictionary<int, PackageItem[]>();
    private List<PackageItemStaticInfo> packageItemStaticInfoList;
    private string hasKeyName;
    private bool isFirstOpen;

    private void Start()
    {
        hasKeyName = "LocalItemsData";
        colsePanelButton = GameObject.Find("ClosePanel");
        InitPackagePanel();

    }

    public override void ClosePanel()
    {
        Time.timeScale = 1.0f;
        CancelBeChecked();
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0, 0.4f).OnComplete(() => base.ClosePanel());
    }

    public override void OpenPanel(string name)
    {
        canvasGroup = GetComponentInParent<CanvasGroup>();
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1, 0.2f).OnComplete(() => base.OpenPanel(name));
    }



    public void AddItem(ItemName itemName, ItemRarity itemRarity)
    {
        if (packageItemsDic.TryGetValue((int)itemName, out var itemList))
        {
            if (itemList[(int)itemRarity] != null)
            {
                itemList[(int)itemRarity].AddItemNum();
                return;
            }
        }
        var itemStaticInfo = UIManager.Instance.packageItemTable.GetPackageItemInfo(itemName);
        var itemClass = Instantiate(itemPrefab, content.transform).GetComponent<PackageItem>();
        itemClass.Init(itemName, itemRarity, this);
        packageItemsList.Add(itemClass);
        PackageItem[] items = new PackageItem[(int)ItemRarity.Mythic];
        items[(int)itemRarity] = itemClass;
        packageItemsDic.Add((int)itemName, items);

    }

    public void CancelBeChecked()
    {
        foreach (var item in packageItemsList)
        {
            item.beChecked.SetActive(false);
        }
    }


    private void InitPackagePanel()
    {
        packageItemStaticInfoList = GetLocalItemData();
        if (packageItemStaticInfoList != null)
        {
            foreach (var itemInfo in packageItemStaticInfoList)
            {
                var itemClass = Instantiate(itemPrefab, content.transform).GetComponent<PackageItem>();
                itemClass.Init((ItemName)Enum.Parse(typeof(ItemName), itemInfo.itemName), itemInfo.itemRarity, this);
                itemClass.SetItemNum(itemInfo.itemNum);
                packageItemsList.Add(itemClass);
                PackageItem[] items = new PackageItem[(int)ItemRarity.Mythic];
                items[(int)itemInfo.itemRarity] = itemClass;
                packageItemsDic.Add((int)(ItemName)Enum.Parse(typeof(ItemName), itemInfo.itemName), items);
            }
        }
    }


    public List<PackageItemStaticInfo> GetLocalItemData()
    {
        if (PlayerPrefs.HasKey(hasKeyName))
        {
            string localdata = PlayerPrefs.GetString(hasKeyName);
            PackageItemListWrapper localItemData = JsonUtility.FromJson<PackageItemListWrapper>(localdata);
            return localItemData.itemInfos;
        }
        else
        {
            Debug.Log("无本地数据");
        }
        Debug.LogError("无法读取本地数据");
        return null;
    }

    private void SaveToLocal()
    {
        if (packageItemsList != null)
        {
            PackageItemListWrapper packageItemListWrapper = new PackageItemListWrapper();
            foreach (var info in packageItemsList)
            {
                var savaInfo = info.CreateCanSerializableInfo();
                packageItemListWrapper.itemInfos.Add(savaInfo);
            }
            string StringSaveInfos = JsonUtility.ToJson(packageItemListWrapper);
            PlayerPrefs.SetString(hasKeyName, StringSaveInfos);
            PlayerPrefs.Save();
            Debug.Log("保存背包信息");
        }
    }

    //关闭游戏时运行
    private void OnApplicationQuit()
    {
        SaveToLocal();
    }
}


[System.Serializable]
public class PackageItemListWrapper
{
    public List<PackageItemStaticInfo> itemInfos = new List<PackageItemStaticInfo>();
}