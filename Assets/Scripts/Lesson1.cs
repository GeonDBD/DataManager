using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ��
        PlayerPrefs.SetInt("myInt", 1);
        PlayerPrefs.SetFloat("myFloat", 2.1f);
        PlayerPrefs.SetString("myName", "Geon");
        PlayerPrefs.Save();
        // ȡ
        PlayerPrefs.GetInt("myInt", 0);
        PlayerPrefs.GetFloat("myInt", 0.0f);
        PlayerPrefs.GetString("myInt", "Null");
        // �ж��Ƿ����
        PlayerPrefs.HasKey("myInt");
        // ɾ
        PlayerPrefs.DeleteKey("myInt");
        PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
