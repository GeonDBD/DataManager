using OpenCover.Framework.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// PlayerPrefs���ݹ����� ͳһ�������ݵĴ洢�Ͷ�ȡ
/// </summary>
public class PlayerPrefsDataMgr
{
    // ����ʵ��
    private static PlayerPrefsDataMgr instance = new PlayerPrefsDataMgr();
    private PlayerPrefsDataMgr()
    {

    }
    public static PlayerPrefsDataMgr Instance
    {
        get { return instance; }
    }

    /// <summary>
    /// �洢����
    /// </summary>
    /// <param name="data">���ݶ���</param>
    /// <param name="keyName">���ݶ����ΨһKey</param>
    public void SaveData(object data, string keyName)
    {
        Type dataType = data.GetType();
        FieldInfo[] infos = dataType.GetFields();
        string saveKeyName = "";
        FieldInfo info;

        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            // ����key���� keyName_��������_�ֶ�����_�ֶ���
            saveKeyName = keyName + "_" + dataType.Name + "_" + info.FieldType.Name + "_" + info.Name;

            // �����ѷ�װ�洢ֵ�ķ���
            SaveValue(info.GetValue(data), saveKeyName);
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �洢ֵ�ķ���
    /// </summary>
    /// <param name="value">���ݵ�ֵ</param>
    /// <param name="keyName">data��Key</param>
    private void SaveValue(object value, string keyName)
    {
        Type valueType = value.GetType();

        // ��������
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
            // �Լ������bool�洢����
            PlayerPrefs.SetInt(keyName, (bool)value ? 1 : 0);
        }
        // List
        else if (typeof(IList).IsAssignableFrom(valueType))
        {
            IList list = value as IList;
            PlayerPrefs.SetInt(keyName, list.Count);
            int index = 0;
            foreach (object item in list)
            {
                SaveValue(item, keyName + index);
                index++;
            }
        }
        // Dictionary
        else if (typeof(IDictionary).IsAssignableFrom(valueType))
        {
            IDictionary dictionary = value as IDictionary;
            PlayerPrefs.SetInt(keyName, dictionary.Count);
            int index = 0;
            foreach (object key in dictionary.Keys)
            {
                SaveValue(key, keyName + "_Key_" + index);
                SaveValue(dictionary[key], keyName + "_Value_" + index);
                index++;
            }
        }
        // �Զ������ʹ洢�����������Ҫ���Դ����ʹ洢��Ҫ������չ��
        else
        {
            SaveData(valueType, keyName);
        }
    }

    /// <summary>
    /// ��ȡ����
    /// </summary>
    /// <param name="type">��Ҫ��ȡ�� ���ݵ� ��������</param>
    /// <param name="keyName">���ݶ����ΨһKey</param>
    /// <returns></returns>
    public object LoadData(Type type, string keyName)
    {
        // ͨ���޲ι���ʵ��������
        object data = Activator.CreateInstance(type);

        FieldInfo[] infos = type.GetFields();
        string loadKeyName = "";
        FieldInfo info = null;

        for (int i = 0; i < infos.Length; i++)
        {
            info = infos[i];
            // ����key����
            // keyName_��������_�ֶ�����_�ֶ���
            loadKeyName = keyName + "_" + type + "_" + info.FieldType.Name + "_" + info.Name;

            // �洢����
            info.SetValue(data, LoadValue(info.FieldType, loadKeyName));
        }
        
        return data;
    }

    /// <summary>
    /// ��ȡֵ�ķ���
    /// </summary>
    /// <param name="fieldType"></param>
    /// <param name="keyName"></param>
    /// <returns></returns>
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
        else if (typeof(IList).IsAssignableFrom(fieldType))
        {
            int count = PlayerPrefs.GetInt(keyName, 0);
            IList list = Activator.CreateInstance(fieldType) as IList;
            for (int i = 0; i < count; i++)
            {
                list.Add(LoadValue(fieldType.GetGenericArguments()[0], keyName + i));
            }
            return list;
        }
        else if (typeof(IDictionary).IsAssignableFrom(fieldType))
        {
            int count = PlayerPrefs.GetInt(keyName, 0);
            IDictionary dic = Activator.CreateInstance(fieldType) as IDictionary;
            Type[] types = fieldType.GetGenericArguments();
            for (int i = 0; i < count; i++)
            {
                dic.Add(LoadValue(types[0], keyName + "_Key_" + i),
                        LoadValue(types[1], keyName + "_Value_" + i));
            }
            return dic;
        }
        else
        {
            return LoadData(fieldType, keyName);
        }
    }
}
