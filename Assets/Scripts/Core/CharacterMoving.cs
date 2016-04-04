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
        CharSpriteRenderer.sortingOrder = (int)(-transform.position.y * 2);

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

        TimeToHandle -= Time.deltaTime;
        if (TimeToHandle < 0f)
        {
            MovingHandling();
            TimeToHandle = HandlingDelay;
        }
    }


    private void MovingHandling()
  {
        float SpeedX = 0f, SpeedY = 0f;
        Vector3 deltaMove = new Vector3();

        currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speedRun : speedWalk;

        if (up)
        {
            deltaMove += Vector3.up * Time.deltaTime * currentSpeed;
            SpeedY = 1;
        }

        if (down)
        {
            deltaMove -= Vector3.up * Time.deltaTime * currentSpeed;
            SpeedY = -1;
        }

        if (right)
        {
            deltaMove += Vector3.right * Time.deltaTime * currentSpeed;
            SpeedX = 1;
        }

        if (left)
        {
            deltaMove -= Vector3.right * Time.deltaTime * currentSpeed;
            SpeedX = -1;
        }

        //DV{
        if ((left || right) && (up || down)) //движение по диагонали
            deltaMove *= 0.7f;

        if (left || right || up || down)
        {
            transform.position += deltaMove;

            thisAnimator.SetBool("Running", true);
            thisAnimator.SetFloat("SpeedX", SpeedX);
            thisAnimator.SetFloat("SpeedY", SpeedY);
            thisAnimator.SetFloat("Speed", currentSpeed);
        }
        else
            thisAnimator.SetBool("Running", false);
        //DV}

    }
}
