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
  /*[HideInInspector]*/ public bool KeyboardControl = true;
  [HideInInspector] public Direction AutoMoveDirection = Direction.None;
  public float HandlingDelay;
  private float TimeToHandle;
  private bool left = false;
  private bool right = false;
  private bool up = false;
  private bool down = false;

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

    
    void Update()
    {
    //DV{
        CharSpriteRenderer.sortingOrder = (int)(-transform.position.y * 2);
        TimeToHandle -= Time.deltaTime;
        if (TimeToHandle < 0f)
        {
            MovingHandling();
            TimeToHandle = HandlingDelay;
        }
    //DV}
    if (KeyboardControl)
    {
      left = Input.GetKey(KeyCode.LeftArrow);
      right = Input.GetKey(KeyCode.RightArrow);
      up = Input.GetKey(KeyCode.UpArrow);
      down = Input.GetKey(KeyCode.DownArrow);
    }
    else
    {
      left = AutoMoveDirection == Direction.Left;
      right = AutoMoveDirection == Direction.Right;
      up = AutoMoveDirection == Direction.Up;
      down = AutoMoveDirection == Direction.Down;
    }
  }


  private void MovingHandling()
  {
    currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speedRun : speedWalk;
    if (up)
    {
      transform.position += Vector3.up * Time.deltaTime * currentSpeed;
      thisAnimator.SetBool("Running", true);
      thisAnimator.SetFloat("SpeedY", 1);
      if (!right && !left)       
        thisAnimator.SetFloat("SpeedX", 0);
      
    }

    if (down)
    {
      transform.position -= Vector3.up * Time.deltaTime * currentSpeed;
      thisAnimator.SetBool("Running", true);
      thisAnimator.SetFloat("SpeedY", -1);
      if (!right && !left)      
        thisAnimator.SetFloat("SpeedX", 0);
      
    }      

    if (right)
    {
      transform.position += Vector3.right * Time.deltaTime * currentSpeed;
      thisAnimator.SetBool("Running", true);
      thisAnimator.SetFloat("SpeedX", 1);
      if (!up && !down)      
        thisAnimator.SetFloat("SpeedY", 0);
      
    }      
      
    if (left)
    {
      transform.position -= Vector3.right * Time.deltaTime * currentSpeed;
      thisAnimator.SetBool("Running", true);
      thisAnimator.SetFloat("SpeedX", -1);
      if (!up && !down)      
        thisAnimator.SetFloat("SpeedY", 0);
      
    } 

      //DV{
    thisAnimator.SetFloat("Speed", currentSpeed);
        //DV}
    

    if (!left && !right && !up && !down)
        thisAnimator.SetBool("Running", false);    
  }
}
