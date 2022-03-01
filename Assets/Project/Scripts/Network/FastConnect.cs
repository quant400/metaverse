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
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        nickName = PlayerPrefs.GetString("nickname");
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
}
