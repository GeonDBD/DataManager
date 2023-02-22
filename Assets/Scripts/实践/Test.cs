using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public int id = 001;
    public string name = "Geon";
    public int age = 24;
    public bool sex = true;
    public List<int> list = new List<int> { 0, 1, 2, 3 };
    public Dictionary<int,int> dic = new Dictionary<int, int> {
        { 0, 1 },
        { 1, 2 }
    };
}

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs.DeleteAll();
        PlayerInfo p = new PlayerInfo();
        PlayerPrefsDataMgr.Instance.SaveData(p, "Player1");
        PlayerInfo pInfo = PlayerPrefsDataMgr.Instance.LoadData(typeof(PlayerInfo), "Player1") as PlayerInfo;
        Debug.Log(pInfo.id + " " + pInfo.name + " " + pInfo.age + " " + pInfo.sex);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
