using UnityEngine;
using Normal.Realtime;
using NaughtyAttributes;
using TMPro;

public class NetworkPlayer : MonoBehaviour
{
    public RealtimeView realtimeView;
    public NetworkPlayerSync playerSync;

    public TextMeshPro nameText;   
    public RealtimeView[] realtimeViews;
    public RealtimeTransform[] realtimeTransforms;

    VRRig localVRRig;
    Transform localHmd;
    Transform localLeftHand;
    Transform localRightHand;

    [Header("Remote Player Components")]
    public GameObject remotePlayer;
    public Transform remoteHmd;
    public Transform remoteLeftHand;
    public Transform remoteRightHand;
    public MeshRenderer[] remoteMeshes;

    void Start()
    {
        if (realtimeView.isOwnedLocallySelf)
        {
            localVRRig = FindObjectOfType<VRRig>();
            localVRRig.transform.parent = transform;

            localHmd = localVRRig.hmd;
            localLeftHand = localVRRig.leftHand.transform;
            localRightHand = localVRRig.rightHand.transform;

            foreach (MeshRenderer rend in remoteMeshes)
            {
                rend.enabled = false;
            }
            foreach (RealtimeView view in realtimeViews)
            {
                view.RequestOwnership();
            }
            foreach (RealtimeTransform trans in realtimeTransforms)
            {
                trans.RequestOwnership();
            }
        }
        if (realtimeView.isOwnedRemotelySelf)
        {
            localVRRig.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (realtimeView.isOwnedLocallySelf)
        {                      
            remoteLeftHand.position = localLeftHand.position;
            remoteLeftHand.rotation = localLeftHand.rotation;

            remoteRightHand.position = localRightHand.position;
            remoteRightHand.rotation = localRightHand.rotation;

            remoteHmd.position = localHmd.position;
            remoteHmd.rotation = localHmd.rotation;       

        }

        nameText.text = playerSync.setName;

        if (remoteMeshes[0].material.color != playerSync.setColor)
        {
            foreach (MeshRenderer rend in remoteMeshes)
            {
                rend.material.color = playerSync.setColor;
            }
        }
    }
}