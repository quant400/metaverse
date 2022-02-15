using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    public GameObject Loading;
    public InputField inputField;
    public Text errorText;

    private void Start()
    {
        inputField.text = PlayerPrefs.GetString("nickname");
        Loading.SetActive(false);
        errorText.enabled = false;
    }

    public void SetName()
    {
        if(inputField.text.Length > 2 && inputField.text.Length < 20)
        {
            PlayerPrefs.SetString("nickname", inputField.text);
            Loading.SetActive(true);
            errorText.enabled = false;
            StartCoroutine(LoadNextScene());
        }
        else
        {
            errorText.enabled = true;
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(1);
    }
}
