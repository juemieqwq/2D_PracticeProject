using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class GMCmd
{
    [MenuItem("GMCmd/读取表格")]
    public static void ReadTable()
    {
        PackageItemTable packageItemTable = Resources.Load<PackageItemTable>("UI/Package/PackageTable");
        //foreach (var ItemInfo in packageItemTable.packageItemList)
        //{
        //    Debug.LogError(string.Format("id:{0},name:{1}", ItemInfo.id, ItemInfo.name));
        //}
    }

    [MenuItem("GMCmd/创建背包测试数据")]
    public static void CreatePackageDate()
    {
        PackageLocalDate.Instance.items = new List<PackageLocalItem>();
        for (int i = 1; i < 9; i++)
        {
            PackageLocalItem packageLocalItem = new PackageLocalItem()
            {
                uid = Guid.NewGuid().ToString(),
                id = i,
                num = i,
                level = i,
                isNew = i / 2 != 0 ? true : false,
            };
            PackageLocalDate.Instance.items.Add(packageLocalItem);
        }
        PackageLocalDate.Instance.SavePackage();
    }

    [MenuItem("GMCmd/读取背包数据")]
    public static void LoadPackageDate()
    {
        List<PackageLocalItem> readItems = PackageLocalDate.Instance.LoadPackage();
        foreach (var ItemInfo in readItems)
        {
            Debug.Log(ItemInfo);
        }
    }

    [MenuItem("GMCmd/删除背包数据")]
    public static void DeletePackageDate()
    {
        PackageLocalDate.Instance.items = null;
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("GMCmd/更具稀有度排序")]
    public static void SortItem()
    {
        if (UIManager.Instance.runningPanels.TryGetValue("PackagePanel", out var packagePanel))
        {
            (packagePanel as PackagePanel).packageItemsList.Sort(new PackageItemCompare());
            Debug.Log("Try Sort");
        }
    }
}
