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

        public class Window
        {
            public string windowID, windowTitle; // <-- Not necessary but im too lazy to delete this.
            public ScrollWindow scrollWindow;
            public GameObject windowGameObject;

            public void updateScrollRect(GameObject content, int count, int size)
            {
                var scrollRect = content.GetComponent<RectTransform>();
                scrollRect.sizeDelta = new Vector2(0, count * size);
            }

            public void Show()
            {
                Windows.ShowWindow(this.windowID);
            }

            public Window(string id, string title)
            {
                scrollWindow = Windows.CreateNewWindow(id, title);

                windowID = id;
                windowTitle = title;

                var scrollView = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{scrollWindow.name}/Background/Scroll View");
                scrollView.gameObject.SetActive(true);

                windowGameObject = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{scrollWindow.name}/Background/Scroll View/Viewport/Content");
            }
        }

        public static GameObject AddTextToWindow(WindowLibrary.Window Window, string text, int fontSize, Vector3 textPosition)
        {
            GameObject textTemplate = GameObject.Find($"/Canvas Container Main/Canvas - Windows/windows/{Window.scrollWindow.name}/Background/Name");
            GameObject textObject = UnityEngine.Object.Instantiate(textTemplate, Window.scrollWindow.gameObject.transform);
            textObject.SetActive(true);

            var textComponent = textObject.GetComponent<Text>();
            textComponent.text = text;
            textComponent.fontSize = fontSize;
            var textRect = textObject.GetComponent<RectTransform>();
            textRect.position = new Vector3(0, 0, 0);
            textRect.localPosition = textPosition;
            textRect.sizeDelta = new Vector2(textComponent.preferredWidth, textComponent.preferredHeight);

            return textObject;
        }
    }
}
