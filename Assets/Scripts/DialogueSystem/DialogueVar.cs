using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class DialogueVar
{

    [TextArea]
    public string dialogue;

    public string characterName;
    public float speed;

    public string iconAnimation;

    public AudioClip characterSound;

    public UnityEvent eventToPlay;

    public Animator animToPlay;

    public string animationName;

    public bool speak = true;

    public bool canSkip = true;

    public float timeToFinish;

    public enum Character
    {
        none,
        hamboza,
        zarellan
    }

    public Character character;

    public enum Animationo
    {
        idle
    }

    public Animationo animation;




}

[System.Serializable]
public class BackIdle
{
    public Animator backIdleAnim;
    public string customBackAnim; // in case there is special event happen to the character

}