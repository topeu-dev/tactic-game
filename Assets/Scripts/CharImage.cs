using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class CharImage : MonoBehaviour
{
    public Sprite[] baseImage;
    public Sprite[] chooseImage;
    Color c;
    int activeChar; 
    bool fade;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        c = GetComponent<Image>().color;
        c.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SetActiveChar(Color c, int character)
    {
        GetComponent<Image>().sprite = chooseImage[character];
        GetComponent<Image>().color = c;
    }
    public void DeactivateChar(Color c, int character)
    {
        GetComponent<Image>().sprite = baseImage[character];
        GetComponent<Image>().color = c;
    }
    public bool SetActivePlayerPortrait(int character)
    {
        activeChar = character;
        if (activeChar == 3)
            return false;
        GetComponent<Image>().sprite = baseImage[activeChar];
        return true;
    }
}
