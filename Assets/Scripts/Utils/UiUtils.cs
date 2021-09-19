using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
    public enum AnchorType
    {
        TopRight, TopLeft,
        LeftCenter,
        Center
    }

    public enum LayoutType
    {
        Square,
        Full
    }

    public enum AspectRatio
    {
        Unregistered,
        Aspect_16_9
    }

    public static class UiUtils
    {
        public static int ScreenWidth;
        public static int ScreenHeight;

        public static readonly Color32 Transparent = new Color32(0, 0, 0, 0);
        public static readonly Color32 Black = new Color32(0, 0, 0, 255);
        public static readonly Color32 White = new Color32(255, 255, 255, 255);
        public static readonly Color32 WhiteTransparent = new Color32(255, 255, 255, 200);

        public static readonly string circle = "";

        public static float GetPercent(float value, float percent)
        {
            return (value / 100f) * percent;
        }

        public static float GetValuePercent(float value, float maxValue)
        {
            return (value * 100f) / maxValue;
        }

        public static float XPercent(float percent)
        {
            return (ScreenWidth / 100f) * percent;
        }
        public static float YPercent(float percent)
        {
            return (ScreenHeight / 100f) * percent;
        }

        public static void SetSize(float x, float y, Button button)
        {
            SetSize(x, y, button.GetComponent<RectTransform>());
            for (int i = 0; i < button.transform.childCount; i++)
            {
                var text = button.transform.GetChild(i).GetComponent<Text>();
                if (text != null)
                    SetTextSize(text, 45);
            }
        }

        public static void SetSize(float x, float y, Image image)
        {
            SetSize(x, y, image.GetComponent<RectTransform>());
        }

        public static void SetSize(float x, float y, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            var w = GetPercent(parent.sizeDelta.x, x);
            var h = GetPercent(parent.sizeDelta.y, y);

            rt.GetComponent<RectTransform>().sizeDelta = new Vector3(w, h);
        }

        public static void SetSize(LayoutType layoutType, RectTransform rt)
        {
            if (layoutType == LayoutType.Full)
            {
                rt.sizeDelta = new Vector3(ScreenWidth, ScreenHeight);
            }
            //rt.sizeDelta = new Vector3(ScreenWidth, ScreenHeight);
        }

        public static void SetSize(float x, LayoutType layoutType, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            var w = GetPercent(parent.sizeDelta.x, x);
            float h = w;

            rt.sizeDelta = new Vector3(w, h);
        }

        public static void SetSize(LayoutType layoutType, float y, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            float w;
            var h = GetPercent(parent.sizeDelta.y, y);
            w = h;

            rt.sizeDelta = new Vector3(w, h);
        }

        public static void SetFixedSize(float x, float y, RectTransform rt)
        {
            rt.GetComponent<RectTransform>().sizeDelta = new Vector3(x, y);
        }

        public static void SetPosition(float x, float y, Button button)
        {
            SetPosition(x, y, button.GetComponent<RectTransform>());
        }

        public static void SetPosition(float x, float y, Image image)
        {
            SetPosition(x, y, image.GetComponent<RectTransform>());
        }

        public static void SetPosition(float x, float y, RectTransform rt)
        {
            var parent = rt.transform.parent.GetComponent<RectTransform>();

            var w = GetPercent(parent.sizeDelta.x, x);
            var h = GetPercent(parent.sizeDelta.y, y);

            rt.GetComponent<RectTransform>().localPosition = new Vector3(w, -h, 0f);
        }

        public static void SetFixedPosition(float x, float y, RectTransform rt)
        {
            rt.GetComponent<RectTransform>().localPosition = new Vector3(x, -y, 0f);
        }

        public static void SetAnchor(AnchorType at, RectTransform rt)
        {
            Vector2 anchor;
            Vector2 pivot;

            if (at == AnchorType.TopLeft)
            {
                anchor = new Vector2(0, 1);
                pivot = new Vector2(0, 1);
            }
            else if (at == AnchorType.LeftCenter)
            {
                anchor = new Vector2(0, 0.5f);
                pivot = new Vector2(0.5f, 0.5f);
            }
            else if (at == AnchorType.Center)
            {
                anchor = new Vector2(0.5f, 0.5f);
                pivot = new Vector2(0.5f, 0.5f);
            }
            else
            {
                anchor = new Vector2(1, 1);
                pivot = new Vector2(1, 1);
            }

            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = pivot;
        }

        public static void SetAnchor(AnchorType at, Button button)
        {
            SetAnchor(at, button.GetComponent<RectTransform>());
        }

        public static void SetAnchor(AnchorType at, Image image)
        {
            SetAnchor(at, image.GetComponent<RectTransform>());
        }

        public static void SetIconSize(Text text)
        {
            var parent = text.transform.parent.GetComponent<RectTransform>();

            text.fontSize = (int)parent.sizeDelta.y;
        }

        public static void SetTextSize(Text text, float predefinedValue = 0)
        {
            var parentHeight = text.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
            var textHeight = text.transform.GetComponent<RectTransform>().sizeDelta.y;

            if (predefinedValue > 1)
            {
                text.fontSize = (int)GetPercent(parentHeight, predefinedValue);
                return;
            }
            else if (Math.Abs(textHeight) < 1)
            {
                text.fontSize = (int)GetPercent(parentHeight, 99);
                return;
            }
            var ratio = GetValuePercent(textHeight, parentHeight);
            text.fontSize = (int)GetPercent(textHeight, ratio);
        }
        
        public static float GCD(float x, float y)
        {
            return Math.Abs(y) < 1 ? x : GCD(y, x % y);
        }

        public static AspectRatio GetAspectRatio()
        {
            var x = ScreenWidth / GCD(ScreenWidth, ScreenHeight);
            var y = ScreenHeight / GCD(ScreenWidth, ScreenHeight);
            
            if ((Math.Abs(x - 16) < 0.1f && Math.Abs(y - 9) < 0.1f) ||
                (Math.Abs(x - 427) < 2 && Math.Abs(y - 240) < 2) ||
                (Math.Abs(x - 80) < 2 && Math.Abs(y - 39) < 2) ||  //  480	× 234
                (Math.Abs(x - 30) < 2 && Math.Abs(y - 17) < 2) ||  //  480	×	272
                (Math.Abs(x - 53) < 2 && Math.Abs(y - 30) < 2) ||
                (Math.Abs(x - 128) < 2 && Math.Abs(y - 75) < 2) ||
                (Math.Abs(x - 71) < 2 && Math.Abs(y - 40) < 2) ||
                (Math.Abs(x - 667) < 2 && Math.Abs(y - 375) < 2) ||
                (Math.Abs(x - 683) < 2 && Math.Abs(y - 384) < 2) ||
                (Math.Abs(x - 222) < 2 && Math.Abs(y - 125) < 2))
            {
                return AspectRatio.Aspect_16_9;
            }

            return AspectRatio.Unregistered;
        }

        public static string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
            hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
            byte a = 255;//assume fully visible unless specified in hex
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            //Only use alpha if the string has enough characters
            if (hex.Length == 8)
            {
                a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            }
            return new Color32(r, g, b, a);
        }
    }
}