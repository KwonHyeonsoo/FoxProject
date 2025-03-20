using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public List<Dictionary<string, object>> data_dialogue1;
    private int story_cursor = 0;
    // Start is called before the first frame update
    public void Start()
    {
        data_dialogue1 = CSVReader.Read("Dialogue/dialogue1");
       // Debug.Log(data_dialogue1[0]["TEXT"]);
    }

    public int CheckCurrentStoryID()
    {
        if (data_dialogue1.Count <= story_cursor) return -1;
        return int.Parse( data_dialogue1[story_cursor]["ID"].ToString());
    }

    public string PassStoryText()
    {
        return data_dialogue1[story_cursor++]["TEXT"].ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
