using UnityEngine;

public class ITM_NoSquee : BaseItem
{
    public override bool OnUse()
    {
        Vector3 playerPosition = GameControllerScript.Instance.player.transform.position;
        Vector3 snappedPosition = SnapToGrid(playerPosition);

        Instantiate(WDNSModel, snappedPosition, Quaternion.identity);
        GameControllerScript.Instance.audioDevice.PlayOneShot(aud_Spray);
        if (SummonSubtitles)
        {
            if (Subtitles != null)
            {
            GameControllerScript.Instance.SubsManager.summonLeSubtitle2D(Subtitles.subtitleOption,Subtitles,0f,new Vector3(0f,-170.5f,0f),GameControllerScript.Instance.audioDevice);
            }
        }

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


    [SerializeField] private GameObject WDNSModel;
    [SerializeField] protected AudioClip aud_Spray;
    [SerializeField] private bool SummonSubtitles;
    [SerializeField] private subsScriptableObject Subtitles;
}