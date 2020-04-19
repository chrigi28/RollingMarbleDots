using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject GridContent;
    [SerializeField] private Button ButtonPrefab;

    private List<Button> levels = new List<Button>();

    void Awake()
    {
        
        ////var levelData = GameManager.Instance.PlayerData.LevelDatas?.OrderBy(f => f.levelNumber);
        ////if (levelData != null)
        ////{
        ////    foreach (var level in levelData)
        ////    {
        ////        var button = Instantiate(ButtonPrefab, GridContent.transform);
        ////        // get button text component in children and set the text property
        ////        button.GetComponentInChildren<Text>().text = level.levelNumber.ToString();
        ////        var stars = button.GetComponentsInChildren<Image>(true);
                
        ////        switch (level.stars)
        ////        {
        ////            case 3: stars[2].enabled = true;
        ////                    goto case 2;
        ////            case 2: stars[1].enabled = true;
        ////                    goto case 1;
        ////            case 1: stars[0].enabled = true;
        ////                break;
        ////        }
        ////    }

            for (int i = 1; i < 20; i++)
            {
                var button = Instantiate(ButtonPrefab, GridContent.transform);
                // get button text component in children and set the text property
                var textmesh = button.transform.Find("LevelButtonText").GetComponent<TextMeshProUGUI>();
                textmesh.text = i.ToString();
                var stars = button.GetComponentsInChildren<Image>(true);
                var lockImage = button.transform.Find("Lock").GetComponent<Image>();
                if (i < 4)
                {
                    lockImage.enabled = false;
                }

                levels.Add(button);
            }
    }                                                           
}
