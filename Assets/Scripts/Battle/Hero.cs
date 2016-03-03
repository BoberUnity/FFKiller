using UnityEngine;

public class Hero : MonoBehaviour
{
  [SerializeField] private HeroUI heroUi = null;
  [SerializeField] private string thisName = "Hero";
  [SerializeField] private float mhp = 1000;
  [SerializeField] private float hp = 500;
  [SerializeField] private float agi = 2;
  #region Propetries 
  public string Name
  {
    get { return thisName; }
    set
    {
      thisName = value;
      heroUi.UpdateUI();
    }
  }

  public float Mhp
  {
    get { return mhp; }
    set
    {
      mhp = value;
      heroUi.UpdateUI();
    }
  }

  public float Hp
  {
    get { return hp; }
    set
    {
      hp = value;
      heroUi.UpdateUI();
    }
  }

  public float Agi
  {
    get { return agi; }
    set
    {
      agi = value;
      heroUi.UpdateUI();
    }
  }

  #endregion

  void Start () {
	
	}
	
	void Update () {
	
	}
}
