using UnityEngine;
using System.Collections.Generic;

public class HeroesPanel : MonoBehaviour
{
  [SerializeField] private GameObject[] menus = null;
  public List<HeroUI> HeroesUi = new List<HeroUI>(4);
  private int attachedHeroes = 0;
  private Animator thisAnimator = null;

  public HeroUI AttachHero (Hero hero)
  {
    if (attachedHeroes < 4)
    {
      HeroesUi[attachedHeroes].gameObject.SetActive(true);
      HeroesUi[attachedHeroes].Hero = hero;      
      ++attachedHeroes;
      return HeroesUi[attachedHeroes - 1];
    }
    Debug.LogWarning("You try attach more than 4 heroes");
    return null;
  }

  private void Start()
  {
    thisAnimator = GetComponent<Animator>();
  }

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.Tab))
      thisAnimator.SetBool("IsVisible", !thisAnimator.GetBool("IsVisible"));
  }

  public void SelectMenu(int activeMenu)
  {
    int i = 0;
    foreach (var menu in menus)
    {
      menu.SetActive(i == activeMenu);
      i++;
    }
  }
}
