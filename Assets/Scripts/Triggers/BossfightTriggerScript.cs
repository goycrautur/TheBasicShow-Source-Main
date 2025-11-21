using System.Collections;
using UnityEngine;

public class BossfightTriggerScript : MonoBehaviour
{
    private bool enter;

    [SerializeField]
    private Transform bossSpawn;

    [SerializeField]
    private NearElevatorTriggerScript nes;

    [SerializeField]
    private int exitID;

    [SerializeField]
    private NearElevatorTriggerScript[] miscElevators;

    private void Start()
    {
        string himode = PlayerPrefs.GetString("CurrentMode");
        if (himode != "zerullclassic" && himode != "famished"||himode != "famished" && himode != "zerullclassic")
        {
            Destroy(base.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameControllerScript.Instance.finaleMode && other.CompareTag("Player"))
        {
            foreach (NearElevatorTriggerScript otherElevators in miscElevators)
            {
                if (otherElevators.exitClosed)
                {
                    if (!enter && GameControllerScript.Instance.exitsReached >= GameControllerScript.Instance.maxExits-1 && !nes.exitClosed)
                    {
                        enter = true;
                        Encounter();
                        return;
                    }
                }
                if (nes.exitClosed)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    private void Encounter()
    {
        enter = true;
        nes.closeExitStuff();
        string himode = PlayerPrefs.GetString("CurrentMode");
        if (himode == "zerullclassic")
        {
        ZerullClassic.Instance.GetBoss().Target = bossSpawn;
        ZerullClassic.Instance.BossIntro();
        }
        if (himode == "famished")
        {
        FamishedModeController.Instance.onebounc(bossSpawn);
        }
        foreach (BossfightTriggerScript bossTriggers in FindObjectsOfType<BossfightTriggerScript>())
        {
            if (bossTriggers != this)
            {
                Destroy(bossTriggers.gameObject);
            }
        }
        gameObject.SetActive(false);
    }

    public bool Enter
    {
        get
        {
            return enter;
        }
    }
}
