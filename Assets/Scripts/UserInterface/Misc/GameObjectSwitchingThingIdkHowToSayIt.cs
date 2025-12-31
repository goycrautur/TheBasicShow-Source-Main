using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSwitchingThingIdkHowToSayIt : MonoBehaviour
{
    public void OnEnable()
    {
        if (SwitchToCharSelect)
        {
            currentValue = PlayerPrefs.GetInt("CharInt");
        }
    }
    public void pressedPreviousButton()
    {
        if (SwitchToCharSelect)
        {
            CharSelectBullshitLMFAO(true);
            return;
        }
        if (currentValue == 0) 
        {
            for (int val = 0 ; val < GameobjectFunnyThing.Length; val++)
            {
                GameobjectFunnyThing[val].SetActive(false);
            }
            currentValue = GameobjectFunnyThing.Length-1;
            GameobjectFunnyThing[currentValue].SetActive(true);
            return;
        }
        GameobjectFunnyThing[currentValue-1].SetActive(true);
        GameobjectFunnyThing[currentValue].SetActive(false);
        currentValue--;
    }
    public void pressedNextButton()
    {
        if (SwitchToCharSelect)
        {
            CharSelectBullshitLMFAO(false);
            return;
        }
        if (currentValue == GameobjectFunnyThing.Length-1) 
        {
            for (int val = 0 ; val < GameobjectFunnyThing.Length; val++)
            {
                GameobjectFunnyThing[val].SetActive(false);
            }
            currentValue = 0;
            GameobjectFunnyThing[currentValue].SetActive(true);
            return;
        }
        GameobjectFunnyThing[currentValue].SetActive(false);
        GameobjectFunnyThing[currentValue+1].SetActive(true);
        currentValue++;
    }
    private void CharSelectBullshitLMFAO(bool PressPrevious)
    {
        if (PressPrevious)
        {
            if (currentValue == 0)
            {
                currentValue = csm.characterThing.Count-1;
                csm.changeCharStuff2(currentValue);
                return;
            }
            currentValue--;
            csm.changeCharStuff2(currentValue);
            return;
        }
        if (currentValue == csm.characterThing.Count-1)
        {
            currentValue = 0;
            csm.changeCharStuff2(currentValue);
            return;
        }
        currentValue++;
        csm.changeCharStuff2(currentValue);
    }
    public GameObject[] GameobjectFunnyThing;
    public int currentValue = 0;
    public bool SwitchToCharSelect;
    public CharSelectManager csm;
}
