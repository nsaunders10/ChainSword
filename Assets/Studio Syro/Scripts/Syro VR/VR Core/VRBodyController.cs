using UnityEngine;


public class VRBodyController : MonoBehaviour
{
    public Transform hmd;
    public Transform neck;

    public float bodyLerpSpeed = 6;

    void Start()
    {
        neck.parent = hmd;
        neck.localPosition = new Vector3(0, 0, -0.15f);
    }

    void Update()
    {
        transform.position = neck.position + new Vector3(0, -0.11f, 0);
        Quaternion targetRot = hmd.rotation;
        targetRot.x = 0;
        targetRot.z = 0;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, bodyLerpSpeed * Time.deltaTime);
    }
}
