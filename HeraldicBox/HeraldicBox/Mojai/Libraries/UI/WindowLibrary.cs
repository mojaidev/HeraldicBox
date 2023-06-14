using System;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine;
using UnityEngine.UI;

namespace Mojai.Libraries.UI
{
    class WindowLibrary
    {
        public class EasyInputField
        {
            public GameObject Element;
            public NameInput Input;

            public EasyInputField(Transform parent, string value, Vector3 position, UnityEngine.Events.UnityAction<string> pAction = null)
            {
                // This is to get the NameInputElement from 'inspect_unit' cus when game starts it doesnt exist.
                Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");
                ScrollWindow.get("inspect_unit").gameObject.SetActive(false);

                Element = UnityEngine.Object.Instantiate(GameObjects.FindEvenInactive("NameInputElement"), parent.transform);
                Input = Element.GetComponent<NameInput>();
                Element.transform.localPosition = position;
                Element.SetActive(true);

                Input.setText(value);
                Input.addListener(pAction);
            }
        }

        public class EasyInner
        {
            public GameObject inner;

            public EasyInner(String name, Transform parent, Vector3 size, Vector3 position)
            {
                inner = new GameObject(name);
                inner.AddComponent<Image>().sprite = Resources.Load<Sprite>("ui/icons/windowInnerSliced");
                inner.transform.SetParent(parent);
                inner.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
                inner.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
                inner.transform.localScale = new Vector3(1, 1, 1);
                inner.transform.localPosition = position;
                inner.GetComponent<RectTransform>().sizeDelta = size;
            }
        }

        public class EasyScrollWindow
        {
            public ScrollWindow scrollWindow;
            public GameObject content;

            private ScrollRect scrollRect;
            private RectTransform scrollRectTransfrom;
            private RectTransform contentRectTransform;

            public void Clear()
            {
                for (int i = 0; i < content.transform.childCount; i++)
                {
                    GameObject obj = content.transform.GetChild(i).gameObject;
                    UnityEngine.Object.Destroy(obj);
                }
            }

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
