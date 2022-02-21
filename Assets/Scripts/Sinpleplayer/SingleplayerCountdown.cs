using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class SingleplayerCountdown : MonoBehaviour
{
    [SerializeField]
    TMP_Text countdownText;
    int time;
    float timeLeft;
    void OnEnable()
    {
        //Invoke("StartCountDown", 0.5f);
        //time = SingleplayerGameControler.instance.startDelay;
        //StartCountDown();
    }
    public void StartCountDown()
    {
        gameplayView.instance.StartGame();
        //StartCoroutine("SinglePlayerCountDown",1f);
    }

    IEnumerator SinglePlayerCountDown(float delay)
    {
        DOTween.To(() => countdownText.fontSize, x => countdownText.fontSize = x, 0, delay);
        yield return new WaitForSeconds(delay);
        for (int i = time; i >= 0; i--)
        {
            countdownText.text = i.ToString();
            if (i == 0)
                countdownText.text = "GO!";
            countdownText.fontSize = 150;
            DOTween.To(() => countdownText.fontSize, x => countdownText.fontSize = x, 0, 1f);
            yield return new WaitForSeconds(1);
        }
        gameplayView.instance.StartGame();
        this.gameObject.SetActive(false);
    }


}
