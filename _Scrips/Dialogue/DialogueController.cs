using UnityEngine;
using TMPro;
using UnityEditor.Search;
using System.Collections.Generic;
using System.Collections;
public class DialogueController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI NPCNameText;
    [SerializeField] private TextMeshProUGUI NPCDialogueText;
    [SerializeField] private float typeSpeed = 10f;
    private Queue<string> paragraphs = new Queue<string>();

    private bool conversationEnded;
    private bool isTyping;
    private Coroutine typeDialogueCoroutine;
    private const string HTML_ALPHA = "<color=#00000000>";
    private string p;

    public float MAX_TYPE_TIME = 0.1f;

    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        if (paragraphs.Count == 0)
        {
            if (!conversationEnded)
            {
                // bắt đầu nói;
                StartConversation(dialogueText);
            }
            else if (conversationEnded && !isTyping)
            {
                // kết thúc nói;
                EndConversation();
                return;
            }
        }

        if (!isTyping)
        {
            p = paragraphs.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(p));
        }

        else
        {
            FinishDialogueEarly();
        }

        // cập nhật danh sách queue nói
        //NPCDialogueText.text = p;

        if (paragraphs.Count == 0)
        {
            // đánh dấu kết thúc trò chuyện, đã thực hiện hết trong queue
            conversationEnded = true;
        }
    }

    private void StartConversation(DialogueText dialogueText)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        NPCNameText.text = dialogueText.NPCName;

        // thêm dialogueText vào queue;

        for (int i = 0; i < dialogueText.paragraps.Length; i++)
        {
            paragraphs.Enqueue(dialogueText.paragraps[i]);
        }
    }

    private void EndConversation()
    {
        conversationEnded = false;

        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        NPCDialogueText.text = "";

        string originalText = p;
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            alphaIndex++;
            NPCDialogueText.text = originalText;

            displayedText = NPCDialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            NPCDialogueText.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }


    private void FinishDialogueEarly()
    {
        StopCoroutine(typeDialogueCoroutine);

        NPCDialogueText.text = p;

        isTyping = false;
    }
}
