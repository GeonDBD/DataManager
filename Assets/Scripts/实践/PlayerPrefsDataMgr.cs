using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// PlayerPrefs数据管理类 统一管理数据的存储和读取
/// </summary>
public class PlayerPrefsDataMgr
{
    private static PlayerPrefsDataMgr instance = new PlayerPrefsDataMgr();

    private PlayerPrefsDataMgr()
    {

    }

    public static PlayerPrefsDataMgr Instance
    {
        get { return instance; }
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    /// <param name="data">数据对象</param>
    /// <param name="keyName">数据对象的唯一Key</param>
    public void SaveData(object data, string keyName)
    {
        Type dataType = data.GetType();
        FieldInfo[] infos = dataType.GetFields();
        string saveKeyName = "";
        FieldInfo info;

        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            // 定义key规则
            // keyName_数据类型_字段类型_字段名
            saveKeyName = keyName + "_" + dataType.Name + "_" + info.FieldType.Name + "_" + info.Name;
            //Debug.Log(saveKeyName);

            // 封装存储值的方法
            SaveValue(info.GetValue(data), saveKeyName);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// 存储值的方法
    /// </summary>
    /// <param name="value">数据的值</param>
    /// <param name="keyName">data的Key</param>
    private void SaveValue(object value, string keyName)
    {
        Type valueType = value.GetType();
        if (valueType == typeof(int))
        {
            PlayerPrefs.SetInt(keyName, (int)value);
        }
        else if (valueType == typeof(float))
        {
            PlayerPrefs.SetFloat(keyName, (float)value);
        }
        else if (valueType == typeof(string))
        {
            PlayerPrefs.SetString(keyName, value.ToString());
        }
        else if (valueType == typeof(bool))
        {
            // 自己定义的bool存储规则
            PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);
        }
    }

    /// <summary>
    /// 读取数据
    /// </summary>
    /// <param name="type">想要读取的 数据的 数据类型</param>
    /// <param name="keyName">数据对象的唯一Key</param>
    /// <returns></returns>
    public object LoadData(Type type, string keyName)
    {
        // 通过无参构造实例化对象
        object data = Activator.CreateInstance(type);

        FieldInfo[] infos = type.GetFields();
        string loadKeyName = "";
        FieldInfo info = null;

        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            // 定义key规则
            // keyName_数据类型_字段类型_字段名
            loadKeyName = keyName + "_" + type + "_" + info.FieldType.Name + "_" + info.Name;

            // 存储数据
            info.SetValue(data, LoadValue(info.FieldType, loadKeyName));
        }
        
        return data;
    }

    public object LoadValue(Type fieldType, string keyName)
    {
        if (fieldType == typeof(int))
        {
            return PlayerPrefs.GetInt(keyName, 0);
        }
        else if (fieldType == typeof(string))
        {
            return PlayerPrefs.GetString(keyName, "");
        }
        else if (fieldType == typeof(float))
        {
            return PlayerPrefs.GetFloat(keyName, 0);
        }
        else if (fieldType == typeof(bool))
        {
            return PlayerPrefs.GetInt(keyName, 0) == 1 ? true : false;
        }
        return null;
    }
}
