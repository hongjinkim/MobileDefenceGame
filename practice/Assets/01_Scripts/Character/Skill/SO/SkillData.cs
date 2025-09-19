using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SkillData
{
    [ShowInInspector, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout, KeyLabel = "Hero ID", ValueLabel = "Skill Info")]
    public Dictionary<string, List<SkillValue>> SkillDict = new Dictionary<string, List<SkillValue>>();


    public void LoadData()
    {
        SkillDict.Clear();
        var SkillDataList = DataTable.Skill.GetList();
        foreach (var skill in SkillDataList)
        {
            if (!SkillDict.ContainsKey(skill.Hero_ID))
            {
                SkillDict.Add(skill.Hero_ID, new List<SkillValue>());
            }

            var skillValue = new SkillValue();

            


            SkillDict[skill.Hero_ID].Add(skillValue);
        }
    }

}
