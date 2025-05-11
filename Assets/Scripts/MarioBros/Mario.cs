using System;
using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using Unity.Mathematics;
using UnityEngine;

public class Mario : MonoBehaviour
{
    Vector2 vect;


    Rigidbody2D rigid;

    string currentAnim;

    float accelration;

    float jumpAccel;

    Animator anim;

    bool jumped = false;


    [SerializeField]
    float speed;


    bool isGrowing = false;

    bool isBig = false;

    string animBig = "";


    float posMinX=0;

    float deathTime = 0;

    float deathPosY = 2;


    bool isDead = false;

    bool crouch = false;


    bool canGetDamaged = true;
    

    bool staticed = false;

    public bool diedByGoomba = false;

    bool hitlered = false;

    bool hitlered2 = false;


    float idleDuration = 0;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        SoundtrackUtil.PlayMusic("marioOst");

        //StartCoroutine(Timer.timer(2,GrowUp));
    }

    // Update is called once per frame
    void Update()
    {
        Jump();

        if (rigid.velocity.y == 0)
        ControlHoriz();


        Animation();

        posMinX = GameObject.FindWithTag("MainCamera").transform.position.x;

        if (Input.GetKey(KeyCode.DownArrow) && isBig)
            crouch = true;
        else
            crouch = false;

        GameObject.FindWithTag("MainCamera").transform.position = new Vector3(Mathf.Clamp(transform.position.x,posMinX,9999),0.66f,-10);
        

        if (isDead)
            {
                deathTime += Time.fixedDeltaTime;
                anim.Play("Dead");

                if (deathTime > 17 && !staticed)
                {
                    AudioUtil.Stop();
                    if (!diedByGoomba)
                        GameObject.FindWithTag("StaticEffect").GetComponent<StaticEffect>().DoStatic();
                    else
                        GameObject.FindWithTag("StaticEffect").GetComponent<StaticEffect>().DoStaticGoomba();

                    staticed = true;

                }
                else if (deathTime > 10 && !staticed)
                {
                    deathPosY -= Time.fixedDeltaTime/2;
                    transform.position = new Vector2(transform.position.x,transform.position.y + deathPosY * Time.fixedDeltaTime);
                }
                
            }

        if (!hitlered2)
            HitlerJoke();

    }


    void HitlerJoke()
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle" && !isBig)
            idleDuration += Time.deltaTime;
        else
            idleDuration = 0;

        if (hitlered && anim.GetCurrentAnimatorClipInfo(0)[0].clip.name != "MarioHitler" && !hitlered2)
        {
            SoundtrackUtil.PlayMusic("marioOst");
            idleDuration = 0;
            hitlered2 = true;
        }


        if (idleDuration > 20 && !hitlered)
        {
            anim.Play("MarioHitler");
            hitlered = true;
            SoundtrackUtil.PlayMusic("nazi anthem");
        }
            


    }

    void FixedUpdate()
    {
        if (!isGrowing)
            Movements();
    }


    void Movements()
    {
        float noMove = crouch?0:1;

        rigid.velocity = new Vector2(Mathf.Clamp(rigid.velocity.x,-accelration,accelration)+ (Input.GetAxis("Horizontal")*noMove)*speed ,rigid.velocity.y);
    }

    

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !jumped && Time.timeScale != 0)
        {
            rigid.velocity = new Vector2(rigid.velocity.x,20*jumpAccel);
            jumped = true;
            AudioUtil.Play("marioJump",0.5f);
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow) && rigid.velocity.y > 0.0001)
            rigid.gravityScale = 10;

        if (rigid.velocity.y == 0)
        {
            //jumped = false;
            //rigid.gravityScale = 5;
        }
    }


    void ControlHoriz()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            accelration = Mathf.Lerp(accelration,10,0.2f);
            jumpAccel = Mathf.Lerp(jumpAccel,1.1f,0.2f);
        }
        else
        {
            accelration = Mathf.Lerp(accelration,5,0.2f);
            jumpAccel = Mathf.Lerp(jumpAccel,1f,0.2f);
        }
    }

    public void GrowUp()
    {
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        isGrowing = true;
        Time.timeScale = 0;
        AudioUtil.Play("mushroom",0.6f);
        anim.Play("Grow");
    }

    void Animation()
    {
        if (isBig)
            animBig = "Big";
        else
            animBig = "";


        if (!isGrowing && !isDead)
        {
        if (jumped)
        {
            AnimationChange("Jump"+animBig);
        }
        else  if (crouch && isBig)
        {
            AnimationChange("Crouch"+animBig);
        }
        else if (rigid.velocity.x != 0 && !jumped)
        {
            if (rigid.velocity.x > 0.4f && Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                if (currentAnim != "Slide"+animBig)
                    AudioUtil.Play("slide",0.8f);
                    
                AnimationChange("Slide"+animBig);
                GetComponent<SpriteRenderer>().flipX = true;

                print(rigid.velocity.x);
            }
            else if (rigid.velocity.x < -0.4f && Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
            {
                if (currentAnim != "Slide"+animBig)
                    AudioUtil.Play("slide",0.8f);

                AnimationChange("Slide"+animBig);
                GetComponent<SpriteRenderer>().flipX = false;

                print(rigid.velocity.x);
            }
            else
            {

            if (rigid.velocity.x > 0)
                GetComponent<SpriteRenderer>().flipX = false;
            else if (rigid.velocity.x < 0)
                GetComponent<SpriteRenderer>().flipX = true;

                AnimationChange("Walk"+animBig);

            }

        }
        else
            AnimationChange("Idle"+animBig);

        }

    }

    public void AnimationChange(string animationName)
    {

        if (currentAnim == animationName)
        return;

        currentAnim = animationName;

        anim.Play(animationName);

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (AllowedFloorDetect("Blocks",col)|| AllowedFloorDetect("Brick",col))
            {
                jumped = false;
                rigid.gravityScale = 5;
            }

    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (AllowedFloorDetect("Blocks",col))
            {
                jumped = false;
                rigid.gravityScale = 5;
            }

        // for all the bricks
        GameObject[] games = GameObject.FindGameObjectsWithTag("Brick");

        for (int i = 0;i < games.Length;i++)
            {
                if (games[i].name == col.name)
                    {
                        jumped = false;
                        rigid.gravityScale = 5;
                    }
            }

    }

    bool AllowedFloorDetect(string tagName,Collider2D col)
    {
        return GameObject.FindWithTag(tagName).name == col.name;
    }



    public bool IsBig()
    {
        return isBig;
    }

    void FinishedGrowth(int isBig)// bool is not supported in animation event
    {
        Time.timeScale = 1;
        anim.updateMode = AnimatorUpdateMode.Normal;

        isGrowing = false;

        this.isBig = isBig==1?true:false;
    }


    public void GrowDown()
    {
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        isGrowing = true; // it's actually not but whatever
        Time.timeScale = 0;
        canGetDamaged = false;
        AudioUtil.Play("loseMushroom",0.8f);
        anim.Play("GrowDown");

        StartCoroutine(Timer.timer(3,()=>canGetDamaged = true));
    }

    public void Damage()
    {
        if (canGetDamaged == true)
        {
        if (isBig)
            GrowDown();
        else
            MarioDead();

            print("damaged");
        }

    }


    public void MarioDead()
    {
        anim.updateMode = AnimatorUpdateMode.UnscaledTime;
        isDead = true;
        Time.timeScale = 0;
        AudioUtil.Play("marioLose");
        SoundtrackUtil.PauseMusic();
        anim.Play("Dead");

    }

    public void Revive()
    {
        deathTime = 0;
        isDead = false;
    }
    
    public void DeadByGoomba()
    {
        diedByGoomba = true;
    }

}
