using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//使用Unity UI程式庫
using UnityEngine.UI;
//使用Rest Client For Unity程式庫
using Proyecto26;

//讀取信箱協定
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
//抓取外部資料
using System.IO;

public class Password : MonoBehaviour
{
    [Header("帳號輸入框")]
    public InputField AccInputField;
    [Header("Email輸入框")]
    public InputField EmailInputField;
    [Header("提示訊息文字")]
    public Text Message;
    //下載Firebase資料到Unity 資料格式
    User DownloadData = new User();
    [Header("密碼頁面")]
    public GameObject PasswordUI;
    [Header("登入頁面")]
    public GameObject LoginUI;
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
            response => {
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
            //代表可以發信
            SendEmail();
        }
        else
        {
        
            Message.text = "查無此帳號，請註冊帳號";
        }
    }
    void SendEmail() {
        //填入信件內容
        //填入自己的Email
        string acc = "shinecookie@gmail.com";
        //填入自己的Gmail密碼
        string pass = "Shine19880710";
        //讀取信箱格式
        MailMessage Mail = new MailMessage();
        //寄件者的Email
        Mail.From = new MailAddress(acc);
        //收件者的Email(讀取InputField)
        Mail.To.Add(EmailInputField.text);
        //Mail主旨
        Mail.Subject = "SecondGame密碼信件";
        //Mail內文
        Mail.Body = "親愛的玩家 您好，" + "\n" + "您的遊戲帳號:" + DownloadData.UserAcc + "," + "\n" + "您的遊戲密碼:" + DownloadData.UserPassword + "。" + "\n" + "遊戲團隊 敬上";

        //Gmail電子郵件使用協定為Smtp格式，每個郵件協定不同未來自行修改
        SmtpClient smtpServer = new SmtpClient();
        smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpServer.Host = "smtp.gmail.com";
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(acc, pass) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
            delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            { return true; };
        smtpServer.Send(Mail);

        Message.text = "信件發送成功";
    }
    public void Back()
    {
        LoginUI.SetActive(true);
        PasswordUI.SetActive(false);
    }
}
