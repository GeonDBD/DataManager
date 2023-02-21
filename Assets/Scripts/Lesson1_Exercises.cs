using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankList
{
    public List<RankInfo> rankInfoList;

    public RankList()
    {
        Load();
    }

    public void Add(string name, int score, int time)
    {
        rankInfoList.Add(new RankInfo(name, score, time));
    }

    public void Save()
    {
        // 存储多少条数据
        PlayerPrefs.SetInt("rankListCount", rankInfoList.Count);
        for (int i = 0; i < rankInfoList.Count; i++)
        {
            RankInfo info = rankInfoList[i];
            PlayerPrefs.SetString("rankInfoName" + i, info.rPlayerName);
            PlayerPrefs.SetInt("rankInfoScore" + i, info.rPlayerScore);
            PlayerPrefs.SetInt("rankInfoTime" + i, info.rPlayerTime);
        }
    }

    private void Load()
    {
        rankInfoList = new List<RankInfo>();
        int count = PlayerPrefs.GetInt("rankListCount", 0);
        for (int i = 0; i < count; i++)
        {
            RankInfo info = new RankInfo(
                PlayerPrefs.GetString("rankInfoName" + i),
                PlayerPrefs.GetInt("rankInfoScore" + i),
                PlayerPrefs.GetInt("rankInfoTime" + i)
            );
            rankInfoList.Add(info);
        }
    }
}

public class RankInfo
{
    public string rPlayerName;
    public int rPlayerScore;
    public int rPlayerTime;

    public RankInfo(string playerName, int score, int time)
    {
        rPlayerName = playerName;
        rPlayerScore = score;
        rPlayerTime = time;
    }
}

public class Item
{
    public int iId;
    public int num;
}

public class Player
{
    public string pName;
    public int pAge;
    public int pAtk;
    public int pDef;
    public List<Item> pItems;

    private string keyName;

    public void Save()
    {
        PlayerPrefs.SetString(keyName + "_name", pName);
        PlayerPrefs.SetInt(keyName + "_age", pAge);
        PlayerPrefs.SetInt(keyName + "_atk", pAtk);
        PlayerPrefs.SetInt(keyName + "_def", pDef);
        // 存储有多少个装备
        PlayerPrefs.SetInt(keyName + "_ItemNum", pItems.Count);
        for (int i = 0; i < pItems.Count; i++)
        {
            // 存储每一个装备信息
            PlayerPrefs.SetInt(keyName + "_itemId" + i, pItems[i].iId);
            PlayerPrefs.SetInt(keyName + "_itemNum" + i, pItems[i].num);
        }
        PlayerPrefs.Save();
    }

    public void Load(string keyName)
    {
        this.keyName = keyName;

        pName = PlayerPrefs.GetString(keyName + "_name", "Null");
        pAge = PlayerPrefs.GetInt(keyName + "_age", 18);
        pAtk = PlayerPrefs.GetInt(keyName + "_atk", 5);
        pDef = PlayerPrefs.GetInt(keyName + "_def", 5);

        // 读取装备数量
        int num = PlayerPrefs.GetInt(keyName + "_ItemNum", 0);
        // 初始化容器
        pItems = new List<Item>();
        Item item;
        for (int i = 0; i < num; i++)
        {
            item = new Item();
            item.iId = PlayerPrefs.GetInt(keyName + "_itemId" + i, 0);
            item.num = PlayerPrefs.GetInt(keyName + "_itemNum" + i, 0);
            pItems.Add(item);
        }
    }
}

public class Lesson1_Exercises : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 1
        Player p1 = new Player();
        p1.Load("Geon");
        print("PlayerInfo: " + p1.pName + " " + p1.pAge + " " + p1.pAtk + " " + p1.pDef);

        Player p2 = new Player();
        p2.Load("Tom");
        print("PlayerInfo: " + p2.pName + " " + p2.pAge + " " + p2.pAtk + " " + p2.pDef);
        p1.pName = "Geon";
        p1.pAge = 24;
        p1.pAtk = 40;
        p1.pDef = 40;
        p1.Save();

        // 2
        print("PlayerItemInfo: " + p1.pItems.Count);


        // 3
        RankList rankList = new RankList();
        print(rankList.rankInfoList.Count);

        for (int i = 0; i < rankList.rankInfoList.Count; i++)
        {
            print("Rank: " + rankList.rankInfoList[i].rPlayerName + " " + rankList.rankInfoList[i].rPlayerScore + " " + rankList.rankInfoList[i].rPlayerTime);
        }

        rankList.Add("Geon", 114514, 90);
        rankList.Save();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
