using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectSwitchingThingIdkHowToSayIt : MonoBehaviour
{
    public void OnEnable()
    {
        switch (SwitchTo)
        {
            case SwitchToTypes.CharSelect:
                currentValue = PlayerPrefs.GetInt("CharInt");
                break;
            case SwitchToTypes.DifficulitySelect:
                currentValue = Difficulity.StartWithWhatDifficulity;
                difficulitystuff();
                break;
        }
    }
    public void pressedPreviousButton()
    {
        switch (SwitchTo)
        {
            case SwitchToTypes.none:
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
                return;
            case SwitchToTypes.CharSelect:
                CharSelectBullshitLMFAO(true);
                return;
            case SwitchToTypes.DifficulitySelect:
                DifficulityManagement(true);
                return;
        }
    }
    public void pressedNextButton()
    {
        switch (SwitchTo)
        {
            case SwitchToTypes.none:
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
                return;
            case SwitchToTypes.CharSelect:
                CharSelectBullshitLMFAO(false);
                return;
            case SwitchToTypes.DifficulitySelect:
                DifficulityManagement(false);
                return;
        }
    }
    private void DifficulityManagement(bool PressPrevious)
    {
        if (PressPrevious)
        {
            if (currentValue == 0)
            {
                currentValue = Difficulity.difficulityeeee.Count-1;
                difficulitystuff();
                return;
            }
            currentValue--;
            difficulitystuff();
            return;
        }
        if (currentValue == Difficulity.difficulityeeee.Count-1)
        {
            currentValue = 0;
            difficulitystuff();
            return;
        }
        currentValue++;
        difficulitystuff();
    }
    private void difficulitystuff()
    {
        newgamebutton.Difficulity = Difficulity.difficulityeeee[currentValue].DifficulitiesString;
        SpriteObjects.sprite = Difficulity.difficulityeeee[currentValue].DifficulitySprites;
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
    public SwitchToTypes SwitchTo;
    public enum SwitchToTypes {none,CharSelect,DifficulitySelect};
    public GameObject[] GameobjectFunnyThing;
    
    public int currentValue = 0;
    [Header("Character Select Thing")]
    public CharSelectManager csm;
    [Header("Difficulity Select Thing")]
    public ModesDifficulityList Difficulity;
    public Image SpriteObjects;
    public PlayButtonScri newgamebutton;
}
