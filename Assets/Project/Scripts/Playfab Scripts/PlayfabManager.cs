using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;
    public Text LoginNumber;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitPlayfab(string accountID)
    {
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            TitleId = "E1955",
            CustomId = accountID,
            CreateAccount = true
        },
        (res) =>
        {
            SetDisplayName();
            SetLoginNumber();
        },
        (error) =>
        {
            Debug.LogError(error.ErrorDetails + " " + error.ErrorMessage);
        });
    }

    void SetDisplayName()
    {

    }

    void SetLoginNumber()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {

        },
        (res) =>
        {
            if (res.Data.Count < 1)
                InitFirstLogin();
            else
                IncrementLogin(res.Data);
        },
        (error) =>
        {
            Debug.Log(error.ErrorDetails + error.ErrorMessage);
        });
    }

    void InitFirstLogin()
    {
        Dictionary<string, string> myData = new Dictionary<string, string>();
        myData.Add("Login", "1");

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = myData
        },
        (res) =>
        {
            LoginNumber.text = "1";
        },
        (error) =>
        {
            Debug.Log(error.ErrorDetails + error.ErrorMessage);
        });
    }

    void UpdateLogins(int logins)
    {
        Dictionary<string, string> myData = new Dictionary<string, string>();
        myData.Add("Login", logins.ToString());

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = myData
        },
        (res) =>
        {
            LoginNumber.text = logins.ToString();
        },
        (error) =>
        {
            Debug.Log(error.ErrorDetails + error.ErrorMessage);
        });
    }

    void IncrementLogin(Dictionary<string, UserDataRecord> keyValuePair)
    {
        LoginNumber.text = "-";
        UserDataRecord userData;
        keyValuePair.TryGetValue("Login", out userData);
        int logins = int.Parse(userData.Value);
        UpdateLogins(logins + 1);
    }
}
