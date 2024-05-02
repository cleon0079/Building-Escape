using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumLock : MonoBehaviour
{
    GameManager gm;

    private bool right = false;
    private int id = 0;
    [SerializeField] Button[] buttons;
    [SerializeField] GameObject num;
    [SerializeField] GameObject imagePrefab;
    [SerializeField] Button check;
    [SerializeField] Button delete;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        setButton();
        delete.onClick.AddListener(() => { Delete(); });
        check.onClick.AddListener(() => { Check(); });
    }

    void setButton() {
        for (int i = 0; i < buttons.Length; i++)
        {
            Sprite buttonSprite = buttons[i].GetComponent<Image>().sprite;
            buttons[i].onClick.AddListener(() => { showNumber(buttonSprite); });
        }
    }

    void showNumber(Sprite sp) {
        GameObject newImage = Instantiate(imagePrefab);
        newImage.transform.SetParent(num.transform);
        Image image = newImage.GetComponent<Image>();
        image.sprite = sp;
        id++;
    }

    // Update is called once per frame
    void Update()
    {
        if (id == 4) {
            foreach (Button button in buttons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        if (num.transform.childCount >= 5) {
            for (int i = 0; i < num.transform.childCount; i++)
            {
                Destroy(num.transform.GetChild(i).gameObject);
            }
        }
    }

    void Delete() {
        if ( id <= 3 && id >0)
        {
            Destroy(num.transform.GetChild(id - 1).gameObject);
            id--;
            Debug.Log(id);
        }
        else if (id == 4) {
            Destroy(num.transform.GetChild(id - 1).gameObject);
            id--;
            setButton();
            Debug.Log(id);
        }

    }

    void Check() {
        if (id <= 3)
        {
            for (int i = 0; i < num.transform.childCount; i++)
            {
                Destroy(num.transform.GetChild(i).gameObject);
                id = 0;
            }
        }

        if (id == 4)
        {
            int k = 0;
            for (int i = 0; i < num.transform.childCount; i++)
            {
                Sprite checkImg = num.transform.GetChild(i).gameObject.GetComponent<Image>().sprite;
                if (i == 0)
                {
                    k = 1;
                }
                else if (i == 1) 
                {
                    k = 4;
                }
                else if (i == 2)
                {
                    k = 6;
                }
                else
                {
                    k = 8;
                }
                Sprite buttonImg = buttons[k].GetComponent<Image>().sprite;

                if (checkImg == buttonImg)
                {
                    right = true;
                }
                else
                {
                    right = false;
                    break;
                }
            }
        }

        if (right)
        {
            for (int i = 0; i < num.transform.childCount; i++)
            {
                Destroy(num.transform.GetChild(i).gameObject);
                id = 0;
            }
            right = false;
            setButton();
            gm.OpenGlassDoor();


        }
    }
}
