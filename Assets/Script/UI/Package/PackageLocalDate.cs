using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageLocalDate
{

    public static PackageLocalDate Instance
    {
        get
        {
            if (instance == null)
                instance = new PackageLocalDate();
            return instance;
        }
    }

    private static PackageLocalDate instance = null;
    private PackageLocalDate()
    {

    }


    public List<PackageLocalItem> items;

    public void SavePackage()
    {
        string inventoryJosn = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PackageLocalDate", inventoryJosn);
        PlayerPrefs.Save();
    }

    public List<PackageLocalItem> LoadPackage()
    {
        if (items != null)
            return items;
        if (PlayerPrefs.HasKey("PackageLocalDate"))
        {
            string inventoryJosn = PlayerPrefs.GetString("PackageLocalDate");
            PackageLocalDate packageLocalDate = JsonUtility.FromJson<PackageLocalDate>(inventoryJosn);
            items = packageLocalDate.items;
            return items;
        }
        else
        {
            items = new List<PackageLocalItem>();
            return items;
        }
    }
}

[System.Serializable]
public class PackageLocalItem
{
    public string uid;
    public int id;
    public int num;
    public int level;
    public bool isNew;
    [Header("物品的稀有度")]
    public ItemRarity itemRarity;

    public override string ToString()
    {
        return string.Format("[id]:{0}  [num]:{1}", id, num);
    }
}

public class PackageItemCompare : IComparer<PackageItem>
{
    public int Compare(PackageItem x, PackageItem y)
    {
        return -x.itemRarity.CompareTo(y.itemRarity);
    }
}


