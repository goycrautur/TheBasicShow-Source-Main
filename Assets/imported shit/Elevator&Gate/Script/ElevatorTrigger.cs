using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    [SerializeField] private ElvDoorScript eDoor;
    [SerializeField] private EndingManager em;
    [SerializeField] private GameControllerScript gc;
    public int WinID;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && eDoor != null && eDoor.IsOpening())
        {
            if (gc.notebooks >= gc.maxNotebooks)
            {
                for (int i = 0; i < AdditionalGameCustomizer.Instance.ExitImages.Length; ++i)
                {
                    gc.ExitReached(i);
                }
                eDoor.Close();
                if (gc.failedNotebooks >= gc.maxNotebooks)
                {
                    em.endingShit(WinID,true);
                }
                else
                {
                    em.endingShit(WinID,false);
                }
            }
        }
    }
}