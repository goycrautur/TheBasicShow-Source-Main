using UnityEngine;

public class ITM_Evaporvape : BaseItem
{
    [SerializeField] private GameObject vapeclouds;
    [SerializeField] protected AudioObjectyeah yummyvape;

    public override bool OnUse()
    {
        Vector3 playerPosition = GameControllerScript.Instance.player.transform.position;
        Vector3 snappedPosition = SnapToGrid(playerPosition);
        GameObject gameObject = Instantiate(vapeclouds, snappedPosition, Quaternion.identity);
        GameControllerScript.Instance.lbams.PlayClip(GameControllerScript.Instance.lbams.MainSource3,yummyvape);
        return true;
    }
    private Vector3 SnapToGrid(Vector3 position)
    {
        float gridSize = 10f;

        float centerX = Mathf.Floor(position.x / gridSize) * gridSize;
        float centerZ = Mathf.Floor(position.z / gridSize) * gridSize;

        float snappedX = centerX + (gridSize / 2);
        float snappedZ = centerZ + (gridSize / 2);

        return new Vector3(snappedX, 5, snappedZ);
    }
}
