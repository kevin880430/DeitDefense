using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [Header("移動速度")]
    public float Speed;

    [Header("怪物倒退的距離值")]
    public float Dis;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.right* Speed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().tag == "NPC"|| hit.GetComponent<Collider>().tag == "BOSS")
        {
            //NPC或BOSS扣血,false=普攻 true=大絕招
            hit.GetComponent<Monster>().HurtMonster(false);
            //NPC和BOSS被打到以後要往後退
            hit.transform.position = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + Dis);
            //刪除普攻物件
            Destroy(gameObject);
        }
    }
}
