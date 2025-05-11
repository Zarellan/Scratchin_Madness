using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class MelodiiOverworld : MonoBehaviour
{
    //static functions
    public static bool slept = false;
    Vector2 vect;
    SpriteRenderer sprite;
    Animator anim;

    [SerializeField] float speed;

    string currentAnim;

    string direction;


    public bool canMove;

    public bool running;

    float runningSpeed = 1;

    string animProcessName = "Walk";


    public bool camFollow = false;


    [SerializeField]
    float minX,maxX,minY,maxY;


    float botX,botY;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        sprite = GetComponent<SpriteRenderer>();

        if (slept)
            anim.Play("IdleDown");
        
    }

    public void MelodiiJump() //for intro
    {
        AudioUtil.Play("jump");
        anim.Play("Jump");
        Tween.PositionY(transform,transform.position.y + 1,0.3f,Ease.OutCirc).OnComplete(()=>{

            Tween.PositionY(transform,transform.position.y - 1,0.3f,Ease.InSine).OnComplete(()=>{

                anim.Play("JumpDown");
            });


        });
    }
    
    public void WokeUp()
    {
        if (!slept)
        {
            //StartMoving();
            slept = true;
        }

        anim.Play("IdleDown");
    }

    Vector3 velocity = Vector3.zero;
    void CamFollow()
    {
        Vector3 target = new Vector3(Mathf.Clamp(transform.position.x,minX,maxX),Mathf.Clamp(transform.position.y,minY,maxY),-10);

        Vector3 mCam = GameObject.FindWithTag("MainCamera").transform.position;
    
        GameObject.FindWithTag("MainCamera").transform.position = Vector3.SmoothDamp(mCam,target,ref velocity,0.25f);

    }

    public void FallDown()
    {
        SoundtrackUtil.PauseMusic();
        StopMoving();
        transform.position = new Vector2(transform.position.x - 1.2f,transform.position.y - 0.8f);
        AudioUtil.Play("fall down");
        anim.Play("melodii fall");

        StartCoroutine(Timer.timer(2,()=>AudioUtil.Play("door bell")));

        StartCoroutine(Timer.timer(2.5f,()=>{
            AudioUtil.Play("jump");
            anim.Play("Jump");

            SoundtrackUtil.UnPauseMusic();
            Tween.PositionY(transform,transform.position.y + 1,0.3f,Ease.OutCirc).OnComplete(()=>{

            Tween.PositionY(transform,transform.position.y - 1,0.3f,Ease.InSine).OnComplete(()=>{

                anim.Play("JumpDown");

                StartCoroutine(Timer.timer(0.6f,()=>GameObject.FindWithTag("AfterFall").GetComponent<ActiveDialogue>().ActiveDialog()));
            });


        });

        }));
    }


    // Update is called once per frame
    void Update()
    {
        if (canMove || botX != 0 || botY != 0)
        AnimationWalk();
        

        print(canMove);

    }

    void FixedUpdate()
    {
        if (canMove)
        MoveFunction();

        if (camFollow)
            CamFollow();


        ForceMoveUpdate();
    }

    void MoveFunction()
    {
        vect = transform.position;

        if (Input.GetKey(KeyCode.X))
        {
            running = true;
            runningSpeed = 2;
        }
        else
        {
            running = false;
            runningSpeed = 1;
        }

        vect.x += Input.GetAxisRaw("Horizontal")*speed*Time.deltaTime * runningSpeed;
        vect.y += Input.GetAxisRaw("Vertical")*speed*Time.deltaTime * runningSpeed;

        

        transform.position = vect;  
    }


    public void ForceMove(string move)
    {

        vect = transform.position;

        
            if (move.IndexOf("left") != -1)
                botX = -1;
            else if (move.IndexOf("right") != -1)
                botX = 1;
            else if (move.IndexOf("right") == -1 && move.IndexOf("left") == -1)
                botX = 0;


            if (move.IndexOf("up") != -1)
                botY = 1;
            else if (move.IndexOf("down") != -1)
                botY = -1;
            else if (move.IndexOf("up") == -1 && move.IndexOf("down") == -1)
                botY = 0;

            if (move == "stop")
                {
                    botY = 0;
                    botX = 0;
                }

        vect.x += botX*speed*Time.deltaTime;
        vect.y += botY*speed*Time.deltaTime;

        transform.position = vect;  

    }

    void ForceMoveUpdate()
    {
        vect = transform.position;

        vect.x += botX*speed*Time.deltaTime;
        vect.y += botY*speed*Time.deltaTime;

        transform.position = vect;  

    }

    void AnimationWalk()
    {
        float getAxisX = Input.GetAxisRaw("Horizontal");
        float getAxisY = Input.GetAxisRaw("Vertical");

        if (!canMove)
        {
            getAxisX = 0;
            getAxisY = 0;
        }

        if (running && canMove)
        {
            animProcessName = "Run";
        }
        else
        {
            animProcessName = "Walk";
        }

        if (getAxisY > 0 || botY > 0)
        {
            anim.Play(animProcessName+"Up");
            direction = "up";
        }
        else if (getAxisY < 0 || botY < 0)
        {
            anim.Play(animProcessName+"Down");
            direction = "down";
        }
        else if (getAxisX > 0 || botX > 0)
        {
            anim.Play(animProcessName+"Right");
            direction = "right";
        }
        else if (getAxisX < 0 || botX < 0)
        {
            anim.Play(animProcessName+"Left");
            direction = "left";
        }
        else
            AnimationWalkidle();


        
    }

    public void AnimationWalkidle()
    {
        switch (direction)
        {
            case "up":anim.Play("IdleUp");break;
            case "down":anim.Play("IdleDown");break;
            case "left":anim.Play("IdleLeft");break;
            case "right":anim.Play("IdleRight");break;

        }
    }



    public void StopMoving()
    {
        canMove = false;
    }

    public void StartMoving()
    {
        canMove = true;
    }


    public void AnimationChange(string animationName)
    {

        if (currentAnim == animationName)
        return;

        currentAnim = animationName;

        anim.Play(animationName);

    }
}
