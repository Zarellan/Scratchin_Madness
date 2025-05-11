using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField]
    GameObject scoreText,coinText,timeText;

    int totalScore;

    int totalCoin;

    [SerializeField]
    float timer;

    bool revealTime = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (revealTime)
        {
            if (timer >= 0)
                timeText.GetComponent<TMP_Text>().text = "TIME\n" + Mathf.FloorToInt(timer);
            else
                timeText.GetComponent<TMP_Text>().text = "TIME\nI forgot";

        }
    }


    void RevealScore(GameObject position) // I give up I have no time for this
    {
        /*Vector2 anchPos;

        Camera cam = GameObject.FindWithTag("Camera").GetComponent<Camera>(); 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(recto,position.transform.position,cam,canv.renderMode == RenderMode.ScreenSpaceOverlay?null:cam,out anchPos);
        revObj = Instantiate(revealScoreObj,rect,Quaternion.identity,this.transform);
        revObj.GetComponent<TMP_Text>().text = 200.ToString();
        //revObj.GetComponent<RectTransform>().anchoredPosition = */
    }


    public void Scoring(int score)
    {
        totalScore += score;
        scoreText.GetComponent<TMP_Text>().text = "MARIO\n" + string.Format("{0:000000}",totalScore);
    }

    public void Scoring(string texto)
    {
        scoreText.GetComponent<TMP_Text>().text = texto;
    }

    public void AddCoin(int coin)
    {
        totalCoin += coin;

        coinText.GetComponent<TMP_Text>().text = "x"+ string.Format("{0:00}",totalCoin);
    }

    public void ChangeTimeText(string texto)
    {
        revealTime = false;
        timeText.GetComponent<TMP_Text>().text =  texto;
    }

    public void RevealTime()
    {
        revealTime = true;
    }

}
