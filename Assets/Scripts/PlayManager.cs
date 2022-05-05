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
    TextMeshProUGUI scrapRequirementUGUI;

    [SerializeField]
    TextMeshProUGUI systemNameUGUI;

    int index;

    PlayerData data = null;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Start()
    {
        data = SaveManager.LoadPlayerData();
        SetCollection(0);
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
            nextButton.interactable = false;
        else if (index == 0)
            prevButton.interactable = false;
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
            scrapRequirementUGUI.transform.parent.gameObject.SetActive(true);
            scrapRequirementUGUI.text = scrapRequirement + " scraps";
        } else
            scrapRequirementUGUI.transform.parent.gameObject.SetActive(false);
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
