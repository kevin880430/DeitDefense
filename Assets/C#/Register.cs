using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//使用Rest Client For Unity套件程式庫
using Proyecto26;
public class Register : MonoBehaviour
{
    //アカウント入力
    [Header("帳號輸入框")]
    public InputField AccInputField;
    //パスワード入力
    [Header("密碼輸入框")]
    public InputField PasswordInputField;
    //メッセージ提示
    [Header("提示訊息文字")]
    public Text Message;

    //FirebaseのデータをUnityにダウンロード
    User DownloadData = new User();
    //Sign upページ
    [Header("註冊頁面")]
    public GameObject RegisterUI;
    //ログインページ
    [Header("登入頁面")]
    public GameObject LoginUI;

    void Start()
    {
        //メッセージを初期化
        Message.text = "";
    }
    public void OnSubmit() {
        Message.text = "";
        ////ユーザーアカウントによってFirebaseからデータを取得
        RestClient.Get<User>("https://secondgame202204-default-rtdb.firebaseio.com/" + AccInputField.text + ".json").Then(
            response => {
                DownloadData = response;
            }
        );
        StartCoroutine(Wait(1f));
       // StaticObj.UserAcc = AccInputField.text;
       // StaticObj.UserPassword = PasswordInputField.text;
        //ユーザーデータををFirebaseに送る
        //PostToFirebase();
    }
    IEnumerator Wait(float WaitTime) {
        yield return new WaitForSeconds(WaitTime);
        if (DownloadData.UserAcc == AccInputField.text)
        {
            Message.text = "已經有相同帳號，請重新註冊";
        }
        else {
             StaticObj.UserAcc = AccInputField.text;
             StaticObj.UserPassword = PasswordInputField.text;
            PostToFirebase();
            Message.text = "註冊成功";
        }
    }
    void PostToFirebase() {
        //ユーザーデータを配列
        User UploadData = new User();
        //Firebaseにアップロード
        RestClient.Put("https://secondgame202204-default-rtdb.firebaseio.com/" + AccInputField.text + ".json", UploadData);
    }
    public void Back() {
        RegisterUI.SetActive(false);
        LoginUI.SetActive(true);
    }
}
