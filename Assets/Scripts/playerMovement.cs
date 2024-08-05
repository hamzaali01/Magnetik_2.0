using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    [SerializeField] private Animator[] _animator;
    private CharacterAnimationController _animationControllerPlayer1;
    private CharacterAnimationController _animationControllerPlayer2;



    public float speed = 6.0F;

    public bool canMove = false;

    //Animation hitAnimation;

    public AudioSource AudioSource1;
    public AudioClip RunSfx;
    private bool RunSfxHasPlayed = false;

    Rigidbody rb;

    //public float rotateSpeed = 3.0F;
    public FloatingJoystick _joystick;

    public bool PowerActivated = false;

    public bool isSlipping = false;
    public bool isHitting = false;

    //below is testing
    public float slidingForce = 10.0f;
    public float slidingDamping = 0.1f;
     private Vector3 _slidingDirection;
    public  bool IsOnSlipperySurface = false;

    //
    public bool isJumping = false;
    //       public float gravityScale = 1.0f;
  
    // public  float globalGravity = -9.81f;

    public bool onGround = true;

    public float horzOffset = 0;
    public float vertOffset = 0;

    Vector3 player1InitialPos;
    Vector3 player2InitialPos;

    public GameObject currentPlayer;

    public GameObject player1;
    public GameObject player2;

    private CharacterAnimationController _animationControllerCurrent;

    private bool switchPlayer = false;

    public float Player1Magnetized = 0f;
     public float Player2Magnetized = 0f;



    void Start()
    {

        player1  = GameObject.FindWithTag("Player");
        player2  = GameObject.FindWithTag("Player2");

            _animationControllerPlayer1 = new CharacterAnimationController(_animator[0]);
            _animationControllerPlayer2 = new CharacterAnimationController(_animator[1]);
       

        // rb = GetComponent<Rigidbody>();

        transform.GetChild(ManageGameState.selectedCharacter).gameObject.SetActive(true);

        canMove = false;
        StartCoroutine(LetMove());
        
        //player1 = gameObject;
        currentPlayer = player1;
        _animationControllerCurrent = _animationControllerPlayer1;

        player1InitialPos =  player1.transform.position;
        player2InitialPos = player2.transform.position;


      //  rb.useGravity = false;
    }

    // Update is called once per frame
    void Update() {
         if(Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.S)){
          switchPlayer = true;
         }
    }
    void FixedUpdate()
    {
        if (ManageGameState.gameIsPaused == false)
        {

                             
            // Vector3 gravity = globalGravity * gravityScale * Vector3.up;
            // Vector3 gravity =  gravityScale * Vector3.up;
            // rb.AddForce(gravity, ForceMode.Acceleration);

            if(switchPlayer==true){
                _animationControllerCurrent.StopAnimation(AnimationType.Move);
                AudioSource1.Stop();
                RunSfxHasPlayed = false;

                if(currentPlayer==player1){
                    currentPlayer = player2;
                    _animationControllerCurrent = _animationControllerPlayer2;
                }
                else if(currentPlayer==player2){
                    currentPlayer = player1;
                     _animationControllerCurrent = _animationControllerPlayer1;
                    
                }
                switchPlayer = false;
             
            }

            if (canMove == true)
            {
                float horizontal = _joystick.Horizontal;
                float vertical = _joystick.Vertical;


                // if (transform.position.z < ManageGameState.backwardBoundary && vertical < 0)
                // {
                //     vertical = 0;
                // }
                // if (transform.position.z > ManageGameState.forwardBoundary && vertical > 0)
                // {
                //     vertical = 0;
                // }
                if (player1.transform.position.y < ManageGameState.fallBoundary)
                {
                    player1.transform.position = player1InitialPos;
                    
                }
                if (player2.transform.position.y < ManageGameState.fallBoundary)
                {
                    player2.transform.position = player2InitialPos;
                    
                }


                if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
                currentPlayer.GetComponent<Rigidbody>().velocity = new Vector3((horizontal+horzOffset) * speed,currentPlayer.GetComponent<Rigidbody>().velocity.y, (vertical+vertOffset) * speed);

                if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
                {
                    if ( currentPlayer.GetComponent<Rigidbody>().velocity != Vector3.zero)
                        currentPlayer.transform.rotation = Quaternion.LookRotation( currentPlayer.GetComponent<Rigidbody>().velocity);

                    _animationControllerCurrent.PlayAnimation(AnimationType.Move);
                    if (!RunSfxHasPlayed)
                    {
                        AudioSource1.volume = 0.2f;
                        AudioSource1.PlayOneShot(RunSfx);
                        RunSfxHasPlayed = true;
                    }

                }
                else
                {
                    _animationControllerCurrent.StopAnimation(AnimationType.Move);
                    AudioSource1.Stop();
                    RunSfxHasPlayed = false;
                   // rb.velocity = Vector3.zero;

                }


                if (gameObject.transform.rotation.eulerAngles.x >= 2)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }

            }
            else
            {
                // transform.rotation = Quaternion.Euler(0, 0, 0);
               // rb.velocity = Vector3.zero;
                _animationControllerCurrent.StopAnimation(AnimationType.Move);
                AudioSource1.Stop();
                RunSfxHasPlayed = false;

            }


        }
        else
        {
            //Time.timeScale = 0f;
        }
    }



    IEnumerator LetMove(){
       yield return new WaitForSeconds(2f);
       canMove=true;
    }
    //     public bool GetJumpState()
    // {
    //     return _animationController.GetJumpState();
    // }


    // void OnCollisionEnter(Collision collisionInfo)
    // {
    //             if(collisionInfo.collider.tag == "Ground" && onGround==false){
    //         Debug.Log("hitting the ground");
    //         PlayJumpAnimation(false);
    //         onGround=true;
    //     }
    // }


    //  void OnCollisionExit(Collision collisionInfo)
    // {
    //             if(collisionInfo.collider.tag == "Ground"){
    //         Debug.Log("Leaving the ground");
    //         StartCoroutine(InAir());
    //         //PlayJumpAnimation(false);
    //     }
    // }

    // IEnumerator InAir(){
    //         yield return new WaitForSeconds(0.1f);
    //         onGround = false;
    // }

    // void OnCollisionEnter(Collision collisionInfo)
    // {
    //       if(collisionInfo.collider.tag == "Ground" && onGround==false){
    //         Debug.Log("Back on ground");
    //           PlayJumpAnimation(false);
    //       }
    // }

}
