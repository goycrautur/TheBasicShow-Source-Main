using UnityEngine;
public class DoorEnteringTrigger : MonoBehaviour //SON,,,,,FOLK...... AAAAA
{
    #region CollisionHandlers
    private void OnTriggerStay(Collider OPEN)
    {
        if (!doorscri.bDoorLocked & OPEN.CompareTag("NPC") & !doorscri.Check || !doorscri.bDoorLocked & OPEN.CompareTag("cork") & !doorscri.Check || !doorscri.bDoorLocked & OPEN.CompareTag("Projectile") & !doorscri.Check)
        {
            doorscri.OpenDoor(3);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!(GameControllerScript.Instance.principal.angry || other.transform.name != "Principal of the Thing") || !(GameControllerScript.Instance.maxplayGames.angry || other.transform.name != "maxplay games if he was principal"))
        {
            doorscri.HandlePrincipalInteraction();
        }

        if (doorscri.bDoorLocked && other.transform.name == "Baldi" || doorscri.bDoorLocked && other.transform.name == "Jerry" || doorscri.bDoorLocked && other.transform.name == "Mucho" || doorscri.bDoorLocked && other.CompareTag("cork"))
        {
            doorscri.UnlockDoor();
            doorscri.OpenDoor(3);
        }
    }
    #endregion
    public DoorScript doorscri;
    public MeshCollider trigger;
}