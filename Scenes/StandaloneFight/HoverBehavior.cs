using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverBehavior : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private GameObject PopupWindow;

    public GameObject Popup;
    public Text PotionName;
    public Text PotionText;
    public Image PopupImage;
    public Sprite PotionImage;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {

        switch (gameObject.name) {

            case "hp":

                PopupWindow = Instantiate(Popup, gameObject.transform);
                PopupWindow.transform.position = new Vector3(gameObject.transform.position.x + 2f, gameObject.transform.position.y +1f, gameObject.transform.position.z);
                PotionName.text = "Small Potion of Healing";
                PotionText.text = "When consumed this potion allows user to restore their Health by 20 points.";
                PopupImage.sprite = PotionImage;


            break;

            case "mp":

                PopupWindow = Instantiate(Popup, gameObject.transform);
                PopupWindow.transform.position = new Vector3(gameObject.transform.position.x + 2f, gameObject.transform.position.y - 1f, gameObject.transform.position.z);
                PotionName.text = "Small Potion of Mana";
                PotionText.text = "When consumed this potion allows user to restore their Mana by 15 points.";
                PopupImage.sprite = PotionImage;

            break;

            default:
                //nothing
            break;

        }

    }


    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (PopupWindow != null)
            Destroy(PopupWindow);

    }
}