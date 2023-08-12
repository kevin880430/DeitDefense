using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    [Header("NPC")]
    public GameObject NPC;
    [Header("Boss")]
    public GameObject Boss;
    [Header("怪物生成的位置")]
    public GameObject CreatePos;
    [Header("多少時間產生一個怪物")]
    public float SetTime;
    [Header("小怪在關卡內的總數量")]
    public float TotalNum;
    //目前小怪產生的數量
    float Num;
    //怪物死亡數量
    public float DeadNum;

    [Header("怪物條")]
    public Image NumBar;

    [Header("信仰條")]
    public Image MagicBar;
    [Header("多少時間以後信仰條集滿")]
    public float SetAllTime;
    //程式中計算信仰條時間
    float ScriptTime;
    [Header("大絕招的圖片")]
    public Image MagicImage;

    [Header("玩家的總血量")]
    public float PlayerTotalHP;
    //程式中計算玩家的血量
    float PlayerScriptHP;
    [Header("玩家的血條")]
    public Image PlayerHPBar;

    [Header("遊戲暫停畫面")]
    public GameObject PauseUI;
    bool isPause;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CreateNPC", SetTime, SetTime);
        MagicBar.fillAmount = 0;
        PlayerScriptHP = PlayerTotalHP;
        PlayerHPBar.fillAmount = 1;
        Time.timeScale = 1;
    }

    void CreateNPC() {
        //抓取NPC生成物件的Collider邊界最大值
        Vector3 MaxVector3 = CreatePos.GetComponent<Collider>().bounds.max;
        //抓取NPC生成物件的Collider邊界最小值
        Vector3 MinVector3 = CreatePos.GetComponent<Collider>().bounds.min;
        //隨機亂數產生怪物要生成的位置
        Vector3 RandomVector3 = new Vector3(Random.Range(MinVector3.x, MaxVector3.x), CreatePos.transform.position.y, CreatePos.transform.position.z);
        //如果目前場景的怪物數量<一關總體數量
        if (Num < TotalNum) {
            //動態生成小怪
            Instantiate(NPC, RandomVector3, CreatePos.transform.rotation);
            //目前數量+1
            Num++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        NumBar.fillAmount = 1f - (DeadNum / TotalNum);
        if (NumBar.fillAmount == 0 && GameObject.FindGameObjectsWithTag("BOSS").Length <= 0) {
            Instantiate(Boss, CreatePos.transform.position, CreatePos.transform.rotation);
        }
        //讓程式中的時間一直累加
        ScriptTime += Time.deltaTime;
        //如果程式計算的時間大於屬性面板中調整的時間值
        if (ScriptTime > SetAllTime)
        {
            //大絕招的圖片顏色變為白色
            MagicImage.color = Color.white;
        }
        //如果時間還沒滿大絕招圖片呈現灰色
        else {
            MagicImage.color = Color.gray;
        }
        //時間顯示在信仰條上
        MagicBar.fillAmount = ScriptTime / SetAllTime;
    }
    public void Reset() {
        MagicBar.fillAmount = 0;
        ScriptTime = 0;
    }

    public void HurtPlayerHP(float hurt) {
        //扣除玩家的血量
        PlayerScriptHP -= hurt;
        //玩家血量條
        PlayerHPBar.fillAmount = PlayerScriptHP / PlayerTotalHP;
        if (PlayerHPBar.fillAmount == 0) {
            GameObject.Find("GM").GetComponent<ControlUI>().GameOver(false);
        }
    }
    public void Pause() {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
        PauseUI.SetActive(isPause);
    }

    public void BackMenu() { 
        Time.timeScale = 1;
        Application.LoadLevel("Menu");
    } 
}
