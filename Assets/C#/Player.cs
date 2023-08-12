using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("玩家物件")]
    public Transform PlayerObj;
    [Header("玩家動畫")]
    public Animator PlayerAni;
    //收集射線打到的所有物件
     RaycastHit[] hits;
    //在射線打到的所有物件中找尋物件
    RaycastHit hit;
    //滑鼠或手指點擊的座標位置
    Vector3 Target_pos;
    //玩家注視位置
    Vector3 Look_Pos;

    [Header("玩家普攻物件")]
    public GameObject FireObj;
    [Header("普攻物件產生的位置")]
    public Transform CreatePos;

    //判斷大絕招是否可以執行
    bool isMagic;
    [Header("大絕招物件")]
    public GameObject MagicObj;
    //存放動態生成出來的MagicObj
    GameObject MagicObjPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //點擊滑鼠左鍵=手指點擊手機螢幕
        if (Input.GetMouseButton(0)) {
            //Camera.main自動去抓場景上標籤為MainCamera的攝影機
            //ScreenPointToRay攝影機為起始點，將滑鼠或手指點擊的位置轉換成3維座標，並連接成射線
            //Input.mousePosition滑鼠或手指點擊的位置
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //將射線打到的所有物件放在Hits陣列中
            hits = Physics.RaycastAll(ray);
            //使用for迴圈檢查hits陣列中是否有物件的名稱為地板
            for (int i = 0; i < hits.Length; i++) {
                hit = hits[i];
                if (hit.collider.name == "mazu_floor") {

                    //isMagic=true可以放大絕招
                    if (isMagic)
                    {
                        //檢查場景上是否有大絕招，如果沒有大絕招就生成大絕招物件
                        if (GameObject.FindGameObjectsWithTag("Magic").Length <= 0)
                        {
                            //動態生成出來的物件 狀態為Object，如果要寫入到GameObject中則需as GameObject轉型態
                            MagicObjPrefab = Instantiate(MagicObj) as GameObject;
                        }
                        else {
                            MagicObjPrefab.transform.position = new Vector3(hit.point.x, hit.point.y+0.1f, hit.point.z);
                        }
                    }
                    //isMagic=false放普攻
                    else
                    {
                        //儲存射線打到地板的座標位置
                        Target_pos = new Vector3(hit.point.x, PlayerObj.position.y, hit.point.z);
                        //使用內插法將玩家的轉向由順移變為慢慢轉向
                        Look_Pos = Vector3.Lerp(Look_Pos, Target_pos, Time.deltaTime * 10);
                        //玩家注視滑鼠/手指點擊的位置
                        PlayerObj.LookAt(Look_Pos);
                        PlayerAni.SetBool("Att", true);
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0)) {
            if (isMagic) {
                //階層下的大絕招的龍Rigidbody使用重力打勾
                MagicObjPrefab.GetComponentInChildren<Rigidbody>().useGravity = true;
                isMagic = false;
            }
            else
            {
                PlayerAni.SetBool("Att", false);
            }
        }
    }
    public void CreateFireObj() {
        Instantiate(FireObj, CreatePos.position, CreatePos.rotation);
    }
    public void ClickMagic()
    {
        //偵測大絕招圖片顏色是否為白色
        if (GameObject.Find("GM").GetComponent<GM>().MagicImage.color == Color.white) {
            isMagic = true;
        }
    }
}
