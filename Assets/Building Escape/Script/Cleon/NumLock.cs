using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumLock : MonoBehaviour
{
    private bool right = false;
    private int id = 0;
    [SerializeField] Button[] buttons;
    [SerializeField] GameObject num;
    [SerializeField] GameObject imagePrefab;
    [SerializeField] Button check;
    [SerializeField] Button delete;

    [SerializeField] GameObject tmpGo;
    [SerializeField] TMP_Text pro;

    // Start is called before the first frame update
    void Start()
    {
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
                tmpGo.SetActive(true);
                pro.text = "Wrong";
                id = 0;
            }
        }

        if (id == 4)
        {
            for (int i = 0; i < num.transform.childCount; i++)
            {
                Sprite checkImg = num.transform.GetChild(i).gameObject.GetComponent<Image>().sprite;
                Sprite buttonImg = buttons[i].GetComponent<Image>().sprite;

                if (checkImg == buttonImg)
                {
                    right = true;
                }
            }
        }

        if (right)
        {
            tmpGo.SetActive(true);
            pro.text = "Right";
            for (int i = 0; i < num.transform.childCount; i++)
            {
                Destroy(num.transform.GetChild(i).gameObject);
                id = 0;
            }
            right = false;
            setButton();
        }
    }
}
