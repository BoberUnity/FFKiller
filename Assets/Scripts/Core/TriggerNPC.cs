using UnityEngine;

public class TriggerNPC : TriggerBase
{
  public Sprite Portrait = null;
  [SerializeField] private Animator bodyAnimator = null;
  protected Animator thisAnimator = null;
  private Vector3 previousPosition = Vector3.zero;
  private bool isRotateToCharacter = false;

  //DV{
  public SpriteRenderer CharSpriteRenderer;
  //DV}

    protected override void Start()
  {
    base.Start();
    previousPosition = transform.position;
    thisAnimator = GetComponent<Animator>();
    if (thisAnimator != null)
      thisAnimator.speed = Random.value * 0.5f + 0.5f;
  }

  protected override void SetDialog(int line)
  {
    base.SetDialog(line);
    dialogPanel.NameText.text = allBoxes[1 + dialogPanel.CurrentLanguage, line];
    Sprite otherPortrait = characterMoving.Portrait;
    dialogPanel.PortraitImage.enabled = true;
    dialogPanel.PortraitImage.sprite = allBoxes[0, line] == "1" ? Portrait : otherPortrait;    
  }

  protected override void StartDialog()
  {
    base.StartDialog();    
    bodyAnimator.SetBool("Running", false);    
    if (thisAnimator != null)
      thisAnimator.speed = 0;
  }

  protected override void Update()
  {
    base.Update();
    //DV{
    CharSpriteRenderer.sortingOrder = (int)(-transform.position.y*2);
    //DV}    
  }

  private void FixedUpdate()
  {
    if (!isRotateToCharacter)
    {
      bodyAnimator.SetFloat("SpeedX", transform.position.x - previousPosition.x);
      bodyAnimator.SetFloat("SpeedY", transform.position.y - previousPosition.y);
    }
    bodyAnimator.SetBool("Running", previousPosition != transform.position);
    previousPosition = transform.position;
  }

  protected override void OnCharacterTriggerEnter()
  {
    if (thisAnimator != null)
      thisAnimator.speed = 0;
    bodyAnimator.SetFloat("SpeedX", isCharacterRight ? 1 : -1);
    isRotateToCharacter = true;
  }

  protected override void OnCharacterTriggerExit()
  {
    if (thisAnimator != null)
      thisAnimator.speed = 1;
    isRotateToCharacter = false;
  }  
}
