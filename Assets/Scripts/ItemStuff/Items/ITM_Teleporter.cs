public class ITM_Teleporter : BaseItem
{
    public override bool OnUse()
    {
        GameControllerScript.Instance.StartCoroutine(GameControllerScript.Instance.TeleporterFunction(tptypes == TeleporterTypes.TeleportationTeleporter ? "normal" : tptypes == TeleporterTypes.evilLeafy ? "evilleaf" : ""));
        return true;
    }
    public TeleporterTypes tptypes;
    public enum TeleporterTypes
    {
        TeleportationTeleporter,
        evilLeafy
    }
}
