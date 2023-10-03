using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlUI : MonoBehaviour
{
    //数字の画像素材
    [Header("數值0-9圖片")]
    public Sprite[] NumberSprite;
    //レベルの画像素材
    [Header("遊戲畫面上的Level Image")]
    public Image[] GameLevelImages;
    //ゲームオバーのレベルの画像素材
    [Header("遊戲結束畫面上的Level Image")]
    public Image[] GameOverLevelImages;
    //スコア
    [Header("遊戲分數")]
    public int TotalScore;
    //モンスターを倒したら得られる点数
    [Header("打死NPC的分數")]
    public int AddNPCScore;
    ////Bossを倒したら得られる点数
    [Header("打死Boss的分數")]
    public int AddBossScore;
    //スコアオブジェクト
    [Header("分數的物件")]
    public GameObject ScoreObj;
    //スコアの親オブジェクト
    [Header("分數的父物件")]
    public GameObject ScoreParentObj;
    //スコア画像プレハブ
    [Header("儲存產出的分數物件")]
    public List<GameObject> ScoreImagePrefab;
    [Header("遊戲結束的UI物件")]
    public GameObject GameOverUI;
    //ボーナス点数
    [Header("獎勵分數")]
    public int AwardScore;
    //ボーナス点数の判定
    bool isAward;
    [Header("獎勵分數的物件")]
    public GameObject AwardScoreObj;
    [Header("獎勵分數的父物件")]
    public GameObject AwardScoreParentObj;
    [Header("遊戲結束總分數的物件")]
    public GameObject GameOverScoreObj;
    [Header("遊戲結束總分數的父物件")]
    public GameObject GameOverParentObj;
    //表示する点数の初期値
    int ScoreStartNum = 0;
    //プログラムの中に点数の計算
    int ScoreResult;
    //何回計算する(計算するの動画)
    int ScoreJump = 15;
    //最終的な点数
    int GameOverTotalScore;
    //ボーナス点数
    int AwardScriptScore;
    //ボーナス点数プレハブ
    [Header("儲存產出的獎勵分數物件")]
    public List<GameObject> AwardScoreImagePrefab;
    //総点数プレハブ
    [Header("儲存產出的遊戲結束總分數物件")]
    public List<GameObject> TotalScoreImagePrefab;
    void Start()
    {
        ScoreObj.SetActive(false);
        GameOverScoreObj.SetActive(false);
        AwardScoreObj.SetActive(false);
        //レベル数値<2，つまり範囲はレベル1-9
        if (StaticObj.LevelID.ToString().Length < 2) {
            //レベル画像[0]は10の位
            #region 遊戲場景裡面的Level
            //十の位
            GameLevelImages[0].sprite = NumberSprite[0];
            //一の位,%は余りを取る(ex:53だったら3を取る)
            GameLevelImages[1].sprite = NumberSprite[StaticObj.LevelID%10];
            #endregion
            #region 遊戲結束中的Level
            //十の位
            GameOverLevelImages[0].sprite = NumberSprite[0];
            //一の位
            //int.Parseは文字を数値化する
            //Substring数値文字の位置を取る
            GameOverLevelImages[1].sprite = NumberSprite[int.Parse(StaticObj.LevelID.ToString().Substring(0,1))];
            #endregion
        }
        //レベル数値>=2，つまりレベル10-99
        else
        {
            #region 遊戲場景裡面的Level
            //十の位
            GameLevelImages[0].sprite = NumberSprite[StaticObj.LevelID / 10];
            //一の位,%は余りを取る(ex:53だったら3を取る)
            GameLevelImages[1].sprite = NumberSprite[StaticObj.LevelID % 10];
            #endregion
            #region 遊戲結束中的Level
            //十の位
            GameOverLevelImages[0].sprite = NumberSprite[int.Parse(StaticObj.LevelID.ToString().Substring(0, 1))];
            //一の位
            //int.Parseは文字を数値化する
            //Substring数値文字の位置を取る
            GameOverLevelImages[1].sprite = NumberSprite[int.Parse(StaticObj.LevelID.ToString().Substring(1, 1))];
            #endregion
        }
    }

    //点数を計算する
    public void Score(bool MonsterState) {
        //MonsterState =trueはBossの点数,MonsterState =falseはモンスター(NPC)
        if (MonsterState)
        {
            TotalScore += AddBossScore;
        }
        else {
            TotalScore += AddNPCScore;
        }
        //点数の画像素材内容を初期化     
        if (ScoreImagePrefab.Count > 0) {
            for (int i = 0; i < ScoreImagePrefab.Count; i++) {
                //リストの中身を消す(初期化のため)
                Destroy(ScoreImagePrefab[i].gameObject);
            }
            ScoreImagePrefab.Clear();
        }
        //点数を(プレハブ)作る
        for (int j = 0; j < TotalScore.ToString().Length; j++) {
            //生成されたオブジェクトをリストに
            ScoreImagePrefab.Add(Instantiate(ScoreObj) as GameObject);
            //リストの中にオブジェクトをスコアの子供にする
            ScoreImagePrefab[j].transform.parent = ScoreParentObj.transform;
            //生成したプレハブを表示
            ScoreImagePrefab[j].SetActive(true);
        }
        //数字の画像をスコア欄に代入
        for (int id = 0; id < TotalScore.ToString().Length; id++) {
            ScoreImagePrefab[id].GetComponent<Image>().sprite = NumberSprite[int.Parse(TotalScore.ToString().Substring(id, 1))];
        }
    }

    //ゲームオバー
    public void GameOver(bool GameState) {
        GameOverUI.SetActive(true);
        //GameState=true はプレイヤーが勝利した場合,Boss死亡 GameState=falseはプレイヤーが失敗した場合
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
        //点数を加算する時加算動画を何回再生する
        int delta = ScoreNum / ScoreJump;
        //表示する点数は0から加算ため初期化する
        ScoreResult = 0;
        //点数を加算
        for (int i = 0; i < ScoreJump; i++) {
           //一回加算する値
            ScoreResult += delta;
            //ボーナス点数画像を表示
            if (ScoreState == 0)
            {
                AwardScoreImage();
            }
            else
            {
                TotalScoreImage();
            }
            //0.1秒毎に加算する
            yield return new WaitForSeconds(0.1f);
        }
        //最終的点数を確定する
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
        //ボーナス点数を初期化する
        if (AwardScoreImagePrefab.Count > 0)
        {
            for (int i = 0; i < AwardScoreImagePrefab.Count; i++)
            {
                
                Destroy(AwardScoreImagePrefab[i].gameObject);
            }
            
            AwardScoreImagePrefab.Clear();
        }
        //点数を(プレハブ)作る
        for (int j = 0; j < ScoreResult.ToString().Length; j++)
        {
            //生成されたオブジェクトをリストに
            AwardScoreImagePrefab.Add(Instantiate(ScoreObj) as GameObject);
            //リストの中にオブジェクトをスコアの子供にする
            AwardScoreImagePrefab[j].transform.parent = AwardScoreParentObj.transform;
            //生成したプレハブを表示
            AwardScoreImagePrefab[j].SetActive(true);
        }
        //数字の画像をスコア欄に代入
        for (int id = 0; id < ScoreResult.ToString().Length; id++)
        {
            AwardScoreImagePrefab[id].GetComponent<Image>().sprite = NumberSprite[int.Parse(ScoreResult.ToString().Substring(id, 1))];
        }
    }
    void TotalScoreImage() {
        //総点数を初期化する
        if (TotalScoreImagePrefab.Count > 0)
        {
            for (int i = 0; i < TotalScoreImagePrefab.Count; i++)
            {
                
                Destroy(TotalScoreImagePrefab[i].gameObject);
            }
            
            TotalScoreImagePrefab.Clear();
        }
        //点数を(プレハブ)作る
        for (int j = 0; j < ScoreResult.ToString().Length; j++)
        {
            //生成されたオブジェクトをリストに
            TotalScoreImagePrefab.Add(Instantiate(ScoreObj) as GameObject);
            //リストの中にオブジェクトをスコアの子供にする
            TotalScoreImagePrefab[j].transform.parent = GameOverParentObj.transform;
            //生成したプレハブを表示
            TotalScoreImagePrefab[j].SetActive(true);
        }
        //数字の画像をスコア欄に代入
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
