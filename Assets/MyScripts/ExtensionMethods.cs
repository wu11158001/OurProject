using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 擴展方法
/// </summary>
public static class ExtensionMethods
{
    /// <summary>
    /// 搜尋子物件
    /// </summary>
    /// <typeparam name="T">Component</typeparam>
    /// <param name="SearchObj">搜尋物件</param>
    /// <param name="searchName">搜尋物件名稱</param>
    /// <returns></returns>
    public static T FindAnyChild<T>(this Transform SearchObj, string searchName ) where T : Component
    {
        for (int i = 0; i < SearchObj.childCount; i++)
        {
            if(SearchObj.GetChild(i).childCount > 0)//子物件下還有子物件
            {
                var child = SearchObj.GetChild(i).FindAnyChild<Transform>(searchName);                
                if (child != null)
                    return child.GetComponent<T>();
            }
            if (SearchObj.GetChild(i).name == searchName)//找到物件
            {
                return SearchObj.GetChild(i).GetComponent<T>();
            }
        }

        return default;
    }
}
