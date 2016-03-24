using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Hero))]

public class CharacterMoving : MonoBehaviour
{
  public Sprite Portrait = null;
  [SerializeField] private float speedWalk = 1;
  [SerializeField] private float speedRun = 1;
  private float currentSpeed = 0;
  private Animator thisAnimator = null;
  [HideInInspector] public bool CanMove = true;
  [HideInInspector] public bool IsBlocked = true;
  public float HandlingDelay;
  private float TimeToHandle;

    //DV{
    public SpriteRenderer CharSpriteRenderer;
    //DV}
    private void Start ()
    {
        thisAnimator = GetComponent<Animator>();
        GetComponent<Hero>().ConnectToPartyGui();    
        //DV{
        CharSpriteRenderer = GetComponent<SpriteRenderer>();
        //DV}
    }

    //DV{
    void Update()
    {
        CharSpriteRenderer.sortingOrder = (int)(-transform.position.y * 2);
        TimeToHandle -= Time.deltaTime;
        if (TimeToHandle < 0f)
        {
            MovingHandling();
            TimeToHandle = HandlingDelay;
        }
    }
    //DV}

    private void MovingHandling()
  {
    if (CanMove && !IsBlocked)
    {
      currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speedRun : speedWalk;
      if (Input.GetKey(KeyCode.UpArrow))
      {
        transform.position += Vector3.up * Time.deltaTime * currentSpeed;
        thisAnimator.SetBool("Running", true);
        thisAnimator.SetFloat("SpeedY", 1);
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
          thisAnimator.SetFloat("SpeedX", 0);
        }
      }

      if (Input.GetKey(KeyCode.DownArrow))
      {
        transform.position -= Vector3.up * Time.deltaTime * currentSpeed;
        thisAnimator.SetBool("Running", true);
        thisAnimator.SetFloat("SpeedY", -1);
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
          thisAnimator.SetFloat("SpeedX", 0);
        }
      }      

      if (Input.GetKey(KeyCode.RightArrow))
      {
        transform.position += Vector3.right * Time.deltaTime * currentSpeed;
        thisAnimator.SetBool("Running", true);
        thisAnimator.SetFloat("SpeedX", 1);
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
          thisAnimator.SetFloat("SpeedY", 0);
        }
      }      
      
      if (Input.GetKey(KeyCode.LeftArrow))
      {
        transform.position -= Vector3.right * Time.deltaTime * currentSpeed;
        thisAnimator.SetBool("Running", true);
        thisAnimator.SetFloat("SpeedX", -1);
        if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))
        {
          thisAnimator.SetFloat("SpeedY", 0);
        }
      } 

      //DV{
      thisAnimator.SetFloat("Speed", currentSpeed);
        //DV}
    }

    if ((!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.DownArrow)) || !CanMove)
        thisAnimator.SetBool("Running", false);    
  }
}
