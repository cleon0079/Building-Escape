using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterials : MonoBehaviour
{   
    [SerializeField] private AudioClip finishSound;
    private AudioSource audioSource;
    [SerializeField] private Material finishMaterial;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        BlackBoardPuzzleCheck blackBroad = FindObjectOfType<BlackBoardPuzzleCheck>();
        if(blackBroad.isFinish)
        {
            GameObject[] blackBroads = GameObject.FindGameObjectsWithTag("BlackBoard");
            foreach(GameObject obj in blackBroads)
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                renderer.material = finishMaterial;
            }
            if(!audioSource.isPlaying){
                audioSource.PlayOneShot(finishSound);
            }
                
            
        }

    }
}
