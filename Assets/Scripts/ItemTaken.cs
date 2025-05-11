using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
public class ItemTaken : MonoBehaviour
{

    public static ItemTaken instance;

    [SerializeField]
    GameObject youGotItem,item,text,splash,info;

    bool openedItem = false;
    bool ableToClose = false;

    
    [SerializeField] UnityEvent eventWhenFinish;


    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.O))
            {
                ItemToken();
            }

        if (!ableToClose && openedItem)
            GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().StopMoving();

        if (ableToClose && Input.GetKeyDown(KeyCode.Z))
        {

            if (SceneManager.GetActiveScene().name == "Kitchen")
                GoBack2();
            else
                GoBack();



        }
    }


    public void ItemToken()
    {
        float duration = 0.25f;
        splash.GetComponent<RectTransform>().localScale = new Vector2(0.8f,0.8f);
        Tween.Scale(splash.GetComponent<RectTransform>(),new Vector2(1,1),duration,Ease.OutBack);

        splash.GetComponent<RectTransform>().localScale = new Vector2(0.8f,0.8f);
        Tween.Scale(youGotItem.GetComponent<RectTransform>(),new Vector2(1.2f,1.2f),duration,Ease.OutBack);

        Tween.Alpha(splash.GetComponent<Image>(),1,duration);
        Tween.Alpha(item.GetComponent<Image>(),1,duration);
        Tween.Alpha(youGotItem.GetComponent<Image>(),1,duration);

        Tween.UIAnchoredPositionY(text.GetComponent<RectTransform>(),-386,duration,Ease.OutBack);

        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().StopMoving();

        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().AnimationChange("itemTaken");

        SoundtrackUtil.PauseMusic();

        AudioUtil.Play("itemget");

        openedItem = true;

        StartCoroutine(Timer.timer(0.5f,()=>{
            Tween.UIAnchoredPositionX(info.GetComponent<RectTransform>(),848,duration,Ease.OutBack);

            ableToClose = true;
        }));
    }   


    public void GoBack()
    {
        float duration = 0.25f;

        Tween.Alpha(splash.GetComponent<Image>(),0,duration);
        Tween.Alpha(item.GetComponent<Image>(),0,duration);
        Tween.Alpha(youGotItem.GetComponent<Image>(),0,duration);

        Tween.UIAnchoredPositionY(text.GetComponent<RectTransform>(),-705,duration,Ease.InBack);


        Tween.UIAnchoredPositionX(info.GetComponent<RectTransform>(),1058,duration,Ease.InBack);


        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().StartMoving();
        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().AnimationChange("IdleDown");

        SoundtrackUtil.UnPauseMusic();
        
        openedItem = false;

        eventWhenFinish.Invoke();
    }

    public void GoBack2()
    {
        float duration = 0.25f;

        Tween.Alpha(splash.GetComponent<Image>(),0,duration);
        Tween.Alpha(item.GetComponent<Image>(),0,duration);
        Tween.Alpha(youGotItem.GetComponent<Image>(),0,duration);

        Tween.UIAnchoredPositionY(text.GetComponent<RectTransform>(),-705,duration,Ease.InBack);


        Tween.UIAnchoredPositionX(info.GetComponent<RectTransform>(),1058,duration,Ease.InBack);


        GameObject.FindWithTag("Player").GetComponent<MelodiiOverworld>().AnimationChange("IdleUp");

        SoundtrackUtil.UnPauseMusic();
        openedItem = false;

        eventWhenFinish.Invoke();

        ableToClose = false;
    }
}
