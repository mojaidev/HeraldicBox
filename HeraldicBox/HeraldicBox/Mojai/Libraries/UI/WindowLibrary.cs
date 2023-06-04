using System;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
using ReflectionUtility;
using NCMS;
using System.Collections.Generic;

namespace Mojai.Libraries.UI
{
    class WindowLibrary
    {
        public class EasyScrollWindow
        {
            public ScrollWindow scrollWindow;
            public GameObject content;

            private ScrollRect scrollRect;
            private RectTransform scrollRectTransfrom;
            private RectTransform contentRectTransform;

            public void UpdateVerticalRect(float newSize)
            {
                contentRectTransform.sizeDelta = new Vector2((float)200, newSize);
            }

            public EasyScrollWindow(string id, string title)
            {
                scrollWindow = Windows.CreateNewWindow(id, title);

                GameObject scrollView = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{scrollWindow.name}/Background/Scroll View");
                scrollRect = scrollView.gameObject.GetComponent<ScrollRect>();
                scrollRectTransfrom = scrollView.gameObject.GetComponent<RectTransform>();
                scrollView.gameObject.SetActive(true);

                content = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{scrollWindow.name}/Background/Scroll View/Viewport/Content");
                contentRectTransform = content.gameObject.GetComponent<RectTransform>();

                scrollRectTransfrom.sizeDelta = new Vector2((float)200, (float)215.2);
                scrollRect.vertical = true;
            }
        }

        public static GameObject AddTextToObject(GameObject parent, string text, int fontSize, Vector3 textPosition, string template = "inspect_unit")
        {
            GameObject textTemplate = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{template}/Background/Name");
            GameObject textObject = UnityEngine.Object.Instantiate(textTemplate, parent.transform);
            textObject.SetActive(true);

            Text textComponent = textObject.GetComponent<Text>();
            textComponent.text = text;
            textComponent.fontSize = fontSize;
            RectTransform textRect = textObject.GetComponent<RectTransform>();
            textRect.position = new Vector3(0, 0, 0);
            textRect.sizeDelta = new Vector2(textComponent.preferredWidth, textComponent.preferredHeight);
            textRect.localPosition = textPosition;

            return textObject;
        }
    }
}
