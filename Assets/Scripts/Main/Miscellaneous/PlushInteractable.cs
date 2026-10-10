using UnityEngine;

public class PlushInteractable : Interactable
{
    public AudioManagerLiveReaction audio;
    public AudioObjectyeah sound;
    public int plush;
    public bool stop;
    public DoorScript d;
    public override void Interact()
    {
        audio.PlaySingleClip(sound);
        Singleton<OtherMainStuffManager>.Instance.HearingShit(9f, this.transform, new Vector3(0f,0f,0f), "all",false);
        if (plush < 100 && !stop)plush++;
        if (plush >= 100)
        {
            plush = 0;
            stop = true;
            d.CreateFakeDoorPrefab(d.transform.position);
            d.DestroyDoor();
        }
    }
}
