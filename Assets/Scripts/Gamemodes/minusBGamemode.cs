using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minusBGamemode : MonoBehaviour
{
    public void Start()
    {
        MinusBPosOg = minusB.transform;
        MinusBIntroDone = false;
    }
    public void Update()
    {
        if (minusbTime)
        {
            if (ColorVal.a < 0.1f) ColorVal = new Color(1f, 0f, 0f, ColorVal.a + (rainboSpee * Time.deltaTime));
            Color.RGBToHSV(ColorVal, out huehuehue, out saturati, out brignes);
            huehuehue += rainboSpee * Time.deltaTime;
            if (huehuehue > 1f) huehuehue = 0f;
            ColorVal = Color.HSVToRGB(huehuehue, saturati, brignes);
            ColorVal = new Color(ColorVal.r, ColorVal.g, ColorVal.b, transparenci);
            RenderSettings.ambientLight = ColorVal;
        }
        if (MinusBIntroDone)
        {
            foreach (minusbScript minu in gc.minusbee) 
            {
                minu.ForceNoSpeed = false;
                minu.Muted = false;
            }
        }
        else
        {
            foreach (minusbScript minu in gc.minusbee) 
            {
                minu.ForceNoSpeed = true;
                minu.Muted = true;
            }
        }
    }
    public void manualUpdate()
    {
        CheeseClonerCounter++;
        
        if (CheeseClonerCounter == 2 && minusB != null)
        {
            GameObject clone = Instantiate(minusB, MinusBPosOg.position, MinusBPosOg.rotation);
            clone.name = minusB.name;
            clone.SetActive(true);
            CheeseClonerCounter = 0;
        }
        if (gc.notebooks == 2)
        {
            Sych.SetGameWindowTitle("The Basic Show - Numberslops, the peak one (minus b)");
            gc.player.walkSpeedMultipler += 1.5f;
            gc.player.runSpeedMultipler += 1.5f;
            minusB.SetActive(true);
            minusbTime = true;
            gc.lbams.EscapeMusic.ClearQueue(true);  
            gc.lbams.EscapeMusic.QueueAudio(minusbang);
            gc.lbams.EscapeMusic.SetLoop(true);
            StartCoroutine(MinusBStartMoving(3.6f));
        }
        if (gc.notebooks == gc.maxNotebooks)
        {
        }
    }
    public IEnumerator MinusBStartMoving(float delay)
    {
        MinusBIntroDone = false;
        yield return new WaitForSeconds(delay);
        MinusBIntroDone = true;
        ZerullClassic.Instance.yourflashbang.Rebind();
        ZerullClassic.Instance.yourflashbang.Play("flashAnim", -1, 0f);
        yield return null;
    }
    public GameObject minusB,TutoMinusB;
    public Transform MinusBPosOg;
    public int CheeseClonerCounter;
    public AudioObjectyeah minusbang;
    public float rainboSpee, huehuehue, saturati, brignes, transparenci;
    public bool minusbTime,MinusBIntroDone;
    public GameControllerScript gc;
    public Color ColorVal;
}
