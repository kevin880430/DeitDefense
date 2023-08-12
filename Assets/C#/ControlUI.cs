using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlUI : MonoBehaviour
{
    [Header("數值0-9圖片")]
    public Sprite[] NumberSprite;
    [Header("遊戲畫面上的Level Image")]
    public Image[] GameLevelImages;
    [Header("遊戲結束畫面上的Level Image")]
    public Image[] GameOverLevelImages;

    [Header("遊戲分數")]
    public int TotalScore;
    [Header("打死NPC的分數")]
    public int AddNPCScore;
    [Header("打死Boss的分數")]
    public int AddBossScore;
    [Header("分數的物件")]
    public GameObject ScoreObj;
    [Header("分數的父物件")]
    public GameObject ScoreParentObj;
    [Header("儲存產出的分數物件")]
    public List<GameObject> ScoreImagePrefab;

    [Header("遊戲結束的UI物件")]
    public GameObject GameOverUI;
    [Header("獎勵分數")]
    public int AwardScore;
    //判斷是否有獎勵分數
    bool isAward;
    [Header("獎勵分數的物件")]
    public GameObject AwardScoreObj;
    [Header("獎勵分數的父物件")]
    public GameObject AwardScoreParentObj;
    [Header("遊戲結束總分數的物件")]
    public GameObject GameOverScoreObj;
    [Header("遊戲結束總分數的父物件")]
    public GameObject GameOverParentObj;
    //跳動分數的起始值
    int ScoreStartNum = 0;
    //程式中計算的數值
    int ScoreResult;
    //程式中要計算幾次(跳動次數)
    int ScoreJump = 15;
    //遊戲結束總分數
    int GameOverTotalScore;
    //程式中的獎勵分數
    int AwardScriptScore;
    [Header("儲存產出的獎勵分數物件")]
    public List<GameObject> AwardScoreImagePrefab;
    [Header("儲存產出的遊戲結束總分數物件")]
    public List<GameObject> TotalScoreImagePrefab;
    // Start is called before the first frame update
    void Start()
    {
        ScoreObj.SetActive(false);
        GameOverScoreObj.SetActive(false);
        AwardScoreObj.SetActive(false);
        //關卡數值長度<2，代表關卡數值介於1-9
        if (StaticObj.LevelID.ToString().Length < 2) {
            //Images陣列 ID 為0=十進位，ID 為1=個進位
            #region 遊戲場景裡面的Level
            //十進位
            GameLevelImages[0].sprite = NumberSprite[0];
            //個進位,%代表取餘數
            GameLevelImages[1].sprite = NumberSprite[StaticObj.LevelID%10];
            #endregion
            #region 遊戲結束中的Level
            //十進位
            GameOverLevelImages[0].sprite = NumberSprite[0];
            //個進位
            //int.Parse將文字轉換成數值
            //Substring取數值文字的位置
            GameOverLevelImages[1].sprite = NumberSprite[int.Parse(StaticObj.LevelID.ToString().Substring(0,1))];
            #endregion
        }
        //關卡數值長度>=2，代表關卡數值介於10-99
        else
        {
            #region 遊戲場景裡面的Level
            //十進位
            GameLevelImages[0].sprite = NumberSprite[StaticObj.LevelID / 10];
            //個進位,%代表取餘數
            GameLevelImages[1].sprite = NumberSprite[StaticObj.LevelID % 10];
            #endregion
            #region 遊戲結束中的Level
            //十進位
            GameOverLevelImages[0].sprite = NumberSprite[int.Parse(StaticObj.LevelID.ToString().Substring(0, 1))];
            //個進位
            //int.Parse將文字轉換成數值
            //Substring取數值文字的位置
            GameOverLevelImages[1].sprite = NumberSprite[int.Parse(StaticObj.LevelID.ToString().Substring(1, 1))];
            #endregion
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //計算分數
    public void Score(bool MonsterState) {
        //MonsterState =true 為Boss,MonsterState =false 為NPC
        if (MonsterState)
        {
            TotalScore += AddBossScore;
        }
        else {
            TotalScore += AddNPCScore;
        }
        //判斷ScoreImagePrefab階層下是否有物件，如果有物件就要先清除，再重新將分數顯示
        //ScoreImagePrefab.Count抓取list數量
        if (ScoreImagePrefab.Count > 0) {
            for (int i = 0; i < ScoreImagePrefab.Count; i++) {
                //將List階層下的物件刪除
                Destroy(ScoreImagePrefab[i].gameObject);
            }
            //清除List長度資料
            ScoreImagePrefab.Clear();
        }
        //使用for迴圈增加分數的圖片物件
        for (int j = 0; j < TotalScore.ToString().Length; j++) {
            //動態生成出來的分數物件存在List中
            ScoreImagePrefab.Add(Instantiate(ScoreObj) as GameObject);
            //將List中的物件移動到分數的父物件下
            ScoreImagePrefab[j].transform.parent = ScoreParentObj.transform;
            //將生成出來的物件開啟
            ScoreImagePrefab[j].SetActive(true);
        }
        //使用for迴圈將數字圖片帶入到List物件中
        for (int id = 0; id < TotalScore.ToString().Length; id++) {
            ScoreImagePrefab[id].GetComponent<Image>().sprite = NumberSprite[int.Parse(TotalScore.ToString().Substring(id, 1))];
        }
    }

    //遊戲結束的分數
    public void GameOver(bool GameState) {
        GameOverUI.SetActive(true);
        //GameState=true 玩家勝利,Boss死亡 GameState=false玩家失敗 玩家血量=0
        if (GameState)
        {
            AwardScriptScore = AwardScore;
        }
        else {
            AwardScriptScore = 0;
        }
        GameOverTotalScore = TotalScore + AwardScriptScore;
        StartCoroutine(JumpNumber(AwardScriptScore,0));
    }

    IEnumerator JumpNumber(int ScoreNum,int ScoreState) {
        //一個區間要跳多少數值
        int delta = ScoreNum / ScoreJump;
        //將結果值歸0
        ScoreResult = 0;
        //依造執行次數，使用for迴圈讓分數累加
        for (int i = 0; i < ScoreJump; i++) {
            //累加區間的獎勵值
            ScoreResult += delta;
            //獎勵分數顯示圖片
            if (ScoreState == 0)
            {
                AwardScoreImage();
            }
            else
            {
                TotalScoreImage();
            }
            //加完一次區間值等待0.1秒再加上另外一個區間數值
            yield return new WaitForSeconds(0.1f);
        }
        //限定分數最後的數值
        ScoreResult = ScoreNum;

        if (ScoreState == 0)
        {
            AwardScoreImage();
            yield return new WaitForSeconds(1f);
            StartCoroutine(JumpNumber(GameOverTotalScore, 1));
        }
        else
        {
            TotalScoreImage();
        }

    }
    void AwardScoreImage() {
        if (AwardScoreImagePrefab.Count > 0)
        {
            for (int i = 0; i < AwardScoreImagePrefab.Count; i++)
            {
                //將List階層下的物件刪除
                Destroy(AwardScoreImagePrefab[i].gameObject);
            }
            //清除List長度資料
            AwardScoreImagePrefab.Clear();
        }
        //使用for迴圈增加分數的圖片物件
        for (int j = 0; j < ScoreResult.ToString().Length; j++)
        {
            //動態生成出來的分數物件存在List中
            AwardScoreImagePrefab.Add(Instantiate(ScoreObj) as GameObject);
            //將List中的物件移動到分數的父物件下
            AwardScoreImagePrefab[j].transform.parent = AwardScoreParentObj.transform;
            //將生成出來的物件開啟
            AwardScoreImagePrefab[j].SetActive(true);
        }
        //使用for迴圈將數字圖片帶入到List物件中
        for (int id = 0; id < ScoreResult.ToString().Length; id++)
        {
            AwardScoreImagePrefab[id].GetComponent<Image>().sprite = NumberSprite[int.Parse(ScoreResult.ToString().Substring(id, 1))];
        }
    }
    void TotalScoreImage() {
        if (TotalScoreImagePrefab.Count > 0)
        {
            for (int i = 0; i < TotalScoreImagePrefab.Count; i++)
            {
                //將List階層下的物件刪除
                Destroy(TotalScoreImagePrefab[i].gameObject);
            }
            //清除List長度資料
            TotalScoreImagePrefab.Clear();
        }
        //使用for迴圈增加分數的圖片物件
        for (int j = 0; j < ScoreResult.ToString().Length; j++)
        {
            //動態生成出來的分數物件存在List中
            TotalScoreImagePrefab.Add(Instantiate(ScoreObj) as GameObject);
            //將List中的物件移動到分數的父物件下
            TotalScoreImagePrefab[j].transform.parent = GameOverParentObj.transform;
            //將生成出來的物件開啟
            TotalScoreImagePrefab[j].SetActive(true);
        }
        //使用for迴圈將數字圖片帶入到List物件中
        for (int id = 0; id < ScoreResult.ToString().Length; id++)
        {
            TotalScoreImagePrefab[id].GetComponent<Image>().sprite = NumberSprite[int.Parse(ScoreResult.ToString().Substring(id, 1))];
        }
    }
    public void Regame() {
        Application.LoadLevel(Application.loadedLevel);
    }
    public void Nextgame()
    {
        StaticObj.LevelID++;
        Application.LoadLevel(Application.loadedLevel);
    }
}
