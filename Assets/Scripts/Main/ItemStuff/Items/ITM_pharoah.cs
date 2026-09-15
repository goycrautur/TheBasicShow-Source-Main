using UnityEngine;

public class ITM_pharoah : BaseItem
{
    public override bool OnUse() => false;
    public override void OnPickup() => pharoahAud.PlaySingleClip(eyeOfRa);
    public override void CustomSpecialFunction() => UndeadFunction();
    private void UndeadFunction()
    {
        player.Iframes = 2;
        player.health = 1;
        pharoahAud.PlaySingleClip(eyeOfRa); //Yeah thats all LMFAO, simple but peak and effective good item
    }
    private PlayerScript player => GameControllerScript.Instance.player;
    [SerializeField] private AudioObjectyeah eyeOfRa;
    [SerializeField] private AudioManagerLiveReaction pharoahAud;
}
