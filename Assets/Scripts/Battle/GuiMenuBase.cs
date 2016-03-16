using UnityEngine;

public class GuiMenuBase : MonoBehaviour
{
  private HeroesPanel heroesPanel = null;
  private Animator thisAnimator = null;

  private void Start()
  {
    heroesPanel = FindObjectOfType<HeroesPanel>();
    thisAnimator = GetComponent<Animator>();
  }

  public void Show()
  {
    thisAnimator.SetBool("IsVisible", true);
  }

  public void Hide()
  {
    thisAnimator.SetBool("IsVisible", false);
  }

	private void Update ()
  {
    if (Input.GetKeyDown(KeyCode.Backspace))
    {
      heroesPanel.Show();
      Hide();
    }
	}
}
