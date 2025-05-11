using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.TextCore.Text;
using PrimeTween;
public class Dialogue : MonoBehaviour
{

    public static Dialogue instance;

    [SerializeField]
    GameObject dialogueSys;

    [SerializeField]
    Text text;

    [SerializeField]
    TextMeshProUGUI text2;
    [SerializeField] TextMeshProUGUI characterText;


    [SerializeField]
    GameObject box;


    [SerializeField]
    DialogueVar[] dialogue;

    [SerializeField]
    GameObject icon;

    string dialogueStorage;

    float timeo = 0.05f;

    int index = 0 ;

    int dialogueCase = 0;


    string part;
    string[] remainingText;

    string[] command;

    int[] partsNum;


    string dialogueStorageClean;
    
    public bool dialogueEnded = false;

    bool dialoguing = true;
    
    public AudioSource sound;

    public bool canDialogue = false;

    bool isAddingRich = false;

    bool instantIt = false;

    bool skipped = false;

    bool waited = false;
    //characters
    public enum Hamboza
    {
        normal,
        angry,
        idk

    }
    //

    private bool canSkip = true;


    int maxLengthInc = 0; //when text is added the number will increased by length of the text (for event system purpose)

    bool ForceStopMove = false; //in case there is still dialogue's
    

    void Awake()
    {
        //DontDestroyOnLoad(this);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        //print(dialogue[0].dialogue);

        //StartCoroutine(Timer.timer(2,StartDialogue));
        //StartCoroutine(Timer.timer(2,StartDialogue));

        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Canvas>().worldCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        if (canDialogue)
        {
        if (Input.GetKeyDown(KeyCode.Z) && dialogueEnded && canSkip && dialogueCase != dialogue.Length)
        {
            NextDiag();
        }

        if (Input.GetKeyDown(KeyCode.X) && !dialogueEnded && canSkip)
        {
            //dialoguing = true;
            Dialoguing();
            SkipDialogue();
        }
        }


        //RichChecker();

        if (instantIt)
        {
            Instant();
        }
        
        //if (skipped)
        //    Diag();

    }


    public void SetDialogue(DialogueVar[] dialo)
    {
        dialogue = dialo;
    }



    public void StartDialogue()
    {


        if (!canDialogue)
        {
        if (dialogue[dialogueCase].speak)
        {
            dialogueSys.SetActive(true);
            canSkip = true;
        }

        dialogueEnded = false;

        ForceStopMove = false;

        dialogueCase = 0;
        text2.text = "";

        maxLengthInc = 0;

        characterText.text = dialogue[dialogueCase].characterName;

        timeo = dialogue[dialogueCase].speed;

        IconVoiceSys();
        
        CharacterAnimationPlay();

        IconToPlay();
        
        StartCoroutine(Timer.timer(timeo,Diag));

        EventSystem();


        dialogue[dialogueCase].eventToPlay.Invoke();

        if (!dialogue[dialogueCase].speak || !dialogue[dialogueCase].canSkip)
        {
            canSkip = false;
        }

        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().StopMoving();
        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().AnimationWalkidle();

        if (dialogue[dialogueCase].speak)
        {
            Tween.UIAnchoredPositionY(dialogueSys.GetComponent<RectTransform>(),0,0.3f,Ease.OutBack);
        }


        }

        canDialogue = true;

    }




    void Diag()
    {



        if (text2.text.Length < dialogueStorageClean.Length + maxLengthInc)
        {
            if (dialoguing)
            {
                text2.text += dialogueStorageClean[index].ToString();
                index += 1;
                StartCoroutine(Timer.timer(timeo,Diag));
                if (!skipped && dialogue[dialogueCase].characterSound != null)
                sound.Play();
                
            }
        }
        else
        {
            dialogueEnded = true;
            if (!dialogue[dialogueCase].canSkip)
                StartCoroutine(Timer.timer(dialogue[dialogueCase].timeToFinish,TimerFinishForUnskip));
        }
        


        EventChecker();
        

    }


