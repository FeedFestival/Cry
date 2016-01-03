using UnityEngine;
using System.Collections;
using Assets.Scripts.Types;
using UnityEngine.UI;
using System.Linq;
using System;

public class Actor : MonoBehaviour
{
    public ActorName Name;

    public LinePause LinePause;

    [HideInInspector]
    public Transform thisTransform;

    [HideInInspector]
    public Text[] DialogText;
    [HideInInspector]
    public int DialogTextIndex;
    [HideInInspector]
    public Image DialogBubble;
    [HideInInspector]
    public Canvas DialogBox;

    public string targetPrefabString;
    public Vector2 dialogBoxPivot;
    public int dialogTexts;

    public Animation ActorAnimator;

    private float Duration;
    private float timePerWord;

    private string[] words;
    private int wordCount;

    // Use this for initialization
    void Start()
    {
        thisTransform = this.transform;
    }

    void Update()
    {
        if (DialogBox)
        {
            DialogBox.transform.position = thisTransform.position + new Vector3(0, 0.2f, 0);

            DialogBox.transform.LookAt(DialogBox.transform.position + GlobalData.CameraControl.thisTransform.rotation * (Vector3.forward), GlobalData.CameraControl.thisTransform.rotation * Vector3.up);
        }
    }

    public void InitDialogBox(DialogBoxType boxType)
    {
        int dialogTextCount = 0;
        GameObject targetPrefab = new GameObject();

        targetPrefab = Resources.Load(targetPrefabString) as GameObject;

        if (thisTransform == null)
            thisTransform = this.transform;

        var createdPrefab = (GameObject)Instantiate(targetPrefab, thisTransform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);

        DialogText = new Text[dialogTexts];

        DialogBox = createdPrefab.GetComponent<Canvas>();
        DialogBox.transform.position = thisTransform.position + new Vector3(0, 1.5f, 0);

        Transform[] allChildren = DialogBox.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            switch (child.gameObject.name)
            {
                case "DialogText":
                    DialogText[dialogTextCount] = child.GetComponent<Text>();
                    dialogTextCount++;
                    break;
                case "BackGroundImage":
                    DialogBubble = child.GetComponent<Image>();
                    break;
                default:
                    break;
            }
        }
        DialogBox.GetComponent<RectTransform>().pivot = dialogBoxPivot;
    }

    public string reSharpString(string line, DialogBoxType boxType)
    {
        var charsAllowedPerLine = 0;
        var linesAllowed = 0;
        var charsAllowed = 0;

        switch (boxType)
        {
            case DialogBoxType.RightSideSentence:
                charsAllowedPerLine = 0;
                linesAllowed = 0;
                targetPrefabString = "Prefabs/UI/RightSideSentence";
                dialogBoxPivot = new Vector2(1, 0);
                dialogTexts = 1;
                break;
            case DialogBoxType.LeftSide_1SmallSentence:
                charsAllowedPerLine = 7;
                linesAllowed = 5;
                charsAllowed = charsAllowedPerLine * linesAllowed;
                targetPrefabString = "Prefabs/UI/LeftSide_1SmallSentence";
                dialogBoxPivot = new Vector2(0, 0);
                dialogTexts = 1;
                break;
            case DialogBoxType.LeftSide_1MediumSentence:
                charsAllowedPerLine = 9;
                linesAllowed = 5;
                charsAllowed = charsAllowedPerLine * linesAllowed;
                targetPrefabString = "Prefabs/UI/LeftSide_1MediumSentence";
                dialogBoxPivot = new Vector2(0, 0);
                dialogTexts = 1;
                break;
            case DialogBoxType.LeftSide_1LargeSentence:
                charsAllowedPerLine = 16;
                linesAllowed = 8;
                charsAllowed = charsAllowedPerLine * linesAllowed;
                targetPrefabString = "Prefabs/UI/LeftSide_1LargeSentence";
                dialogBoxPivot = new Vector2(0, 0);
                dialogTexts = 1;
                break;
            case DialogBoxType.LeftSide_2Sentence:
                charsAllowedPerLine = 0; //12
                linesAllowed = 5;
                charsAllowed = charsAllowedPerLine * linesAllowed;
                targetPrefabString = "Prefabs/UI/LeftSide_2Sentence";
                dialogBoxPivot = new Vector2(0, 0);
                dialogTexts = 2;
                break;
            default:
                break;
        }
        // just so we can break when we dont care
        if (charsAllowedPerLine == 0)
            return line;

        var charsCount = line.Length;
        // find out on how many lines the ActorLine spans;
        var lineSpan = charsCount / charsAllowedPerLine;
        var remainder = charsCount % charsAllowedPerLine;
        if (remainder > 0)
            lineSpan++;
        remainder = 0;

        if (line.Contains("..."))
        {
            lineSpan++;
            line = line.Replace("...", "\n...");
        }

        var linesRemaining = linesAllowed - lineSpan;
        if (linesRemaining > 0)
        {
            var linesForStart = linesRemaining / 2;
            remainder = linesRemaining % 2;
            if (remainder > 0)
                linesForStart++;

            var linesForEnd = linesRemaining - linesForStart;

            var i = 0;
            for (i = 0; i < linesForStart; i++)
            {
                line = "\n" + line;
            }
            i = 0;
            for (i = 0; i < linesForEnd; i++)
            {
                line = line + "\n";
            }
        }
        return line;
    }

    public void DestroyDialogBox()
    {
        Destroy(DialogBox.gameObject);
    }

    public void SayLine(string line, DialogBoxType boxType, float duration)
    {
        Duration = duration;

        //words = new string[100];
        words = line.Split(' ');

        wordCount = words.Count();

        timePerWord = 0.23f;
        if (timePerWord * wordCount > (Duration + (Duration / 3)))
        {
            timePerWord = Duration / (float)wordCount;
        }

        line = reSharpString(line, boxType);

        InitDialogBox(boxType);

        //Debug.Log("timePerWord = " + timePerWord + "; \n"
        //    + "Duration = " + Duration + "; \n"
        //    + "wordCount = " + wordCount + "; \n"
        //    + "line = ' " + line + "'; \n");

        DialogTextIndex = 0;
        if (DialogText.Length > 1)
        {
            string[] array_postBack_dialog = new string[line.Length];
            array_postBack_dialog = line.Split('\n');

            for (var i = 0; i < DialogText.Length; i++)
            {
                DialogText[i].color = new Color32(81, 81, 81, 255);
                DialogText[i].lineSpacing = 0.88f;
                DialogText[i].text = array_postBack_dialog[i];
            }
        }
        else
        {
            DialogText[0].color = new Color32(81, 81, 81, 255);
            DialogText[0].lineSpacing = 0.88f;
            DialogText[0].text = line;
        }

        StartCoroutine(CloseDialogBox());
        StartCoroutine(UpdateTextByWord());
    }

    public IEnumerator UpdateTextByWord()
    {
        string newWord = "";
        foreach (var word in words)
        {
            newWord = "<color=DBDBDB>" + word + "</color>";

            DialogText[DialogTextIndex].text = DialogText[DialogTextIndex].text.Replace(word, newWord);

            if (DialogText.Length > 1)
            {
                if (word == "\n")
                    DialogTextIndex++;
            }

            if (word.Contains("."))
                yield return new WaitForSeconds(timePerWord + 0.44f);   // hard coded
            if (word.Contains(","))
                yield return new WaitForSeconds(timePerWord + 4.44f);   // hard coded
            else
                yield return new WaitForSeconds(timePerWord);
        }
    }

    public IEnumerator CloseDialogBox()
    {
        yield return new WaitForSeconds(Duration + (Duration / 3));
        DestroyDialogBox();
    }
}