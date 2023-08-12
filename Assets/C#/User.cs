using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

//序列化
[Serializable]
public class User
{
    //使用者帳號
    public string UserAcc;
    //使用者密碼
    public string UserPassword;
    //統一上傳與接收格式
    public User() {
        UserAcc = StaticObj.UserAcc;
        UserPassword = StaticObj.UserPassword;

    }
}