    void PrintCommand()
    {
        
        
    }


    void NextDiag()
    {

        dialogueCase += 1;

        if (dialogueCase == dialogue.Length)
        {
            DialogueFinished();
            return;
        }

        if (dialogue[dialogueCase].speak)
        {
        if (canDialogue)
        {
            maxLengthInc = 0;

            text2.text = "";
            index = 0;
            dialogueEnded = false;
            timeo = dialogue[dialogueCase].speed;
            characterText.text = dialogue[dialogueCase].characterName;
            

            skipped = false;

            IconVoiceSys();

            IconToPlay();

            CharacterAnimationPlay();

            EventSystem();

            Diag();

            //dialogueSys.GetComponent<RectTransform>().position = new Vector2(0,800);
            Tween.UIAnchoredPositionY(dialogueSys.GetComponent<RectTransform>(),0,0.3f,Ease.OutBack);
            
            

            canSkip = true;
        }
        }
        else
        {
            //dialogueSys.SetActive(false);
            Tween.UIAnchoredPositionY(dialogueSys.GetComponent<RectTransform>(),-400,0.3f,Ease.InBack);
            CharacterAnimationPlay();
            StartCoroutine(Timer.timer(dialogue[dialogueCase].timeToFinish,TimerFinishForUnskip));
            
        }
            if (dialogueCase != 0)
                dialogue[dialogueCase].eventToPlay.Invoke();

        if (!dialogue[dialogueCase].speak || !dialogue[dialogueCase].canSkip)
        {
            canSkip = false;
        }

    }


    void IconToPlay()
    {
        if (dialogue[dialogueCase].iconAnimation != "")
        {
            icon.GetComponent<Animator>().Play(dialogue[dialogueCase].iconAnimation);
            icon.GetComponent<Image>().enabled = true;
        }
        else
            icon.GetComponent<Image>().enabled = false;
    }

    public void IconFinish()
    {
        if (dialogueEnded && dialogueCase < dialogue.Length)
            icon.GetComponent<Animator>().Play(dialogue[dialogueCase].iconAnimation+" End");
    }

    void DialogueFinished()
    {
        StartCoroutine(Timer.timer(0.25f,()=>{
            canDialogue = false;
            canSkip = true;
            skipped = false;
            index = 0;

            dialogueCase = 0;
            })); //cooldown

        Tween.UIAnchoredPositionY(dialogueSys.GetComponent<RectTransform>(),-400,0.3f,Ease.InBack);

        if (!ForceStopMove)
            GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().StartMoving();


    }

    void changeSpeedText(float number)
    {
        if (!skipped)
        timeo = number;
    }

    public void ForceStopMoving()
    {
        ForceStopMove = true;
    }

    void ExecuteEvent(string command)
    {
         // initializing

        try
        {
        string[] comm = command.IndexOf(',') != 1?command.Split(','):null; 

        string coma = comm.Length > 0?comm[0]:command;

        string value = comm.Length > 1?comm[1]:null; // in case parameter hasn't been added

        string value2 = comm.Length > 2?comm[2]:null; // for extra purpose

        string value3 = comm.Length > 3?comm[3]:null; // for another extra purpose

        switch(coma)
        {
            case "speed":changeSpeedText(float.Parse(value));break; // changing the text speed value
            case "print":print(value);break; // printing (for test purpose)
            case "finish it":DialogueFinished();break; // finishing the dialogue
            case "skip":SkipDialogue();break; // skipping dialogue
            case "wait":waitTime(float.Parse(value));break;// dialogue text wait's for a specific time
            //case "voice":waitTime(value);break;// change voice in specific dialogue (not made yet)
            case "camPosX":CamPosX(float.Parse(value));break;// move camera with X position
            case "camPosY":CamPosY(float.Parse(value));break; // move camera with Y position
            case "scene change":SceneChange(value,float.Parse(value2),float.Parse(value3));break;
            case "Instant":Instant();break;
            case "UnInstant":UnInstant();break;
            case "color":ColorChange(value);break;
            case "changeChar":CharacterChange(value,value2);break;
            case "pos character":characterPos(value,float.Parse(value2),float.Parse(value3));break;
            case "set active":SetActiveObject(value,bool.Parse(value2));break;
            case "force next":ForceNext();break;
            case "change text":ChangeEntireText(value);break; // Warning: must user when dialogue ends


        }
        }
        catch{}
        //print(coma);


    }


