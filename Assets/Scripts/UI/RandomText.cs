using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class RandomText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lineText;
    private string[] lines = new string[0];
    private List<string> lastlines = new List<string>();

    private string line;
    int randomIndex;

    private void OnEnable() 
    {
        if (lastlines.Count > 5)
            lastlines.RemoveAt(0);

        try
        {
            LoadTextFile();
            SetLine();
            lineText.text = line;
            lastlines.Add(line);
        }
        catch(System.Exception e)
        {
            lineText.text = e.ToString();                                            
        }
    }

    private void SetLine()
    {
        randomIndex = Random.Range(0, lines.Length);
        line = lines[randomIndex];

        if (lastlines.Contains(line))
            SetLine();
    }

#if DEBUG

    private void LoadTextFile()
    {
        lines = File.ReadAllLines("Assets/puns.txt");
    }

#else

    [System.Serializable]
    private class SerializationClass
    {
        public string[] texts;
    }

    private void Update() 
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
            SerializeText(); 
    }

    private SerializationClass serObj;
    private void SerializeText()
    {
        serObj = new SerializationClass{
            texts = File.ReadAllLines("Time Pass Runner_Data/puns.txt")
        };

        using (FileStream fs = new FileStream("Time Pass Runner_Data/guitext", FileMode.Create))
        {
            BinaryFormatter binForm = new BinaryFormatter();
            binForm.Serialize(fs, serObj);
        }
    }

    private void LoadTextFile()
    {
        using (FileStream fs = new FileStream("Time Pass Runner_Data/guitext", FileMode.Open))
        {
            BinaryFormatter binForm = new BinaryFormatter();
            serObj = (SerializationClass)binForm.Deserialize(fs);
        }

        lines = serObj.texts;
    }

#endif

}
