using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public bool isChoosing;
    public Image speaker;
    public GameObject buttonStart;
    public string[] names;
    public Sprite[] bg;
    public Image nowbg;
    public Image nextbg;
    public GameObject[] panels;
    public AudioClip[] audios;
    public AudioSource source;
    public float fadespeed = 1;
    public float bgfadespeed = 1;
    public float baseTypeSpeed;
    public TextMeshProUGUI speechTextBox;
    public TextMeshProUGUI speechTextBoxSample;
    public TextMeshProUGUI namesTextBox;
    float typeSpeed;
    string fullText = "";
    public string[] nowText;
    public RectTransform container;
    int count = 0;
    int spritecount;
    bool end, choose, endoftext, fade, fadebg;
    public Color charColor;
    public Color bgColor;
    public Color onChooseColor;
    public int[] charqueue;
    GameObject activeChar;

    [SerializeField] GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    [SerializeField] EventSystem m_EventSystem;
    [SerializeField] RectTransform canvasRect;

    private void Start()
    {
        charColor = speaker.color;
        bgColor = nextbg.color;
        if (audios.Length > 0)
        {
          source.clip = audios[count];
          source.Play();
        }
        typeSpeed = baseTypeSpeed;
        namesTextBox.text = names[charqueue[count]];
        fullText = nowText[count];
        speechTextBox.fontSize = speechTextBoxSample.fontSize;
        StartCoroutine(TypeText(fullText));
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpdatePortrait();
        }
        if (fadebg)
        {
            bgColor.a += Time.deltaTime*fadespeed;
            nextbg.color = bgColor;
            if (bgColor.a >= 1)
            {
                fadebg = false;
                nowbg.sprite = nextbg.sprite;
                bgColor.a = 0;
                nextbg.color = bgColor;
                spritecount++;
            }
        }
        if (fade)
        {
            charColor.a -= Time.deltaTime * bgfadespeed;
            speaker.color = charColor;
            if (charColor.a <= 0)
            {
                charColor.a = 0;
                UpdateText();
            }
        }
    }


    private IEnumerator TypeText(string fullText)
    {
        foreach (char letter in fullText.ToCharArray())
        {
            speechTextBox.text += letter;             
            yield return new WaitForSeconds(typeSpeed);
        }
        end = true;
    } 
    
    public void ShowFullText()
    {
        if (!endoftext)
        {
            StopAllCoroutines();
            end = true;
            speechTextBox.text = fullText;
        }
        return;
    }

  public void UpdateText()
  {
    fade = false;
    if (charqueue[count] > 100)
    {
      charqueue[count] -= 100;
      if (spritecount < bg.Length)
      {
        nextbg.sprite = bg[spritecount];
        fadebg = true;
      }
    }
    if (speaker.GetComponent<CharImage>().SetActivePlayerPortrait(charqueue[count]))
      charColor.a = 1;
    namesTextBox.text = names[charqueue[count]];
    speaker.color = charColor;
    if (audios.Length > 0)
    {
      source.clip = audios[count];
      source.Play();
    }
    speechTextBox.text = "";
    fullText = nowText[count];

    StartCoroutine(TypeText(fullText));
  }

    public void UpdatePortrait()
    {
        if (!end)
        {
            ShowFullText();
            return;
        }
        count++;
        if (count >= nowText.Length)
        {
            StopAllCoroutines();
            endoftext = true;
            ShowPanels();
            return;
        }
        end = false;
        fade = true;
    }
   
    void ShowPanels()
    {
        if (isChoosing)
        {
            choose = true;
            panels[0].SetActive(false);
            speaker.gameObject.SetActive(false);
            panels[1].SetActive(true);
            isChoosing = false;
        }
        else
        {
            Debug.Log("End");
            buttonStart.SetActive(true);
        }
    }

    public void Choose(int i)
    {
        
    }

    public void Choose()
    {
        //send on nextscene
    }
}