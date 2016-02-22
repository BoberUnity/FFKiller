using UnityEngine;

public class Dialog : MonoBehaviour
{
  [SerializeField] private string textAssetName = "Klaus";
  [SerializeField] private bool automatic = false;
  [SerializeField] private Sprite myPortrait = null;
  [SerializeField] private Sprite otherPortrait = null;
  private string file = "";
  private string[,] allBoxes;
  protected DialogPanel dialogPanel = null;
  private int currentLine = 2;
  private bool hasStartSpeaking = false;
  private bool dialogFinished = false;
  private bool canSpeak = false;

  protected virtual void Start()
  {
    dialogPanel = FindObjectOfType<DialogPanel>();
    file = Application.dataPath + "/StreamingAssets/" + textAssetName + ".csv";
    if (System.IO.File.Exists(file))
    {
      WriteAllBoxes();
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

  private void SetDialog(int line)
  {
    dialogPanel.MainText.text = allBoxes[3 + dialogPanel.CurrentLanguage, line];
    dialogPanel.NameText.text = allBoxes[1 + dialogPanel.CurrentLanguage, line];
    dialogPanel.PortraitImage.sprite = allBoxes[0, line] == "0" ? otherPortrait : myPortrait;
  }

  protected virtual void Update()
  {
    if (Input.GetKeyUp(KeyCode.Space))
    {
      if (hasStartSpeaking)
      {
        currentLine += 1;
        if (currentLine < allBoxes.GetLength(1))
        {
          SetDialog(currentLine);
        }
        else
        {
          EndDialog();
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
      OnCharacterTriggerExit();
    }
  }

  protected virtual void StartDialog()
  {
    dialogPanel.Show();
    hasStartSpeaking = true;
    currentLine = 2;
    SetDialog(currentLine);
  }

  protected virtual void EndDialog()
  {
    dialogPanel.Hide();
    hasStartSpeaking = false;
    dialogFinished = true;
  }

  protected virtual void OnCharacterTriggerEnter()
  {
  }

  protected virtual void OnCharacterTriggerExit()
  {
  }  
}
