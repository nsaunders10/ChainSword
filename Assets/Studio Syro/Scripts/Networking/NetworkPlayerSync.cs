using UnityEngine;
using Normal.Realtime;
using NaughtyAttributes;
using TMPro;

public class NetworkPlayerSync : RealtimeComponent<NetworkPlayerModel>
{
    public string setName;
    public bool randomColor;
    public Color setColor;
    Color savedColor;

    public TextMeshPro nameText;

    public RealtimeView[] realtimeViews;
    public RealtimeTransform[] realtimeTransforms;        

    public VRRig localVRRig;
    Transform localHmd;
    Transform localLeftHand;
    Transform localRightHand;

    [Header("Remote Player Components")]
    public GameObject remotePlayer;
    public Transform remoteHmd;
    public Transform remoteLeftHand;
    public Transform remoteRightHand;
    public MeshRenderer[] remoteMeshes;
    public MeshRenderer[] localMeshes;

    public MonoBehaviour[] remoteDisable;

    private void Awake()
    {
        localVRRig = FindObjectOfType<VRRig>();

        if (randomColor)
        {
            if (!PlayerPrefs.HasKey("Player Color R"))
            {
                PlayerPrefs.SetFloat("Player Color R", Random.Range(0f, 1f));
                PlayerPrefs.SetFloat("Player Color G", Random.Range(0f, 1f));
                PlayerPrefs.SetFloat("Player Color B", Random.Range(0f, 1f));
            }
        }
    }

    void Start()
    {        

        if (realtimeView.isOwnedLocallySelf)
        {            
            localVRRig.transform.parent = transform;

            localHmd = localVRRig.hmd;
            localLeftHand = localVRRig.leftHand.transform;
            localRightHand = localVRRig.rightHand.transform;

            localMeshes = localVRRig.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer rend in localMeshes)
            {
                //rend.enabled = false;
            }

            foreach (MeshRenderer rend in remoteMeshes)
            {
              //  rend.enabled = false;
            }
            foreach (RealtimeView view in realtimeViews)
            {
                view.RequestOwnership();
            }
            foreach (RealtimeTransform trans in realtimeTransforms)
            {
                trans.RequestOwnership();
            }            
            remotePlayer.SetActive(false);
        }

        if (realtimeView.isOwnedRemotelySelf)
        {
          // localVRRig.gameObject.SetActive(false);

            foreach (MonoBehaviour monoBehaviour in remoteDisable)
            {
                monoBehaviour.enabled = false;
            }
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

            if (setName != PlayerPrefs.GetString("Player Name"))
            {
                model.playerName = PlayerPrefs.GetString("Player Name");
            }

            if (randomColor)
            {
                if (localVRRig.leftHand.menu.down)
                {
                    PlayerPrefs.SetFloat("Player Color R", Random.Range(0f, 1f));
                    PlayerPrefs.SetFloat("Player Color G", Random.Range(0f, 1f));
                    PlayerPrefs.SetFloat("Player Color B", Random.Range(0f, 1f));
                }
            }
            savedColor = new Color(PlayerPrefs.GetFloat("Player Color R"), PlayerPrefs.GetFloat("Player Color G"), PlayerPrefs.GetFloat("Player Color B"));

            if (setColor != savedColor)
            {
               model.playerColor = savedColor;
            }

        }

        if (nameText)
        {
            nameText.text = setName;
        }

        if (remoteMeshes[0].material.color != setColor)
        {
            foreach (MeshRenderer rend in remoteMeshes)
            {
                rend.material.color = setColor;
            }

            foreach (MeshRenderer rend in localMeshes)
            {
                rend.material.color = setColor;
            }
        }
    }

    protected override void OnRealtimeModelReplaced(NetworkPlayerModel previousModel, NetworkPlayerModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.playerNameDidChange -= PlayerNameDidChange;
            previousModel.playerColorDidChange -= PlayerColorDidChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                currentModel.playerName = PlayerPrefs.GetString("Player Name");
                currentModel.playerColor = Color.gray;
            }

            UpdateValues();

            currentModel.playerNameDidChange += PlayerNameDidChange;
            currentModel.playerColorDidChange += PlayerColorDidChange;
        }
    }

    void PlayerNameDidChange(NetworkPlayerModel model, string value)
    {
        UpdateValues();
    }
    void PlayerColorDidChange(NetworkPlayerModel model, Color value)
    {
        UpdateValues();
    }

    void UpdateValues()
    {
        setName = model.playerName;
        setColor = model.playerColor;        
    } 
}