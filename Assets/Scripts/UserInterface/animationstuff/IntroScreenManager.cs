using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        hhhhhh.GetComponent<Animator>().SetTrigger("yooo");
        DiscordRPC_stuff.current.UpdateStatus("Opening menu", "peak", "van", "the crx");
    }

    // Update is called once per frame

    public GameObject hhhhhh;
}
