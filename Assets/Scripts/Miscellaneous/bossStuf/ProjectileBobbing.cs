using UnityEngine;

public class ProjectileBobbing : MonoBehaviour
{
    public float center = 1f;
    private void OnWillRenderObject()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, bobValStuf.GetBobValue() + center, transform.localPosition.z);
    }
}
