using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]

public class DialogPanel : MonoBehaviour
{
  public Text MainText = null;
  public Text NameText = null;
  public Image PortraitImage = null;
  public int CurrentLanguage = 0;//0-russian, 1-english
  private Animator thisAnimator = null;
  private CharacterMoving characterMoving = null;

  private void Start()
  {
    thisAnimator = GetComponent<Animator>();
    characterMoving = FindObjectOfType<CharacterMoving>();
  }

  public void Show()
  {
    thisAnimator.SetBool("IsVisible", true);
    characterMoving.CanMove = false;
    NameText.text = "";
    PortraitImage.enabled = false;
  }

  public void Hide()
  {
    thisAnimator.SetBool("IsVisible", false);
    characterMoving.CanMove = true;
  }

  public void SetLanguage(int lang)
  {
    CurrentLanguage = lang;
  }
}