    void EventSystem() // event system to make the task easier instead of adding every statement in the script
    {
        dialogueStorage = dialogue[dialogueCase].dialogue; // storaging dialogue on every index

        command = new string[10];

        partsNum = new int[10];

        remainingText = new string[10];



        //string[] parts = dialogueStorage.Split('['); // splitting every [ symbol


        /*for (int i = 0;i < parts.Length - 1;i++)
        {

            partsNum[i] = dialogueStorage.IndexOf('['); // getting dialogue storage index

            this.command[i] = parts[i + 1].Substring(0, parts[i + 1].IndexOf(']')); // getting every command before ] symbol and after [ symbol
            

            if (i == 0)
            this.remainingText[0] = dialogueStorage; // initializing
            
            string partse = "";
            for (int j = 0;j < i;j++) // getting every remained text after removing previous event symbol
            {
                partse += parts[j].Substring(parts[j].IndexOf(']') + 1);
                this.remainingText[0] = partse + remainingText[0].Substring(remainingText[0].IndexOf(']') + 1);
                
            }
            

            partsNum[i] = remainingText[0].IndexOf('['); // getting every index text length


        }

        dialogueStorageClean = ""; // resetting the dialogue storage
        for (int i = 0; i < parts.Length ; i++)
            dialogueStorageClean += parts[i].Substring(parts[i].IndexOf(']') + 1); // getting every text that doesn't contains event symbols

        for (int i = 0;i < parts.Length - 1;i++)
        {
            //print($"{i}-> part number: {partsNum[i]}\ncommand: {command[i]}");
        }*/
        RemoveRectangleString(dialogueStorage);

    }

    void SkipDialogue()// skipping dialogue (in case player don't care about the story)
    {
        //text2.text = dialogueStorageClean; removed due to event system functionality
        dialoguing = true;
        skipped = true;
        timeo = 0.0000000000000000001f;
        Diag();
        //dialogueEnded = true;
    }

    public void ChangeEntireText(string texto) //for trolling LOL
    {
        text2.text = texto;
    }

    void waitTime(float waitTime)
    {
        if (!waited && !skipped)
        {
            UnDialoguing();
            StartCoroutine(Timer.timer(waitTime,Dialoguing));
            waited = true;
        }

    }

    void UnDialoguing()
    {
        dialoguing = false;
    }

    void Dialoguing()
    {
        dialoguing = true;
        if (waited)
        {
        Diag();
        waited = false;
        }
    }


    void ColorChange(string color) // less performance but more efficient (more efficient in this case since having a event system)
    {
        string col = "";

        col = "<color="+color+">";
        maxLengthInc += col.Length;

        text2.text += col;
    }

    void ChangeAudio()
    {
        
    }

    public void PlayAudio(string name)
    {
        AudioUtil.Play(name);
    }

    public void PlayMusic(string name)
    {
        SoundtrackUtil.PlayMusic(name);
    }


    public void PauseMusic()
    {
        SoundtrackUtil.PauseMusic();
    }

    public void UnPauseMusic()
    {
        SoundtrackUtil.UnPauseMusic();
    }

    void CharacterChange(string value1,string value2)
    {
        
        CharacterAnimationPlayAdvanced(value1,value2);

    }

    void CharacterAnimationPlay()
    {
        DialogueVar diago =  dialogue[dialogueCase];
        try
        {
            diago.animToPlay.Play(diago.animationName);
        }
        catch // in case the animation didn't exist the system have to warn me
        {
            print("no animation specified");
        }


    }


