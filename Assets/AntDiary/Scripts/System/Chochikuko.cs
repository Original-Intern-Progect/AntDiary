using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AntDiary;

public class Chochikuko : MonoBehaviour
{
    public Sprite Esaari;
    public Sprite Esanashi;
    private SpriteRenderer spriteRenderer;

    // デバッグ用に季節とアリのステータスを指定
    public int season;
    public int status;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        // 貯蓄庫の初期の貯蓄量を0とする
        NestSystem.Instance.Data.StoredFood = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(NestSystem.Instance.Data.StoredFood);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject colliderObj = collision.gameObject;
        // DebugaAntMoveByKeyはデバッグ用のクラス名，のちにAntDataに変更
        // scriptは仮の変数名，isHoldingFoodのbool値が取得できるスクリプトを登録
        DebugAntMoveByKey script = colliderObj.GetComponent<DebugAntMoveByKey>();

        if (script.isHoldingFood)
        {
            // 貯蓄部屋のストックが0から増える際はspriteを餌ありに変更
            if (NestSystem.Instance.Data.StoredFood == 0)
            {
                spriteRenderer.sprite = Esaari;
            }
            NestSystem.Instance.Data.StoredFood += season * status;
            script.isHoldingFood = false;
        }
    }
}
