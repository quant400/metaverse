using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChatManager : MonoBehaviourPun
{
    public InputField inputField;
    public GameObject LinePrefab;
    public Transform ChatContainer;
    public Text Info;

    private void Update()
    {
        if (!PhotonNetwork.LocalPlayer.IsLocal)
            return;

        if(inputField.interactable)
        {
            inputField.Select();
            inputField.ActivateInputField();
            Info.enabled = false;
        }
        else
        {
            inputField.DeactivateInputField();
            Info.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!inputField.interactable)
            {
                inputField.interactable = true;
            }
            else
            {
                if (!string.IsNullOrEmpty(inputField.text))
                {
                    photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName + ": " + inputField.text);
                    inputField.interactable = false;
                    inputField.text = "";
                }
                else
                {
                    Debug.Log("Won't send empty");
                }
            }
        }
    }

    [PunRPC]
    public void SendChat(string msg)
    {
        GameObject line = Instantiate(LinePrefab, ChatContainer);
        line.GetComponent<Text>().text = msg;
    }
}
