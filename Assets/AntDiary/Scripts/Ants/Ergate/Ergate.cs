using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntDiary;
using System.Linq;

//餌運びプログラム
//一つの巣から往復するプログラムを作成
public class Ergate : MonoBehaviour
{
    ErgateAntData data;
    public ErgateAnt HostAnt;
    public NestPathNode nodeNest;

    //デバッグ用
    public NestPathNode nodeFeed;

         
    void Start()
    {
        data = (ErgateAntData)GetComponent<ErgateAnt>().Data;
        //HostAnt = new ErgateAnt();
        HostAnt = (ErgateAnt)GetComponent<ErgateAnt>();
        //目的地の設定
        var elements = NestSystem.Instance.NestElements;
        Debug.Log(elements.Count);
        foreach (var element in elements)
        {
            if (element.GetType().Name.Equals("StoreRoom"))
            {
                nodeNest = element.GetNodes().ElementAt(0);
            }
        }
       
       
        //以下デバッグ用
        Vector3 a = HostAnt.transform.position;
        a.z = -1;
        HostAnt.transform.position = a;
        HostAnt.transform.localScale = new Vector3(1/2f, 1/2f, 1);

        nodeFeed = NestSystem.Instance.NestElements[1].GetNodes().FirstOrDefault(n => n.Name == "top");
        //nodeFeed = null;
        if(nodeFeed ==null)Debug.Log("nullです");               
        ErgateAntStart(nodeFeed);
        //ここまで
    }

    private void Cancel(){
        Debug.Log("Canceled");
        HostAnt.CancelMovement();
    }
    // Update is called once per frame
    
    //餌を獲得
    private void GetFeed()
    {
        data.IsHoldingFood = true;
        Debug.Log("We got feed!!!");
        //巣に帰還
        HostAnt.StartForPathNode(nodeNest , LeaveFeed , Cancel);
    }

    //餌を離す
    private void LeaveFeed()
    {
        //餌を持ってない状態にする
        data.IsHoldingFood = false;
        Debug.Log("Finish!");
    }

    //Debug用スタートプログラム
    private void ErgateAntStart(NestPathNode target){
        if(target != null){
            Debug.Log("start!");
            HostAnt.StartForPathNode(target , GetFeed , Cancel);
        }else{
            Debug.Log("働きたくないでござる");
        }        
    }
}

