
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.IO;
using UniRx.Triggers;
using UniRx;
using UniRx.Operators;
public class characterSelectionView : MonoBehaviour
{
    public int currentCharacter;
    [SerializeField]
    Camera cam;
    [SerializeField]
    Transform[] characters;
    [SerializeField]
    Transform characterList;
   
    bool selected;
    [SerializeField]
    float sideCharZdisp;
    [SerializeField]
    Button rightButton, leftButton , select;
    NFTInfo[] myNFT;
    [SerializeField]
    RuntimeAnimatorController controller;
    [SerializeField]
    GameObject buttonsToEnable, ButtonToDisable;
    [SerializeField]
    NFTInfo[] characterNFTMap;
    public void Start()
    {
        observeCharacterSelectionBtns();
        //observesessionCounter();
    }
    public void observesessionCounter()
    {
      
        gameplayView.instance.dailysessionReactive
            .Do(_ => setPlayButtonDependtoSessions(_))
            .Do(_=>chickenGameModel.currentNFTSession=_)
            .Subscribe()
            .AddTo(this);
    }
    void setPlayButtonDependtoSessions(int sessions)
    {
        if (sessions >= 10)
        {
            select.interactable = false;
            
        }
        else
        {
            select.interactable = true;
        }
    }
    void observeCharacterSelectionBtns()
    {
        rightButton.OnClickAsObservable()
            .Do(_ => MoveRight())
            .Where(_=>PlaySounds.instance!=null)
            .Do(_=> PlaySounds.instance.Play())
            .Subscribe()
            .AddTo(this);
        leftButton.OnClickAsObservable()
           .Do(_ => MoveLeft())
           .Where(_ => PlaySounds.instance != null)
           .Do(_ => PlaySounds.instance.Play())
           .Subscribe()
           .AddTo(this);
        select.OnClickAsObservable()
         .Do(_ => FinalSelectSinglePlayer())
         .Where(_ => PlaySounds.instance != null)
         .Do(_ => PlaySounds.instance.Play())
         .Do(_ => chickenGameModel.gameCurrentStep.Value = chickenGameModel.GameSteps.OnCharacterSelected)
         .Subscribe()
         .AddTo(this);
    }
    //temp flag for skip 
    bool skip = false;
    public void MoveRight()
    {
        rightButton.interactable = false;
        leftButton.interactable = false;
        if (currentCharacter < characters.Length - 1)
        {
            if (selected)
            {
                characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", false);
                selected = false;
            }
            characters[currentCharacter].transform.localPosition += new Vector3(0, 0, sideCharZdisp);
            currentCharacter++;
            characters[currentCharacter].transform.DOLocalMove(characters[currentCharacter].transform.localPosition + new Vector3(0, 0, -sideCharZdisp), 0.5f);
            cam.transform.DOMoveX(characters[currentCharacter].transform.position.x, 0.5f).OnStepComplete(() =>
            {

                rightButton.interactable = true;
                leftButton.interactable = true;
            });
        }
        else
        {
            characters[currentCharacter].transform.localPosition += new Vector3(0, 0, sideCharZdisp);
            currentCharacter = 0;
            characters[currentCharacter].transform.DOLocalMove(characters[currentCharacter].transform.localPosition + new Vector3(0, 0, -sideCharZdisp), 0.5f);
            cam.transform.DOMoveX(characters[currentCharacter].transform.position.x, 0.5f).OnStepComplete(() =>
            {

                rightButton.interactable = true;
                leftButton.interactable = true;
            });
        }
      //  gameplayView.instance.chosenNFT = characterNFTMap[currentCharacter];
       // gameplayView.instance.GetScores();
       FastConnect.chosenNFT= characterNFTMap[currentCharacter];
    }

    public void MoveLeft()
    {
        if (currentCharacter > 0)
        {
            if (selected)
            {
                characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", false);
                selected = false;
            }
            characters[currentCharacter].transform.localPosition += new Vector3(0, 0, sideCharZdisp);
            currentCharacter--;
            characters[currentCharacter].transform.DOLocalMove(characters[currentCharacter].transform.localPosition + new Vector3(0, 0, -sideCharZdisp), 0.5f);

            cam.transform.DOMoveX(characters[currentCharacter].transform.position.x, 0.5f).OnStepComplete(() =>
            {
                rightButton.interactable = true;
                leftButton.interactable = true;
            });
        }
        else
        {
            characters[currentCharacter].transform.localPosition += new Vector3(0, 0, sideCharZdisp);
            currentCharacter = characters.Length - 1;
            characters[currentCharacter].transform.DOLocalMove(characters[currentCharacter].transform.localPosition + new Vector3(0, 0, -sideCharZdisp), 0.5f);
            cam.transform.DOMoveX(characters[currentCharacter].transform.position.x, 0.5f).OnStepComplete(() =>
            {

                rightButton.interactable = true;
                leftButton.interactable = true;
            });
        }
        // gameplayView.instance.chosenNFT = characterNFTMap[currentCharacter];
        //  gameplayView.instance.GetScores();
        FastConnect.chosenNFT = characterNFTMap[currentCharacter];

    }


