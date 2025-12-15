using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PackageTable", menuName = "Package/Table")]
public class PackageItemTable : ScriptableObject
{

    [SerializeField]
    private List<PackageItemTableInfo> packageItemList = new List<PackageItemTableInfo>();
    private Dictionary<int, PackageItemTableInfo> packageItemDic;


    public PackageItemTableInfo GetPackageItemInfo(ItemName itemName)
    {
        if (packageItemDic == null)
        {
            packageItemDic = new Dictionary<int, PackageItemTableInfo>();
            foreach (var item in packageItemList)
            {
                if (!packageItemDic.ContainsKey((int)item.IdAndName))
                {
                    packageItemDic.Add(((int)item.IdAndName), item);
                }
            }
        }
        if (packageItemDic == null)
        {
            Debug.LogError("背包物品信息表配置为空,背包物品信息字典初始化失败");
            return null;
        }
        int id = (int)itemName;
        if (packageItemDic.TryGetValue(id, out var getItem))
        {
            return getItem;
        }
        return null;
    }

}

public enum ItemName
{
    Wood,
    Ruby,
    SilverBreastplate


}


public enum ItemType
{
    Armor,
    Weapon,
    Materials,

}

public enum ItemRarity
{
    Common,
    Uncommon,
    //优秀
    Rare,
    Epic,
    Legendary,
    //神话
    Mythic

}



[System.Serializable]
public class PackageItemTableInfo
{
    public ItemName IdAndName;
    [Header("物品属于的种类")]
    public ItemType itemType;
    [Header("物品的描述")]
    public string description;
    [Header("物品图片")]
    public Sprite sprite;
}


