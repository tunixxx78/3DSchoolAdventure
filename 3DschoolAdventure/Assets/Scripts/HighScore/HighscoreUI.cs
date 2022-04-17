using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighscoreUI : MonoBehaviour
{

    [SerializeField] GameObject highscoreUiElement;
    [SerializeField] Transform elementWrapper;

    List<GameObject> uiElement = new List<GameObject>();

    private void OnEnable()
    {
        HighscoreHandler.onHighscoreListChanged += UpdateUI;
    }

    private void OnDisable()
    {
        HighscoreHandler.onHighscoreListChanged -= UpdateUI;
    }

    private void UpdateUI (List<HighscoreElement> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            HighscoreElement el = list[i];

            if(el.points > 0)
            {
                if(i >= uiElement.Count)
                {
                    var inst = Instantiate(highscoreUiElement, Vector3.zero, Quaternion.identity);
                    inst.transform.SetParent(elementWrapper, false);

                    uiElement.Add(inst);

                    
                }
            }
            var texts = uiElement[i].GetComponentsInChildren<TMP_Text>();
            texts[0].text = el.playerName;
            texts[1].text = el.points.ToString();
        }
    }
}
