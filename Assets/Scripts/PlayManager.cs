using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayManager : MonoBehaviour
{
    #region Variables

    [SerializeField]
    Button nextButton;

    [SerializeField]
    Button prevButton;

    [SerializeField]
    PlanetSystemData[] planetSystems;

    [SerializeField]
    PlanetButton[] planetButtons;

    [SerializeField]
    TextMeshProUGUI totalScrapUGUI;

    [SerializeField]
    TextMeshProUGUI scrapRequirementUGUI;

    [SerializeField]
    TextMeshProUGUI systemNameUGUI;

    int index;

    Animator animator;

    PlayerData data = null;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        data = SaveManager.LoadPlayerData();
        SetCollection(0);
        totalScrapUGUI.text = data.scraps.ToString();

    }

    #endregion Engine

    #region Private Methods

    void CreateSystemUnlockedPopup() 
    {
    
    }

    #endregion Private Methods

    #region Public Methods

    public void LoadLevel(string scene) 
    {
        SceneManager.LoadScene(scene);
    }

    public void SetCollection(int id) 
    {
        if (index == planetSystems.Length - 1)
        {
            prevButton.interactable = true;
            nextButton.interactable = false;
            nextButton.Select();
        }
        else if (index == 0)
        {
            nextButton.interactable = true;
            prevButton.interactable = false;
            prevButton.Select();
        }
        else
        {
            nextButton.interactable = true;
            prevButton.interactable = true;
        }

        int i = 0;
        foreach (PlanetData planet in planetSystems[id].Planets)
        {
            // set button to planet sprite and target=
            planetButtons[i].UpdateButton(planet);


            // see if prevous planet has been played

            i++;
        }

        systemNameUGUI.text = planetSystems[id].SystemName.ToUpper();

        int scrapRequirement = planetSystems[id].ScrapRequirement - (data == null ? 0 : data.scraps);

        if (scrapRequirement > 0)
        {
            animator.Play("Disabled");
            scrapRequirementUGUI.transform.parent.gameObject.SetActive(true);
            scrapRequirementUGUI.text = scrapRequirement + " scraps";

            foreach (Button button in planetButtons)
                button.interactable = false;
        }
        else
        {
            animator.Play("Enabled");
            scrapRequirementUGUI.transform.parent.gameObject.SetActive(false);

            foreach (Button button in planetButtons)
                button.interactable = true;
        }
    }

    public void NextCollection() 
    {
        index++;
        SetCollection(index);
    }

    public void PreviousCollection() 
    {
        index--;
        SetCollection(index);
    }

    #endregion Public Methods
}
