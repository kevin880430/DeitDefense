using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("移動速度")]
    public float Speed;

    [Header("怪物倒退的距離值")]
    public float Dis;
    

    void Update()
    {
        //弾を飛ばす
        transform.Translate(Vector3.right* Speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider hit)
    {
        //弾の当たり判定
        if (hit.GetComponent<Collider>().tag == "NPC"|| hit.GetComponent<Collider>().tag == "BOSS")
        {
            //NPC又はBOSSに当たったらダメージを加える,falseはNPC trueはBoss
            hit.GetComponent<Monster>().HurtMonster(false);
            ////NPC又はBOSSが攻撃を受けたら一定の距離に下がる
            hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + Dis);
            //弾を消す
            Destroy(gameObject);
        }
    }
}
