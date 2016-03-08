using UnityEngine;
using UnityEngine.UI;
using System;
using System.Xml;
using System.IO;
using UnityEngine.SceneManagement;
using System.Globalization;
using System.Collections.Generic;

[Serializable] public class Atribut
{
  public string Name = "";
  public string Value = "";
}

public class SaveController : MonoBehaviour
{
  [SerializeField] private int firstScene = 1;
  [SerializeField] private Button loadButton = null;
  private string sceneName = "";
  private string filePath = "";
  private bool isLoadSavedGame = false;
  private List<Atribut> atributs = new List<Atribut>();

  private void Start()
  {    
    DontDestroyOnLoad(this);
    filePath = Application.dataPath + "/StreamingAssets/Saves/LastScene.xml";
    loadButton.interactable = File.Exists(filePath);    
  }

  private void OnLevelWasLoaded(int level)
  {
    sceneName = SceneManager.GetActiveScene().name;
    filePath = Application.dataPath + "/StreamingAssets/Saves/" + sceneName + "Data.xml";    
    if (isLoadSavedGame)
      Invoke("LoadGame", 0);
  }

  void SaveGame()
  {   
    XmlDocument doc = new XmlDocument();
    XmlNode rootNode = null;
    if (!File.Exists(filePath))
    {      
      rootNode = doc.CreateElement("Triggers");      
      doc.AppendChild(rootNode);
    }
    else
    {
      doc.Load(filePath);
      rootNode = doc.DocumentElement;
    }
    rootNode.RemoveAll();
    //Save Triggers
    TriggerBase[] triggersBase = FindObjectsOfType<TriggerBase>();
    foreach (var triggerBase in triggersBase)
    {
      AddAtribute("trigger", (triggerBase.CurrentTrigger).ToString());
      AddAtribute("automatic", triggerBase.automatic ? "1" : "0");
      AddXmlElements(doc, rootNode, "Trigger" + triggerBase.gameObject.name);
    }
    //Save Character Position    
    GameObject[] savePositionOjects = GameObject.FindGameObjectsWithTag("SavePosition");
    foreach (var savePositionOject in savePositionOjects)
    {
      Vector3 pos = savePositionOject.transform.position;
      AddAtribute("positionX", (pos.x).ToString());
      AddAtribute("positionY", (pos.y).ToString());
      AddXmlElements(doc, rootNode, "SavePosition" + savePositionOject.name);
    }    
    //    
    doc.Save(filePath);
    ///////////////////////Save current Scene
    doc = new XmlDocument();    
    filePath = Application.dataPath + "/StreamingAssets/Saves/LastScene.xml";
    if (!File.Exists(filePath))
    {
      rootNode = doc.CreateElement("LastSceneData");
      doc.AppendChild(rootNode);
    }
    else
    {
      doc.Load(filePath);
      rootNode = doc.DocumentElement;
    }
    rootNode.RemoveAll();
    AddAtribute("lastScene", sceneName);
    AddXmlElements(doc, rootNode, "LastScene");    
    doc.Save(filePath);
  }

  private void AddXmlElements(XmlDocument d, XmlNode node, string name)
  {    
    XmlElement e = d.CreateElement(name);
    foreach (var atribut in atributs)
    {
      XmlAttribute atr = d.CreateAttribute(atribut.Name);
      atr.Value = atribut.Value;
      e.SetAttributeNode(atr);
    }    
    node.AppendChild(e);
    atributs.Clear();
  }

  private void AddAtribute(string name, string value)
  {
    Atribut newAtr = new Atribut();
    newAtr.Name = name;
    newAtr.Value = value;
    atributs.Add(newAtr);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.S))
    {
      SaveGame();
    }
    if (Input.GetKeyDown(KeyCode.L))
    {
      LoadGame();
    }
  }

  void LoadGame()
  {
    isLoadSavedGame = false;
    XmlDocument doc = new XmlDocument();
    XmlNodeList elemList;
    doc.Load(filePath);
    //LoadTriggers
    TriggerBase[] triggersBase = FindObjectsOfType<TriggerBase>();
    foreach (var triggerBase in triggersBase)
    {
      elemList = doc.GetElementsByTagName("Trigger" + triggerBase.gameObject.name);
      for (int i = 0; i < elemList.Count; i++)
      {
        triggerBase.CurrentTrigger = Int32.Parse(elemList[i].Attributes["trigger"].Value);
        triggerBase.automatic = elemList[i].Attributes["automatic"].Value == "1" ? true : false;
      }
    }
    //Load Save Position Objects position
    GameObject[] savePositionOjects = GameObject.FindGameObjectsWithTag("SavePosition");
    foreach (var savePositionOject in savePositionOjects)
    {
      elemList = doc.GetElementsByTagName("SavePosition" + savePositionOject.name);
      for (int i = 0; i < elemList.Count; i++)
      {
        float x = Convert.ToSingle(elemList[i].Attributes["positionX"].Value, new CultureInfo("en-US"));
        float y = Convert.ToSingle(elemList[i].Attributes["positionY"].Value, new CultureInfo("en-US"));
        savePositionOject.transform.position = new Vector3(x, y, 0);
      }
    }      
  }
  
  public void PressButtonNewGame()
  {
    SceneManager.LoadScene(firstScene);
  }  

  public void PressButtonLoadGame()
  {
    isLoadSavedGame = true;
    filePath = Application.dataPath + "/StreamingAssets/Saves/LastScene.xml";
    XmlDocument doc = new XmlDocument();
    doc.Load(filePath);
    XmlNodeList elemList = doc.GetElementsByTagName("LastScene");
    
    for (int i = 0; i < elemList.Count; i++)
    {      
      string lastSceneName = elemList[i].Attributes["lastScene"].Value;      
      SceneManager.LoadScene(lastSceneName);
    }
  }
}


