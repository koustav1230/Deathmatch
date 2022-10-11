using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerMovement : MonoBehaviour
{
    private CharacterController player_conltroller;
    private float get_Horizontal;
    private float get_Vertical;
    private float desired_Angle;
    private float smooth_Angle;
    private float smooth_rotation = 0.3f;
    private float getSoomthness;
    private GameObject Maincamera;
    private Vector3 Moving_direction;
    [SerializeField] float player_speed = 6f;
    private Animator anime;
    Vector3 newDirection;



    [SerializeField]float Gravity = -0.1f;
    [SerializeField]float jumpheight = 0.1f;
    
    [SerializeField]float groundDistance;
    private GameObject GroundCheck;
    public LayerMask ground;
    private Vector3 velocity;
    private bool isgrounded;

    public GameObject VirtualCamera;
    public GameObject freecam;
    bool isfp;
    bool iscrouch,isrunning;

    public GameObject Gun;
    GameObject GetGun;
    bool IsGetgun;

    void Start()
    {
        iscrouch = false;
        isrunning = false;
        IsGetgun = false;
        player_conltroller = GetComponent<CharacterController>();
        Maincamera = GameObject.Find("Main Camera");
        GroundCheck = GameObject.Find("GroundChecker");
        anime = GetComponent<Animator>();
    }


    void Update()
    {
        Axis();
        AnimationController();
        Jump();
        Crouch();
        CameraChanger();
        slide();


        if (Input.GetKeyDown(KeyCode.V) && !IsGetgun)
        {
            anime.SetBool("Rifleidle", true);
            Vector3 gunposition = transform.position; 
            gunposition.x += -0.112f;
            gunposition.y += 0.88f;
            gunposition.z += 0.209f;
            GetGun = Instantiate(Gun, gunposition, Quaternion.Euler(10, -44, 0));
            GetGun.transform.SetParent(transform);
            IsGetgun = true;
        }

    }

    private void CameraChanger()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            isfp = !isfp;
        }
        if (!isfp)
        {
            freecam.SetActive(true);
            VirtualCamera.SetActive(false);
        }
        if (isfp)
        {
            freecam.SetActive(false);
            VirtualCamera.SetActive(true);
        }
    }

    #region GetAxis
    private void Axis()
    {
        get_Horizontal = Input.GetAxisRaw("Horizontal");
        get_Vertical = Input.GetAxisRaw("Vertical");
        Moving_direction = new Vector3(get_Horizontal, 0, get_Vertical).normalized;


    }
    #endregion

    #region Jump
    private void Jump()
    {
        isgrounded = Physics.CheckSphere(GroundCheck.transform.position, groundDistance, ground);
        if (isgrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isgrounded)
        {
            velocity.y = Mathf.Sqrt(jumpheight * -2f * Gravity);
            anime.SetTrigger("Jump");
        }
        velocity.y += Time.deltaTime * Gravity;

        player_conltroller.Move(velocity);
    }
    #endregion

    #region AnimationController
    private void AnimationController()
    {
        if (Moving_direction.magnitude >= 0.5f)
        {
            isrunning = false;
            player_speed = 2;
            if (!iscrouch)
            {
                anime.SetBool("IsWalking", true);
                anime.SetBool("IsRunning", false);
                anime.SetBool("Crouchwalk", false);
            }
            if(iscrouch)
            {
                anime.SetBool("Crouchwalk", true);
            }
            MovementCotroller();
        }
        if (Moving_direction.magnitude >= 0.5f && Input.GetKey(KeyCode.LeftShift) && !iscrouch)
        {
            isrunning = true;
            player_speed = 6;
            anime.SetBool("IsRunning", true);
            MovementCotroller();
        }
        if (Moving_direction.magnitude == 0)
        {
            anime.SetBool("IsRunning", false);
            anime.SetBool("IsWalking", false);
            anime.SetBool("Crouchwalk", false);
        }
    }
    #endregion

    #region ThirdPersoncontroller
    private void MovementCotroller()
    {
        desired_Angle = Mathf.Atan2(Moving_direction.x, Moving_direction.z) * Mathf.Rad2Deg + Maincamera.transform.eulerAngles.y;
        smooth_Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, desired_Angle, ref getSoomthness, smooth_rotation);
        transform.rotation = Quaternion.Euler(0f, smooth_Angle, 0f);
        newDirection = Quaternion.Euler(0, desired_Angle, 0) * Vector3.forward;
        player_conltroller.Move(newDirection.normalized * Time.deltaTime * player_speed);


    }
    #endregion


    public void slide()
    {
        if(Input.GetKeyDown(KeyCode.Q) && isrunning)
        {
            anime.SetTrigger("Slide");

        }
    }

    public void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            iscrouch = !iscrouch;
            anime.SetBool("Iscrouch", iscrouch);
        }
    }


    #region CursurLocker
    private void OnApplicationFocus(bool focus)
    {
        
        if(focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState= CursorLockMode.None;  
        }
    }
    #endregion
}



