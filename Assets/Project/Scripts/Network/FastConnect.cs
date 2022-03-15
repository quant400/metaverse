using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FastConnect : MonoBehaviourPunCallbacks
{
    public string nickName;
    public static NFTInfo chosenNFT;
    private string chosenNFTName;
    public GameObject startCamera;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
        PhotonNetwork.SerializationRate = 6;
        PhotonNetwork.SendRate = 6;
    }
    void Start()
    {
        nickName = PlayerPrefs.GetString("nickname");
        if (PlayfabManager.instance != null)
        {
            if (!string.IsNullOrEmpty(PlayfabManager.instance.DisplayName))
                nickName = PlayfabManager.instance.DisplayName;
        }

        if (!PhotonNetwork.IsConnected)
        {
            Connect();
        }
    }

    void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.NickName = nickName;
        PhotonNetwork.JoinOrCreateRoom("TEST", new RoomOptions { MaxPlayers = 0 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        startCamera.SetActive(false);
        if (PhotonNetwork.LocalPlayer.IsLocal)
            PhotonNetwork.Instantiate(selectNFTName(), new Vector3(PhotonNetwork.LocalPlayer.ActorNumber + 30, 0, 30), Quaternion.identity);
    }
    public string selectNFTName()
    {
        string nameofPlayer;
        chosenNFTName = NameToSlugConvert(chosenNFT.name);
        return nameofPlayer = "SinglePlayerPrefabs/Characters/" + chosenNFTName;
    }
    string NameToSlugConvert(string name)
    {
        string slug;
        slug = name.ToLower().Replace(".", "").Replace("'", "").Replace(" ", "-");
        return slug;

    }

    private void Update()
    {
        Debug.Log(PhotonNetwork.IsMessageQueueRunning);
    }
}
