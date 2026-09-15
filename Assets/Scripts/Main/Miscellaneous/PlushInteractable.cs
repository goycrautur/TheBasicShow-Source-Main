using UnityEngine;

public class PlushInteractable : Interactable
{
    public AudioManagerLiveReaction audio;
    public AudioObjectyeah sound;
    public override void Interact()
    {
        audio.PlaySingleClip(sound);
        Singleton<OtherMainStuffManager>.Instance.HearingShit(9f, this.transform, new Vector3(0f,0f,0f), "all",false);
    }
}
