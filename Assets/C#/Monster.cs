using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Monster : MonoBehaviour
{
    [Header("移動速度")]
    public float SetSpeed;
    //程式中使用的移動變數
    float Speed;

    [Header("怪物總血量")]
    public float TotalHP;
    //程式中計算怪物的血量
    float ScriptHP;
    [Header("被普攻打到扣多少血")]
    public float HurtHP;
    [Header("被大絕招打到扣多少血")]
    public float MagicHurtHP;

    //使用列舉切割Npc和Boss
    public enum MonsterState { 
        NPC,
        Boss
    }
    [Header("切換NPC和Boss身分")]
    public MonsterState Monster_State;
    [Header("NPC打玩家扣多少血")]
    public float NPCHurtPlayerHP;
    [Header("Boss打玩家扣多少血")]
    public float BossHurtPlayerHP;
    // Start is called before the first frame update
    void Start()
    {
        Speed = SetSpeed;
        ScriptHP = TotalHP;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider hit) {
        if (hit.GetComponent<Collider>().name == "mazu_wall") {
            Speed = 0;
            GetComponent<Animator>().SetBool("Att", true);
        }
    }
    void OnTriggerExit(Collider hit)
    {
        if (hit.GetComponent<Collider>().name == "mazu_wall")
        {
            Speed = SetSpeed;
            GetComponent<Animator>().SetBool("Att", false);
        }
    }
    public void HurtMonster(bool isMagic) {
        //如果是被大絕招打到
        if (isMagic)
        {
            ScriptHP -= MagicHurtHP;
        }
        //如果是被普攻打到
        else {
            ScriptHP -= HurtHP;
        }
        //如果怪物沒有血
        if (ScriptHP <= 0) {
            Speed = 0;
            GetComponent<Animator>().SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
            if (gameObject.tag == "NPC")
            {
                GameObject.Find("GM").GetComponent<ControlUI>().Score(false);
                GameObject.Find("GM").GetComponent<GM>().DeadNum++;
            }
            else {
                GameObject.Find("GM").GetComponent<ControlUI>().Score(true);
                GameObject.Find("GM").GetComponent<ControlUI>().GameOver(true);

            }
        }
    }
    public void HurtPlayer() {
        switch (Monster_State) {
            case MonsterState.NPC:
                GameObject.Find("GM").GetComponent<GM>().HurtPlayerHP(NPCHurtPlayerHP);
                break;
            case MonsterState.Boss:
                GameObject.Find("GM").GetComponent<GM>().HurtPlayerHP(BossHurtPlayerHP);
                break;
        }
    }
}
