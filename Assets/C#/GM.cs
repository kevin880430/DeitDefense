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
    //モンスターを生成する間隔時間
    [Header("多少時間產生一個怪物")]
    public float SetTime;
    //全モンスター数
    [Header("小怪在關卡內的總數量")]
    public float TotalNum;
    //現在モンスターの数
    float Num;
    //モンスター死亡数
    public float DeadNum;
    //モンスターバー
    [Header("怪物條")]
    public Image NumBar;
    //MPバー
    [Header("信仰條")]
    public Image MagicBar;
    //MPバーをためる時間
    [Header("多少時間以後信仰條集滿")]
    public float SetAllTime;
    //プログラムで計算する時間
    float ScriptTime;
    //スキルの画像素材
    [Header("大絕招的圖片")]
    public Image MagicImage;
    //プレイヤーhp
    [Header("玩家的總血量")]
    public float PlayerTotalHP;
    //プログラムで計算するプレイヤーhp
    float PlayerScriptHP;
    [Header("玩家的血條")]
    public Image PlayerHPBar;
    //一時停止画面
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
        //モンスター(NPC)を生成するオブジェクトのCollider最小値
        Vector3 MaxVector3 = CreatePos.GetComponent<Collider>().bounds.max;
        //モンスター(NPC)を生成するオブジェクトのCollider最大値
        Vector3 MinVector3 = CreatePos.GetComponent<Collider>().bounds.min;
        //モンスター(NPC)を生成するオブジェクトの位置をランダムにする
        Vector3 RandomVector3 = new Vector3(Random.Range(MinVector3.x, MaxVector3.x), CreatePos.transform.position.y, CreatePos.transform.position.z);
        //現在のモンスター数は設定値より少ないなら
        if (Num < TotalNum) {
            //モンスターを生成する
            Instantiate(NPC, RandomVector3, CreatePos.transform.rotation);
            //現在モンスター数+1
            Num++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //モンスターバーとモンスターの数を一致させる
        NumBar.fillAmount = 1f - (DeadNum / TotalNum);
        //全部のモンスターを倒したらBossを生成する
        if (NumBar.fillAmount == 0 && GameObject.FindGameObjectsWithTag("BOSS").Length <= 0) {
            Instantiate(Boss, CreatePos.transform.position, CreatePos.transform.rotation);
        }
        //タイマーを足して
        ScriptTime += Time.deltaTime;
        //設定時間を達したら(MPは時間によって貯める)
        if (ScriptTime > SetAllTime)
        {
            //スキルボタンの色が変わる
            MagicImage.color = Color.white;
        }
        //貯めていない時スキルボタンは灰色
        else {
            MagicImage.color = Color.gray;
        }
        //MPバーを貯める
        MagicBar.fillAmount = ScriptTime / SetAllTime;
    }
    public void Reset() {
        //スキルを使ったらMPバーをリセット
        MagicBar.fillAmount = 0;
        ScriptTime = 0;
    }

    public void HurtPlayerHP(float hurt) {
        //プレイヤーのhpを減る
        PlayerScriptHP -= hurt;
        //プレイヤーのhpバー
        PlayerHPBar.fillAmount = PlayerScriptHP / PlayerTotalHP;
        if (PlayerHPBar.fillAmount == 0) {
            GameObject.Find("GM").GetComponent<ControlUI>().GameOver(false);
        }
    }
    public void Pause() {
        isPause = !isPause;
        //一時停止機能
        if (isPause)
        {
            Time.timeScale = 0;
        }
        else {
            Time.timeScale = 1;
        }
        //一時停止画面を表示
        PauseUI.SetActive(isPause);
    }

    public void BackMenu() { 
        //Menuに戻る時一時停止を解除
        Time.timeScale = 1;
        Application.LoadLevel("Menu");
    } 
}
