using UnityEngine;
using System;

[Serializable]
public class ChangeTrigger
{
  public int MyTrigger = 1;
  public TriggerBase OtherTrigger = null;
  public int OtherTriggerNewValue = 1;
}
public class TriggerBase : MonoBehaviour
{
  [SerializeField] private string textAssetName = "Klaus";
  [SerializeField] private bool automatic = false;
  [SerializeField] private ChangeTrigger[] changeTriggers = null;
  private string file = "";
  protected string[,] allBoxes;
  protected DialogPanel dialogPanel = null;
  private int currentLine = 2;
  private bool hasStartSpeaking = false;
  private bool dialogFinished = false;
  private bool canSpeak = false;
  protected CharacterMoving characterMoving = null;
  private int[] triggerNumLines = new int[1];//номера строк с которых начинаются триггеры в .csv таблице
  private int currentTrigger = 1;

  private int CurrentTrigger
  {
    get { return currentTrigger;}
    set
    {
      currentTrigger = value;
      currentLine = currentLine = triggerNumLines[currentTrigger];
    }
  }

  protected virtual void Start()
  {
    dialogPanel = FindObjectOfType<DialogPanel>();
    file = Application.dataPath + "/StreamingAssets/" + textAssetName + ".csv";
    if (System.IO.File.Exists(file))
    {
      WriteAllBoxes();
      SetTriggers();
    }
    else
    {
      Debug.LogError("File:" + file + "do not found!");
    }
  }

  private void WriteAllBoxes()
  {
    string[] lines = System.IO.File.ReadAllLines(file);
    allBoxes = new string[5, lines.Length + 1];//
    for (var lineNum = 0; lineNum < lines.Length; lineNum++)
    {
      string currentBoxText = "";
      int stolb = 0;
      for (var symbNum = 0; symbNum < lines[lineNum].Length; symbNum++)
      {
        string currentSymbol = lines[lineNum].Substring(symbNum, 1);
        if (currentSymbol == ";")
        {
          ///Запись ячейки
          allBoxes[stolb, lineNum + 1] = currentBoxText;
          currentBoxText = "";
          stolb += 1;
        }
        else
        {
          currentBoxText += currentSymbol;//Запоминаем текст ячейки
          if (symbNum == lines[lineNum].Length - 1)//Последний символ строки
            allBoxes[stolb, lineNum + 1] = currentBoxText;//Запись крайнего стобца
        }
      }
    }
  }

  private void SetTriggers()
  {
    for (int i = 1; i < allBoxes.GetLength(1) - 1; i++)
    {
      if (allBoxes[0, i].Length > 1)
      {
        Array.Resize(ref triggerNumLines, triggerNumLines.Length + 1);
        triggerNumLines[triggerNumLines.Length - 1] = i + 1;
      }
    }
    Array.Resize(ref triggerNumLines, triggerNumLines.Length + 1);
    triggerNumLines[triggerNumLines.Length - 1] = allBoxes.GetLength(1) + 1;
  }

  protected virtual void SetDialog(int line)
  {
    dialogPanel.MainText.text = allBoxes[3 + dialogPanel.CurrentLanguage, line];    
  }

  protected virtual void Update()
  {
    if (Input.GetKeyUp(KeyCode.Space))
    {
      if (hasStartSpeaking)
      {
        currentLine += 1;
        if (currentLine == allBoxes.GetLength(1) || currentLine == triggerNumLines[currentTrigger + 1] - 1)//
        {
          EndDialog(); 
        }
        else
        {
          SetDialog(currentLine);
        }
      }
      if (canSpeak && !dialogFinished && !hasStartSpeaking)
        StartDialog();
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.GetComponent<CharacterMoving>() != null)
    {
      characterMoving = other.GetComponent<CharacterMoving>();
      OnCharacterTriggerEnter();
      if (!hasStartSpeaking && !dialogFinished)
      {
        canSpeak = true;
        if (automatic)
          StartDialog();
      }    
    }    
  }

  private void OnTriggerExit2D(Collider2D other)
  {
    if (other.GetComponent<CharacterMoving>() != null)
    {
      canSpeak = false;
      dialogFinished = false;
      OnCharacterTriggerExit();
    }
  }

  protected virtual void StartDialog()
  {
    dialogPanel.Show();
    hasStartSpeaking = true;
    currentLine = triggerNumLines[currentTrigger];
    SetDialog(currentLine);
  }

  protected virtual void EndDialog()
  {
    dialogPanel.Hide();
    hasStartSpeaking = false;
    dialogFinished = true;

    foreach (var changeTrigger in changeTriggers)
    {
      if (changeTrigger.MyTrigger == CurrentTrigger)
        changeTrigger.OtherTrigger.CurrentTrigger = changeTrigger.OtherTriggerNewValue;
    }      
  }

  protected virtual void OnCharacterTriggerEnter()
  {
  }

  protected virtual void OnCharacterTriggerExit()
  {
  }  
}
