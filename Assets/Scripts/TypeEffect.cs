using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TypeEffect : MonoBehaviour
{
    public Image fader;
    public int nextScene = 1;
    public bool isChoosing;
    public Image speaker;
    public TextMeshProUGUI debuffTextBox;
    public GameObject buttonStart;
    public string[] names;
    public Sprite[] bg;
    public Image nowbg;
    public Image nextbg;
    public GameObject[] panels;
    public AudioClip[] audios;
    public AudioClip[] audiosByChoose;
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
    public string[] debuffText;
    int count = 0;
    int spritecount;
    bool end, choose, endoftext, fade, fadebg, fadeui;
    public Color charColor;
    public Color bgColor;
    public Color faderColor;
    public int[] charqueue;
    string debuff;
    int itemChoose;


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
        
        if (Input.GetKeyDown(KeyCode.Space) && !fade)
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
        if (fadeui)
        {
            faderColor.a += Time.deltaTime * bgfadespeed;
            fader.color = faderColor;
            if (faderColor.a >= 1)
            {
                fadeui = false;
                StartNewScene(nextScene);
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
    if (nowText[count] == "")
    {
      ShowPanels();
      return;
    }
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
            source.Stop();
            choose = true;
            panels[0].SetActive(false);
            speaker.gameObject.SetActive(false);
            panels[1].SetActive(true);
            isChoosing = false;
        }
        else
        {
            source.Stop();
            Debug.Log("End");
            buttonStart.SetActive(true);
            if (debuffTextBox is not null)
            {
              panels[0].SetActive(false);
              speaker.gameObject.SetActive(false);
              debuffTextBox.text = debuff;
            }
        }
    }

    public void Choose(int i)
  {
    itemChoose = i;
  }
    public void Choose(string text)
  {
        count++;
        debuff = debuffText[itemChoose];
        audios[count] = audiosByChoose[itemChoose];
        nowText[count] = text; 
        panels[0].SetActive(true);
        speaker.gameObject.SetActive(true);
        panels[1].SetActive(false);
        UpdateText();    
  }

  public void Choose()
    {
        fadeui = true;
    }

    public void StartNewScene(int i)
    {
    SceneManager.LoadSceneAsync(i);
    }
}