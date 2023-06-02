using NCMS.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Mojai.Libraries.UI
{
    class ButtonLibrary
    {
        public class WindowBackGroundButton
        {
            public PowerButton powerButton;
            public Image buttonBackGround;
            public Button buttonObject;

            public WindowBackGroundButton(GameObject buttonParent, string buttonID, string buttonName, string buttonDesc, Sprite buttonSprite, float buttonX, float buttonY, Sprite buttonBackGroundSprite, UnityEngine.Events.UnityAction callBack = null)
            {
                powerButton = PowerButtons.CreateButton(
                    buttonID,
                    buttonSprite,
                    buttonName,
                    buttonDesc,
                    new Vector2(buttonX, buttonY),
                    ButtonType.Click,
                    buttonParent.transform,
                    callBack
                );

                buttonBackGround = powerButton.gameObject.GetComponent<Image>();
                buttonBackGround.sprite = buttonBackGroundSprite;
                buttonObject = powerButton.gameObject.GetComponent<Button>();
                buttonBackGround.rectTransform.localScale = Vector3.one;

            }
        }

        public class WindowSideButton
        {
            // Will be used in future
            public WindowBackGroundButton windowBackGroundButton;
            // I dont remember what i was gonna do in the future


            public enum PositionSlot
            {
                slot1 = -55
                // ill add more in future.
            }

            public enum Side
            {
                rightSide = 245
                // ill add more in the future
            }

            public WindowSideButton(GameObject buttonParent, string buttonID, Side buttonX, PositionSlot buttonY, string buttonName, string buttonDescription, Sprite buttonSprite, ButtonType buttonType, UnityEngine.Events.UnityAction callBack = null)
            {
                windowBackGroundButton = new WindowBackGroundButton(
                    buttonParent,
                    buttonID,
                    buttonName,
                    buttonDescription,
                    buttonSprite,
                    (float)buttonX,
                    (float)buttonY,
                    Resources.Load<Sprite>("ui/icons/backgroundTabButton"),
                    callBack
                );
            }
        }
    }
}
