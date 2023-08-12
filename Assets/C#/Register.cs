using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//使用Rest Client For Unity套件程式庫
using Proyecto26;
public class Register : MonoBehaviour
{
    [Header("帳號輸入框")]
    public InputField AccInputField;
    [Header("密碼輸入框")]
    public InputField PasswordInputField;
    [Header("提示訊息文字")]
    public Text Message;

    //下載Firebase資料到Unity 資料格式
    User DownloadData = new User();

    [Header("註冊頁面")]
    public GameObject RegisterUI;
    [Header("登入頁面")]
    public GameObject LoginUI;

    // Start is called before the first frame update
    void Start()
    {
        //遊戲一開始時，將訊息內的文字清空
        Message.text = "";
    }
    public void OnSubmit() {
        Message.text = "";
        //透過使用者帳號至Firebase找尋資料並帶回Unity
        RestClient.Get<User>("https://secondgame202204-default-rtdb.firebaseio.com/" + AccInputField.text + ".json").Then(
            response => {
                DownloadData = response;
            }
        );
        StartCoroutine(Wait(1f));
       // StaticObj.UserAcc = AccInputField.text;
       // StaticObj.UserPassword = PasswordInputField.text;
        //將使用者資料傳回Firebase
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
        //使用User資料排序的方式
        User UploadData = new User();
        //上傳資料到Firebase
        RestClient.Put("https://secondgame202204-default-rtdb.firebaseio.com/" + AccInputField.text + ".json", UploadData);
    }
    public void Back() {
        RegisterUI.SetActive(false);
        LoginUI.SetActive(true);
    }
}
