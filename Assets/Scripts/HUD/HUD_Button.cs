using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.Scripts.Types;

public class HUD_Button : MonoBehaviour {

    public ButtonName buttonType;

    // buttons
    private CryButton ESC_COG_button;

    private Button GameMenu_ResumeGame;

    // Image / Panels
    private Image GameMenu;

    // buttons images
    private Sprite ESC_COG_image;
    private Sprite ESC_COG_image_active;

    public void Initialize()
    {
        ESC_COG_image = Resources.Load<Sprite>("HUD/Cog");
        ESC_COG_image_active = Resources.Load<Sprite>("HUD/Cog_Active");
        
        Transform[] allChildren = GetComponentsInChildren<Transform>(true); // we want the transforms that are inactive too with the 'true' parameter.
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "ESC_COG_button":

                    ESC_COG_button = new CryButton();
                    ESC_COG_button.Button = child.transform.GetComponent<Button>();
                    break;

                case "GameMenu":

                    GameMenu = child.transform.GetComponent<Image>();
                    break;

                case "GameMenu_ResumeGame":

                    GameMenu_ResumeGame = child.transform.GetComponent<Button>();
                    break;

                default:
                    break;
            }
        }
    }

    public ButtonName CurrentButton;

    public void MouseHover(int buttonTypeId)
    {
        //Debug.Log(buttonTypeId);
        CurrentButton = (ButtonName)buttonTypeId;
        switch (CurrentButton)
        {
            case ButtonName.ESC_COG:

                if (ESC_COG_button.pressed == false)
                {
                    ESC_COG_button.Button.image.overrideSprite = ESC_COG_image_active;
                }
                break;

            default:
                break;
        }
    }

    public void MouseExit(int buttonTypeId)
    {
        CurrentButton = (ButtonName)buttonTypeId;
        switch (CurrentButton)
        {
            case ButtonName.ESC_COG:

                if (ESC_COG_button.pressed == false)
                {
                    ESC_COG_button.Button.image.overrideSprite = ESC_COG_image;
                }
                break;

            default:
                break;
        }
        CurrentButton = ButtonName.None;
    }

    public void MouseClick(int mouseNameId = 0)
    {
        if (mouseNameId != 0)
        {
            CurrentButton = (ButtonName)mouseNameId;
        }
        HUDAction();
    }

    public void HUDAction(ButtonName keyboardPress = ButtonName.None)
    {
        if (keyboardPress != ButtonName.None)
        {
            CurrentButton = keyboardPress;
        }
        Debug.Log(CurrentButton);
        if (CurrentButton != ButtonName.None)
        {
            switch (CurrentButton)
            {
                case ButtonName.ESC_COG:

                    if (ESC_COG_button.pressed == false)
                    {
                        ESC_COG_button.Button.image.overrideSprite = ESC_COG_image_active;
                        ESC_COG_button.pressed = true;

                        // Show Options
                        GameMenu.transform.gameObject.SetActive(true);

                        Time.timeScale = 0.0f;
                    }
                    else
                    {
                        ESC_COG_button.Button.image.overrideSprite = ESC_COG_image;
                        ESC_COG_button.pressed = false;

                        // Hide Options
                        GameMenu.transform.gameObject.SetActive(false);

                        Time.timeScale = 1.0f;
                    }
                    break;

                case ButtonName.W_HANDS:
                    break;
                default:
                    break;
            }
            CurrentButton = ButtonName.None;
        }
    }
}

public class CryButton
{
    public bool pressed;
    public Button Button;
}