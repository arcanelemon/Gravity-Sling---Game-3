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
    SystemCollection[] systemCollections;

    [SerializeField]
    TextMeshProUGUI scrapRequirementUGUI;

    [SerializeField]
    TextMeshProUGUI systemNameUGUI;

    int index;

    int totalScraps = 480;

    #endregion Variables
    
    #region Engine

    // Start is called before the first frame update
    void Start()
    {
        // Check if new system unlocked
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (index == systemCollections.Length - 1)
            nextButton.interactable = false;
        else if (index == 0)
            prevButton.interactable = false;
        else 
        { 
            nextButton.interactable = true;
            prevButton.interactable = true;
        }

        foreach (SystemCollection collection in systemCollections)
        {
            collection.gameObject.SetActive(false);

        }

        systemCollections[id].gameObject.SetActive(true);
        systemNameUGUI.text = systemCollections[id].SystemName.ToUpper();

        int scrapRequirement = systemCollections[id].ScrapRequirement - totalScraps;

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