    public void Selected()
    {
       // GameRoom.room.ChooseAvatar("PlayerAvatar" + currentCharacter);
        selected = true;
        characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", true);

    }


    public void UndoFinalSelect()
    {
        rightButton.interactable = true;
        leftButton.interactable = true;
    }

    //added for single player
    public void FinalSelectSinglePlayer()
    {
       // gameplayView.instance.chosenNFT = characterNFTMap[currentCharacter];
        selected = true;
        characters[currentCharacter].GetComponent<Animator>().SetBool("Selected", true);

        FastConnect.chosenNFT = characterNFTMap[currentCharacter];

    }



    internal void SetData(NFTInfo[] nFTData)
    {
        //will be respossible for setting up characters according to nft
        myNFT = nFTData;
        SetUpCharacters();

    }


    private void SetUpCharacters()
    {
        if (chickenGameModel.charactersSetted == false)
        {

            characters = new Transform[myNFT.Length];
            characterNFTMap = new NFTInfo[myNFT.Length];
            int currentindex = 1;
            for (int i = 0; i < myNFT.Length; i++)
            {
                string charName = NameToSlugConvert(myNFT[i].name);
                Debug.Log(charName);
                GameObject charModel = Resources.Load(Path.Combine("SinglePlayerPrefabs/DisplayModels", charName)) as GameObject;
                GameObject temp = Instantiate(charModel, characterList);
                temp.transform.localEulerAngles = new Vector3(0, 180, 0);
                if (i == 0)
                {
                    temp.transform.localPosition = new Vector3(0, -0.1f, 0);
                    characters[0] = temp.transform;
                    characterNFTMap[0] = myNFT[i];
                }
                else if (i % 2 == 0)
                {
                    temp.transform.localPosition = new Vector3(-currentindex, -0.1f, 0.2f);
                    characters[characters.Length - currentindex] = temp.transform;
                    characterNFTMap[characters.Length - currentindex] = myNFT[i];
                    currentindex++;
                }
                else if (i % 2 != 0)
                {
                    temp.transform.localPosition = new Vector3(currentindex, -0.1f, 0.2f);
                    characters[currentindex] = temp.transform;
                    characterNFTMap[currentindex] = myNFT[i];

                }
                temp.GetComponent<Animator>().runtimeAnimatorController = controller;

            }
            chickenGameModel.charactersSetted = true;
        }

        Done();


    }

    // for skip to test all characters
    public void Skip()
    {
        var info = Resources.LoadAll("SinglePlayerPrefabs/DisplayModels", typeof(GameObject));
        characters = new Transform[info.Length];
        characterNFTMap = new NFTInfo[info.Length];
        int currentindex = 1;
        for (int i = 0; i < characters.Length; i++)
        {
            string name = info[i].name;
            GameObject charModel = Resources.Load(Path.Combine("SinglePlayerPrefabs/DisplayModels", name)) as GameObject;
            GameObject temp = Instantiate(charModel, characterList);
            temp.transform.localEulerAngles = new Vector3(0, 180, 0);
            if (i == 0)
            {
                temp.transform.localPosition = new Vector3(0, -0.1f, 0);
                characters[0] = temp.transform;
                characterNFTMap[0] = new NFTInfo { id = 175, name = name };
            }
            else if (i % 2 == 0)
            {
                temp.transform.localPosition = new Vector3(-currentindex, -0.1f, 0.2f);
                characters[characters.Length - currentindex] = temp.transform;
                characterNFTMap[characters.Length - currentindex] = new NFTInfo { id = 175, name = name };
                currentindex++;
            }
            else if (i % 2 != 0)
            {
                temp.transform.localPosition = new Vector3(currentindex, -0.1f, 0.2f);
                characters[currentindex] = temp.transform;
                characterNFTMap[currentindex] = new NFTInfo { id = 175, name = name };
            }

            temp.GetComponent<Animator>().runtimeAnimatorController = controller;
        }

        Done();
    }

    private void Done()
    {
        buttonsToEnable.SetActive(true);
        ButtonToDisable.SetActive(false);
    }



    string NameToSlugConvert(string name)
    {
        string slug;
        slug = name.ToLower().Replace(".", "").Replace("'", "").Replace(" ", "-");
        return slug;

    }
}

