using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AntDiary;
using UnityEngine.Audio;

public class Winter : MonoBehaviour
{

    public Text Wintertext;
    private int PlayWinterBGMornot;
    private float season;
    private void Start()
    {
      PlayWinterBGMornot = 0;
      season=0;
      Wintertext.text = "";  
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        season = TimeSystem.Instance.CurrentSeason;
        Wintercheck();
    }

    private void Wintercheck()
    {
        if(season==3){
            Wintertext.text = "Winter";
            if(PlayWinterBGMornot == 0){
               //GameObject.Find("冬BGM");
                PlayWinterBGMornot = 1;
            }
        }else{
            PlayWinterBGMornot = 0;
        }
    } 
}
