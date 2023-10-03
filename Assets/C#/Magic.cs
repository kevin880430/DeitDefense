using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    //モンスターが撃退される距離
    [Header("怪物倒退的距離值")]
    public float Dis;
    
    void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().tag == "NPC" || hit.GetComponent<Collider>().tag == "BOSS")
        {
            //NPC又はBOSSの当たり判定,false=一般攻撃 true=スキル
            hit.GetComponent<Monster>().HurtMonster(true);
            //NPC又はBOSSがプレイヤーの攻撃に撃たれたら一定の距離下がる　
            hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + Dis);
            GameObject.Find("GM").GetComponent<GM>().Reset();
           //プレイヤーの攻撃(弾)を消す
           Destroy(transform.parent.gameObject);
        }
            //スキルが地面に落ちたらMPをリセット
        if (hit.GetComponent<Collider>().name == "mazu_floor") {
            GameObject.Find("GM").GetComponent<GM>().Reset();
            Destroy(transform.parent.gameObject);
        }
    }
}
