
using UnityEngine;

public class BsodaSparyScript : MonoBehaviour
{
    #region Initialization
    private void Start()
    {
        if (shouldRotate)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = Mathf.Round(Random.Range(0f, 359f));
            transform.eulerAngles = eulerAngles;
        }

        cc = GetComponent<CharacterController>();
        ccDirection = GameControllerScript.Instance.cameraTransform.TransformDirection(Vector3.forward) * speed/100;
        //rb = GetComponent<Rigidbody>();
        //rb.velocity = transform.forward * speed;
    }
    #endregion

    #region Per-Frame Logic
    private void Update()
    {
        if (Time.timeScale != 1) return;
        //rb.velocity = transform.forward * speed;
        cc.Move(ccDirection);

        lifeSpan -= Time.deltaTime;
        if (lifeSpan < 0f) Destroy(gameObject, 0f);
    }
    #endregion

    #region Serialized Configuration
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    public Vector3 ccDirection;

    [Header("Lifespan Settings")]
    [SerializeField] private float lifeSpan;

    [Header("Rotation Settings")]
    public bool shouldRotate;
    #endregion

    #region Internal References
    private Rigidbody rb;
    private CharacterController cc;
    #endregion
}