using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DialogLoader : MonoBehaviour {
    public List<Dialog> context = new List<Dialog>();
    private string dialogDataPath;
    
    public void loadData()
    {
        dialogDataPath = Application.dataPath + "/Resources/dialog.txt";
        Debug.Log(File.Exists(dialogDataPath));
        if (File.Exists(dialogDataPath))
        {
            string[] strs = File.ReadAllLines(dialogDataPath);
            foreach (string rawData in strs)
            {
                if (rawData == "")
                    continue;
                string[] data = rawData.Replace("\\", "\n").Split('|');
                Dialog dialog = new Dialog();
                dialog.id = int.Parse(data[0]);
                dialog.section = data[1];
                dialog.characterName = data[2];
                dialog.text = data[3];
                dialog.imagePath = data[4];
                dialog.branchNum = int.Parse(data[5]);
                dialog.branches = new List<Dialog.branch>();
                if (dialog.branchNum > 0)
                {
                    for (int i=0; i<dialog.branchNum; i++)
                    {
                        Dialog.branch branch = new Dialog.branch();
                        branch.switch_section = data[6 + i * 2];
                        branch.text = data[6 + i * 2 + 1];
                        dialog.branches.Add(branch);
                    }
                }
                Debug.Log(dialog.text);
                context.Add(dialog);
            }
        }
    }
}
