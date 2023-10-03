using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

//直列化
[Serializable]
public class User
{
    //ユーザーアカウント
    public string UserAcc;
    //ユーザーパスワード
    public string UserPassword;
    //フォマードを統一
    public User() {
        UserAcc = StaticObj.UserAcc;
        UserPassword = StaticObj.UserPassword;

    }
}
