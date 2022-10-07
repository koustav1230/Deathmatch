using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void Start()
    {
        player_conltroller = GetComponent<CharacterController>();
        Maincamera = GameObject.Find("Main Camera");
        anime = GetComponent<Animator>();
    }


    void Update()
    {
        get_Horizontal = Input.GetAxisRaw("Horizontal");
        get_Vertical = Input.GetAxisRaw("Vertical");
        Moving_direction = new Vector3(get_Horizontal, 0, get_Vertical).normalized;
        AnimationController();

    }

    private void AnimationController()
    {
        if (Moving_direction.magnitude >= 0.5f)
        {
            player_speed = 2;
            anime.SetBool("IsWalking", true);
            anime.SetBool("IsRunning", false);
            ThirdPersonMovement();
        }
        if (Moving_direction.magnitude >= 0.5f && Input.GetKey(KeyCode.LeftShift))
        {
            player_speed = 6;
            anime.SetBool("IsRunning", true);
            ThirdPersonMovement();
        }
        if (Moving_direction.magnitude == 0)
        {
            anime.SetBool("IsRunning", false);
            anime.SetBool("IsWalking", false);

        }
    }

    private void ThirdPersonMovement()
    {
        desired_Angle = Mathf.Atan2(Moving_direction.x, Moving_direction.z) * Mathf.Rad2Deg + Maincamera.transform.eulerAngles.y;
        smooth_Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, desired_Angle, ref getSoomthness, smooth_rotation);
        transform.rotation = Quaternion.Euler(0f, smooth_Angle, 0f);
        newDirection = Quaternion.Euler(0, desired_Angle, 0) * Vector3.forward;
        player_conltroller.Move(newDirection.normalized * Time.deltaTime * player_speed);

    }


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
}



