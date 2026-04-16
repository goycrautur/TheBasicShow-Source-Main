using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreenManager : MonoBehaviour
{
    private void Start()
    {
        hhhhhh.SetTrigger("open");
        DiscordRPC_stuff.current.UpdateStatus("Opening menu", "peak", "van", "the crx");
    }


    public Animator hhhhhh;
}