    void characterPos(string name,float x,float y)
    {
        switch(name)
        {
            case "hamboza":GameObject.FindWithTag("Player").transform.position = new Vector3(x,y);break;
            case "zarellan":GameObject.FindWithTag("Player2").transform.position = new Vector3(x,y);break;
        }

    }

    public void CharacterPosEvent(string text)
    {
        string[] texts = text.Split(',');

        switch(texts[0])
        {
            case "hamboza":GameObject.FindWithTag("Player").transform.position = new Vector3(float.Parse(texts[1]),float.Parse(texts[2]));break;
            case "zarellan":GameObject.FindWithTag("Player2").transform.position = new Vector3(float.Parse(texts[1]),float.Parse(texts[2]));break;
        }

    }


    void SetActiveObject(string name,bool value)
    {
        
        GameObject.FindWithTag(name).SetActive(value);
        

    }

    public void SetActiveObjectEvent(string text)
    {
        string[] texts = text.Split(',');

        GameObject.FindWithTag(texts[0]).SetActive(bool.Parse(texts[1]));
    }

    public void ObjectAppereance(string text)
    {
        string[] texts = text.Split(',');

        Color col = texts[1] == "invisible"?new Color(255,255,255,0):new Color(255,255,255,1);

        GameObject.FindWithTag(texts[0]).GetComponent<SpriteRenderer>().color = col;
    }

    public void ForceNext()
    {
        //SkipDialogue();
        NextDiag();

    }


    void CharacterAnimationPlayAdvanced(string character,string anim)
    {
        GameObject.FindWithTag(character).GetComponent<Animator>().Play(anim);
    }


    void CamPosX(float x)
    {
        GameObject cam = GameObject.FindWithTag("MainCamera");
        cam.transform.position = new Vector2(x,cam.transform.position.y);
    }

    void CamPosY(float y)
    {
        GameObject cam = GameObject.FindWithTag("MainCamera");
        cam.transform.position = new Vector2(cam.transform.position.x,y);
    }

    public void CamRotation(float rotation)
    {
        GameObject cam = GameObject.FindWithTag("MainCamera");
        cam.transform.eulerAngles = new Vector3(0,0,rotation);
    }

    void CamZoom(float zoom)
    {
        GameObject cam = GameObject.FindWithTag("MainCamera");
        cam.GetComponent<Camera>().orthographicSize = zoom;
    }

    public void TweenCamToPC()
    {
        GameObject cam = GameObject.FindWithTag("MainCamera");
        Tween.Position(cam.transform,new Vector3(-3.1f,2.25f,-10),2.14f,Ease.Linear);
        Tween.CameraOrthographicSize(cam.GetComponent<Camera>(),2.67f,2.14f,Ease.Linear);
    }


    void SceneChange(string sceneName)
    {
        float duration = 0.5f;
        GameObject.FindWithTag("FlashLight").GetComponent<SpriteRenderer>().color = Color.black;
        Color ab = GameObject.FindWithTag("FlashLight").GetComponent<SpriteRenderer>().color;
        Tween.Alpha(GameObject.FindWithTag("FlashLight").GetComponent<SpriteRenderer>(),endValue:0,duration:00000.1f,Ease.Linear);
        Tween.Alpha(GameObject.FindWithTag("FlashLight").GetComponent<SpriteRenderer>(),endValue:1f,duration:duration,Ease.Linear);
        StartCoroutine(Timer.timer(duration,StartChangeScene,sceneName));
    }

    void SceneChange(string sceneName,float x,float y)
    {
        float duration = 0.5f;
        GameObject.FindWithTag("FlashLight").GetComponent<SpriteRenderer>().color = Color.black;
        Color ab = GameObject.FindWithTag("FlashLight").GetComponent<SpriteRenderer>().color;
        Tween.Alpha(GameObject.FindWithTag("FlashLight").GetComponent<SpriteRenderer>(),endValue:0,duration:00000.1f,Ease.Linear);
        Tween.Alpha(GameObject.FindWithTag("FlashLight").GetComponent<SpriteRenderer>(),endValue:1f,duration:duration,Ease.Linear);
        StartCoroutine(Timer.timer(duration,StartChangeScene,sceneName));
    }

