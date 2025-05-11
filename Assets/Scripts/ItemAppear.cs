using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class ItemAppear : MonoBehaviour
{
    public static ItemAppear instance;
    [SerializeField]
    GameObject black,item;
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
        
    }

    public void AppearItem(Sprite image)
    {
        item.GetComponent<Image>().sprite = image;
        Tween.UIAnchoredPositionY(item.GetComponent<RectTransform>(),0,0.2f,Ease.OutBack);
        Tween.Alpha(black.GetComponent<Image>(),0.2f,0.2f);
    }

    public void AppearItemBottom(Sprite image)
    {
        item.GetComponent<Image>().sprite = image;
        Tween.UIAnchoredPositionY(item.GetComponent<RectTransform>(),-1080,0.000001f,Ease.OutBack);
        Tween.UIAnchoredPositionY(item.GetComponent<RectTransform>(),0,0.2f,Ease.OutBack);
        Tween.Alpha(black.GetComponent<Image>(),0.2f,0.2f);
    }

    public void ChangeItem(Sprite image)
    {
        item.GetComponent<Image>().sprite = image;
    }

    public void ItemDissapear()
    {
        Tween.UIAnchoredPositionY(item.GetComponent<RectTransform>(),1080,0.2f,Ease.InBack);
        Tween.Alpha(black.GetComponent<Image>(),0,0.2f);
    }
}
