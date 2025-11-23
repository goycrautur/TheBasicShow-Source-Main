using System.Collections;
using TMPro;
using UnityEngine;

public class MathMachineScript : MonoBehaviour
{
    public int num1,sign,num3,correct = -1;
    public AudioClip gen,pop,err;
    public AudioSource audioSource;
    public MeshRenderer meshRenderer;
    public Material right,wrong;
    public bool won, hasLost;
    public Transform player;
    public TextMeshPro text1,textsign,text3,text4;
    public GameObject prize, disabled;
    public SpriteRenderer machineMapIcon;
    public GameObject[] balls;
    public float cooldown,Say;
    public void Pickup(int num, GameObject gameObject)
    {
        try
        {
            disabled.GetComponent<NumBallScript>().IsPickup = false;
			disabled.transform.position = gameObject.transform.position;
        }
        catch { }
        GameControllerScript.Instance.PickBall = gameObject;
        disabled = gameObject;
        GameControllerScript.Instance.isHoldingBall = true;
        GameControllerScript.Instance.numOfBall = num;
    }

    public void GetNewProblem()
    {
        machineMapIcon.enabled = true;
        num1 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f));//Pick a random number between 0 and 9
        num3 = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f));//Pick a random number between 0 and 9
        sign = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f));// Picks a random sign (addition) or (subtraction)
        text1.text = num1.ToString();
        text3.text = num3.ToString();
        switch (sign)
        {
            case 0:
                textsign.text = "+"; correct = num1 + num3; break;
            default:
                textsign.text = "-"; correct = num1 - num3; break;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        Say = ((int)(UnityEngine.Random.Range(1f, 6f)));
        player = GameObject.Find("Player").transform;
        GetNewProblem();
        while (correct < 0 || correct > 9)
        {
            GetNewProblem();
        }
        machineMapIcon.enabled = true;
    }

    private void Update()
    {
        if (Vector3.Distance(base.transform.position, player.position) > GameControllerScript.Instance.player.LocalRange * 100f && GameControllerScript.Instance.isHoldingBall && disabled != null)
        {
            disabled.SetActive(true);
            disabled = null;
            GameControllerScript.Instance.isHoldingBall = false;
        }
        if (Input.GetMouseButtonDown(0) && GameControllerScript.Instance.isHoldingBall)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                if (raycastHit.transform.gameObject.Equals(this.transform.gameObject) & Vector3.Distance(player.position, base.transform.position) < GameControllerScript.Instance.player.LocalRange)
                {
                    if (GameControllerScript.Instance.numOfBall == correct)
                    {
                        StartCoroutine(Win());
                    }
                    else
                    {
                        StartCoroutine(Lose());
                    }
                }
            }
        }
        if (cooldown > 0)
        {
            text1.text = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f)).ToString();
            text3.text = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f)).ToString();
            if (Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f)) == 0) { textsign.text = "+"; }
            else { textsign.text = "-"; }
            cooldown -= Time.deltaTime;
        }
        else if (cooldown < 0 & !won)
        {
            text1.text = num1.ToString();
            text3.text = num3.ToString();
            switch (sign)
            {
                case 0:
                    textsign.text = "+"; correct = num1 + num3; break;
                default:
                    textsign.text = "-"; correct = num1 - num3; break;
            }
        }
        /*if (GameControllerScript.Instance.notebooks >= GameControllerScript.Instance.UnlockAmount-1 && !won)
        {
            text1.text = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f)).ToString();
            text3.text = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 9f)).ToString();
            if (Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f)) == 0) { textsign.text = "+"; }
            else { textsign.text = "-"; }

            text4.gameObject.SetActive(true);

            if (Mathf.RoundToInt(UnityEngine.Random.Range(0f, 1f)) == 0) { text4.text = "?"; }
            else { text4.text = "!"; }

            correct = (int)11.1;
        }*/
    }

    // Update is called once per frame
    public IEnumerator Win()
    {
        if (!won & !hasLost & cooldown <= 0)
        {
            machineMapIcon.enabled = false;
            audioSource.PlayOneShot(gen);
            GameControllerScript.Instance.SubsManager.summonLeSubtitle(GameControllerScript.Instance.subtitlesScriptableObject[8].subtitleOption, GameControllerScript.Instance.subtitlesScriptableObject[8], 0f, audioSource);
            GameControllerScript.Instance.isHoldingBall = false;
            disabled = null;
            won = true;
            if (GameControllerScript.Instance.LapManag.Meeptimar.isActiveAndEnabled)
            {
                meepTimerScript.Instance.AddTime(35f,Color.green);
            }
            if (!GameControllerScript.Instance.spoopMode & GameControllerScript.Instance.notebooks == 1) // If this is the players first notebook and they didn't get any questions wrong, reward them with a quarter
            {
                if (GameControllerScript.Instance.mode == "story")
                {
                    LearningGameManager.Instance.Tutor.tutorSource.Stop();
                    LearningGameManager.Instance.quarter.SetActive(true);
                    LearningGameManager.Instance.Tutor.tutorSource.PlayClip(LearningGameManager.Instance.aud_Prize, false, 1f);
                }
            }
            meshRenderer.material = right;
            text4.gameObject.SetActive(true);
            text4.text = correct.ToString();
            
            yield return new WaitForSeconds(0.1f);
            prize.SetActive(true);
            foreach (GameObject ball in balls)
            {
                GameControllerScript.Instance.PickBall.SetActive(false);
                ball.GetComponent<balloonFloatScript>().minDirectionTime = 0;
                ball.GetComponent<balloonFloatScript>().maxDirectionTime = 0;
                ball.GetComponent<CapsuleCollider>().enabled = false;
                ball.GetComponent<SpriteRenderer>().enabled = false;
                ball.GetComponent<NumBallScript>().die();
                ball.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
        }

    }

    public IEnumerator Lose()
    {
        if (!won & cooldown <= 0)
        {
            machineMapIcon.enabled = false;
            if (!GameControllerScript.Instance.spoopMode)
            {
                GameControllerScript.Instance.ActivateSpoopMode();
            }
            if (GameControllerScript.Instance.LapManag.Meeptimar.isActiveAndEnabled)
            {
                meepTimerScript.Instance.AddTime(-5f,Color.red);
            }
            else
            {
                if (!GameControllerScript.Instance.spoopMode & GameControllerScript.Instance.notebooks == 2)
                {
                    if (GameControllerScript.Instance.mode == "story")
                    {
                        LearningGameManager.Instance.Tutor.StartCoroutine(LearningGameManager.Instance.Tutor.captions());
                    }
                }
                Singleton<OtherMainStuffManager>.Instance.HearingShit(9f, this.transform, new Vector3(0f,0f,0f), "all",false);
                Singleton<OtherMainStuffManager>.Instance.AngerShit(3f, 0, false, "all");
            }
            audioSource.PlayOneShot(err);
            GameControllerScript.Instance.SubsManager.summonLeSubtitle(GameControllerScript.Instance.subtitlesScriptableObject[9].subtitleOption, GameControllerScript.Instance.subtitlesScriptableObject[9], 0f, audioSource);
            disabled = null;
            won = true;
            meshRenderer.material = wrong;
            text4.gameObject.SetActive(true);
            text4.text = correct.ToString();
            yield return new WaitForSeconds(0.1f);
            prize.SetActive(true);
            GameControllerScript.Instance.PickBall.SetActive(false);
            foreach (GameObject ball in balls)
            {
                GameControllerScript.Instance.isHoldingBall = false;
                ball.GetComponent<balloonFloatScript>().minDirectionTime = 0;
                ball.GetComponent<balloonFloatScript>().maxDirectionTime = 0;
                ball.GetComponent<SpriteRenderer>().enabled = false;
                ball.GetComponent<CapsuleCollider>().enabled = false;
                ball.GetComponent<NumBallScript>().die();
                ball.SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
