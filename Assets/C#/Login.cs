using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
//使用Rest Client For Unity套件程式庫
using Proyecto26;
public class Login : MonoBehaviour
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
    [Header("查詢密碼頁面")]
    public GameObject SearchUI;
    [Header("遊戲Menu")]
    public GameObject MenuUI;
    // Start is called before the first frame update
    void Start()
    {
        //遊戲一開始時，將訊息內的文字清空
        Message.text = "";
    }

    public void OnSubmit()
    {
        Message.text = "";
        //透過使用者帳號至Firebase找尋資料並帶回Unity
        RestClient.Get<User>("https://secondgame202204-default-rtdb.firebaseio.com/" + AccInputField.text + ".json").Then(
            response =>
            {
                DownloadData = response;
            }
        );
        StartCoroutine(Wait(1f));
    }
    IEnumerator Wait(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        if (DownloadData.UserAcc == AccInputField.text)
        {
            if (DownloadData.UserPassword == PasswordInputField.text)
            {
                //登入
                StartCoroutine(WaitLogIn(1f));
                Message.text = "登入成功";

            }
            else {
                Message.text = "密碼錯誤，請重新輸入";
            }
        }
        else
        {
            Message.text = "此帳號尚未註冊，請註冊帳號";
        }
    }
    IEnumerator WaitLogIn(float WaitTime) {
        yield return new WaitForSeconds(WaitTime);
        MenuUI.SetActive(true);
        LoginUI.SetActive(false);
    }
    public void ToRegister()
    {
        RegisterUI.SetActive(true);
        LoginUI.SetActive(false);
    }
    public void ToSearchPassword()
    {
        SearchUI.SetActive(true);
        LoginUI.SetActive(false);
    }
}
