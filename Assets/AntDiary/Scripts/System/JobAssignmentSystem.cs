﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.ComponentModel;
using UnityEngine;

namespace AntDiary
{
    /// <summary>
    /// 仕事割り振りクラス
    /// </summary>
    public class JobAssignmentSystem : MonoBehaviour
    {
        //仕事の型リスト
        private List<Type> antjobs = new List<Type>{ typeof(BuilderAntData),typeof(SoldierAntData),typeof(ErgateAntData),typeof(UnemployedAntData)};
        private int jobCount;
        int total;
        //理想の割合
        public float[] idealrate;
        private NestData nestdata => NestSystem.Instance?.Data;
        Dictionary<Type, int> antCounter = new Dictionary<Type, int>();

        public float ideal_Architect = 0f;
        public float ideal_Soilder = 0f;
        public float ideal_Mule = 0f;
        public float ideal_Free = 100.0f;

        void Start()
        {
            jobCount = antjobs.Count;

            //理想値の初期値を設定
            idealrate = new float []{0f,0f,0f,100f };
            ideal_Free = 100f;
        }

        /// <summary>
        /// 仕事割り振り関数
        /// </summary>
        /// <returns>新しく割り振る仕事のtype</returns>
        public void AssignJob(Ant ant)
        {
            InitAntCounter();

            //理想値の取得
            for (int i = 0; i < jobCount; i++)
            {
                if (antjobs[i].Name == "BuilderAntData")
                {
                    idealrate[i] = ideal_Architect;
                }
                else if (antjobs[i].Name == "SoldierAntData")
                {
                    idealrate[i] = ideal_Soilder;
                }
                else if (antjobs[i].Name == "ErgateAntData")
                {
                    idealrate[i] = ideal_Mule;
                }
                else if (antjobs[i].Name == "UnemployedAntData")
                {
                    idealrate[i] = ideal_Free;
                }
            }

            //diffに現在と理想の割合の差を保存
            float[] diff = new float[jobCount];
            int[] index = new int[jobCount];
            List<int> current = new List<int>(antCounter.Values);

            for (int i = 0; i < idealrate.Length; i++)
            {
                diff[i] = 100.0f * current[i] / total - idealrate[i];
                index[i] = i;
            }

            //diffを元にindexをソート
            Array.Sort(diff,index);
            //一番理想より少ない役職を取得
            Type nextjob = antjobs[index[0]];

            //別の仕事に割り振るのであれば場所だけコピーして元のアリは殺す
            if (nextjob.GetType() == ant.Data.GetType()) return;

            AntData newantdata = (AntData)Activator.CreateInstance(nextjob);
            var newant = NestSystem.Instance.InstantiateAnt(newantdata);
            newant.transform.position = ant.Data.Position;

            ant.Data.IsAlive = false;
            NestSystem.Instance.RemoveAnt(ant);
            Destroy(ant.gameObject);
        }

        /// <summary>
        /// antCounterの再計算
        /// </summary>
        public void InitAntCounter()
        {
            antCounter.Clear();
            //antjobsを元にantCounter設定
            for (int i = 0; i < jobCount; i++)
            {
                antCounter.Add(antjobs[i], 0);
            }

            //生きているアリの総数と仕事ごとの数をカウント
            total = 0;
            foreach (var ant in nestdata.Ants)
            {
                Type antjob = ant.GetType();
                if (antCounter.ContainsKey(antjob) && ant.IsAlive)
                {
                    antCounter[antjob]++;
                    total++;
                }
            }
        }
    }
}