using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    [Header("怪物倒退的距離值")]
    public float Dis;
    
    void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().tag == "NPC" || hit.GetComponent<Collider>().tag == "BOSS")
        {
            //NPC或BOSS扣血,false=普攻 true=大絕招
            hit.GetComponent<Monster>().HurtMonster(true);
            //NPC和BOSS被打到以後要往後退
            hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + Dis);
            GameObject.Find("GM").GetComponent<GM>().Reset();
           //刪除大絕招父物件
           Destroy(transform.parent.gameObject);
        }
        if (hit.GetComponent<Collider>().name == "mazu_floor") {
            GameObject.Find("GM").GetComponent<GM>().Reset();
            Destroy(transform.parent.gameObject);
        }
    }
}