    void TimerFinishForUnskip()
    {
        dialogueSys.SetActive(true);

        NextDiag();
    }




    void StartChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        //Tween.Alpha(GameObject.FindWithTag("FlashLight").GetComponent<SpriteRenderer>(),endValue:0,duration:0.5f,Ease.Linear);
    }

    public int GetCaseDialogue()
    {
        return dialogueCase;
    }

    void Instant()
    {
        instantIt = true;
        if (instantIt)
        {
        //text2.text += dialogueStorageClean[index].ToString();
        //index += 1;
        Diag();
        }


        EventChecker();
    }

    void UnInstant()
    {
        instantIt = false;
    }

    public void FinishIconAnimation()
    {

        //float length = iconPlay.GetCurrentAnimatorStateInfo(0).length;
        //float normalize = iconPlay.GetCurrentAnimatorStateInfo(0).normalizedTime;
        



    }



    public void IconVoiceSys()
    {
        if (dialogue[dialogueCase].characterSound != null)
            sound.clip = dialogue[dialogueCase].characterSound;


    }

    public void RichChecker() // failed
    {
        
        if (dialogueStorageClean[index] == '<' && dialogueStorageClean[index] != '>' || isAddingRich)
        {
            isAddingRich = true;
            text2.text += dialogueStorageClean[index].ToString();
            index += 1;
            
            


            if (dialogueStorageClean[index] == '>')
            {

                text2.text += dialogueStorageClean[index].ToString();
                index += 1;
                
                isAddingRich = false;
            }
        }

    }

    public void MusStopo()
    {
        Tween.AudioVolume(SoundtrackUtil.GetAudio(),0,1);
        Tween.AudioPitch(SoundtrackUtil.GetAudio(),0.6f,1);
    }

    void EventChecker()
    {
        for (int i = 0; i < partsNum.Length;i++) // looping on every parts number length that has been added in the array
        {
            if (text2.text.Length - maxLengthInc == partsNum[i] && dialogue[dialogueCase].dialogue != "" ) // activate when the text is on the specific requested length
                ExecuteEvent(command[i]); // execute the event
        }

    }

    public void CamChange(string text)
    {
        string[] texts = text.Split(',');
        Camera cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        cam.transform.position = new Vector3(float.Parse(texts[0]),float.Parse(texts[1]),-10);
        cam.orthographicSize = float.Parse(texts[2]);

    }



    void RemoveRectangleString(string text)// probably solved (will be back)
    {
        //string hello = "hii[dasdas] how[sasa] susssy dasdasas [dsa] lol [hamboooza] hehhe I lk";

        string[] helloSplit = text.Split('[');

        string[] helloSplit2 = text.Split(']');

        //int[] partsNum;
        //partsNum = new int[30];

        //string[] commands;
        //commands = new string[30];

        string remainingText = "";

        string remainingText2 = text;
        
        for (int i = 0;i < helloSplit.Length-1;i++)
        {
            
            this.partsNum[i] = remainingText2.IndexOf('[');
            remainingText += CheckIfExist(helloSplit2[i+1],'[');
            this.command[i] = helloSplit[i+1].Substring(0,helloSplit[i+1].IndexOf(']'));

            remainingText2 = remainingText2.Substring(0,remainingText2.IndexOf('[')) + remainingText2.Substring(remainingText2.IndexOf(']') + 1);

        }
        //this.partsNum = partsNum;
        //this.command = command;
        this.dialogueStorageClean = remainingText2;
        

    }



    public string CheckIfExist(string thisArray,char condit)
    {
        if (thisArray.IndexOf(condit) != -1)
        {
            return thisArray.Substring(0,thisArray.IndexOf(condit));
        }
        else
            return thisArray.Substring(0);
    }


    public void PrintTest()
    {
        print("omak1");
    }


    
}


