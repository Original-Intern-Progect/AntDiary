﻿using System.Collections;
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
    [SerializeField] ChangeBackground changeBackground;
    [SerializeField] FadeOut fadeOut;
    private void Start()
    {
      PlayWinterBGMornot = 0;
      season=0;
      //Wintertext.text = "";  
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
            //Wintertext.text = "Winter";
            if(PlayWinterBGMornot == 0){
                changeBackground.SetBackground(3);
                fadeOut.BGMsystem("ari_winter_master");
                PlayWinterBGMornot = 1;
            }
        }else{
            PlayWinterBGMornot = 0;
        }
    } 
}
