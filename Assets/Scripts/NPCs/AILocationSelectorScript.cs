using UnityEngine;
using UnityEngine.AI;

public class AILocationSelectorScript : MonoBehaviour
{
    private void Awake() => Instance = this;
    public static AILocationSelectorScript Instance;
    #region Setting Locations
    public Vector3 SetNewTargetForAgent(NavMeshAgent agent, string locationType = "default")
    {
        ambience.PlayAudio();

        Transform[] targetLocations = locationType switch
        {
            "hall" => hallLocation,
            "present" => presentLocation,
            "outside" => outsideLocation,
            _ => newLocation
        };

        int id = Random.Range(0, targetLocations.Length);
        Vector3 targetPosition = targetLocations[id].position;

        if (agent != null)
        {
            agent.SetDestination(targetPosition);
        }

        return targetPosition;
    }
    #endregion

    #region Serialized Fields
    [Header("Location Arrays")]
    [SerializeField] public Transform[] newLocation;
    [SerializeField] public Transform[] hallLocation;
    [SerializeField] public Transform[] presentLocation;
    [SerializeField] public Transform[] outsideLocation;

    [Header("References")]
    [SerializeField] private AmbienceScript ambience;
    #endregion
}