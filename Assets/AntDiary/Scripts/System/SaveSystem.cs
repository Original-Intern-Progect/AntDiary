using System.Collections;
using System.Collections.Generic;
using System.IO;
using MessagePack;
using UnityEngine;

namespace AntDiary
{
    public class SaveSystem : IDebugMenu
    {
        
        public void SaveCurrentSaveUnit(int number)
        {
            SaveSaveFile(number, SaveUnit.Current);
        }

        public void LoadSaveUnitToCurrent(int number)
        {
            LoadSaveFile(number).SetAsCurrent();
        }

        public void LoadDefaultSaveUnitToCurrent()
        {
            SaveUnit.GetDefaultSaveUnit().SetAsCurrent();
        }
        
        

        private SaveUnit SaveSaveFile(int number, SaveUnit su)
        {
            string fileName = GetSaveFileName(number);
            byte[] raw = MessagePackSerializer.Serialize(su);
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            System.IO.File.WriteAllBytes(fileName, raw);
            return su;
        }
        
        private SaveUnit LoadSaveFile(int number)
        {
            string fileName = GetSaveFileName(number);
            byte[] raw = System.IO.File.ReadAllBytes(fileName);
            var su = MessagePackSerializer.Deserialize<SaveUnit>(raw);
            return su;
        }

        private string GetSaveFileName(int number)
        {
            return $"{Application.persistentDataPath.TrimEnd('/', '\\')}/Save/{number}/saveunit.bin";
        }
        
        #region Debug
        public string pageTitle { get; } = "セーブ/ロード";
        public void OnGUIPage()
        {
            if (SaveUnit.Current == null)
            {
                GUILayout.Label("セーブデータは未ロードです");
            }
            
            if (GUILayout.Button($"初期状態のデータをロード"))
            {
                LoadDefaultSaveUnitToCurrent();
            }

            for (int i = 0; i < 4; i++)
            {
                GUILayout.BeginHorizontal();
                
                if (GUILayout.Button($"Load from {i}"))
                {
                    LoadSaveUnitToCurrent(i);
                }
                GUI.enabled = SaveUnit.Current != null;
                if (GUILayout.Button($"Save to {i}"))
                {
                    SaveCurrentSaveUnit(i);
                }
                GUI.enabled = true;
                GUILayout.EndHorizontal();
            }
        }
        #endregion
    }
}