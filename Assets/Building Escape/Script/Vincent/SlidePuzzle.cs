using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePuzzle : MonoBehaviour{   
    public NumBox boxPrefab;

    public NumBox[,] boxes= new NumBox[4,4];

    public Sprite[] sprites;

    private int emptyIndex = 16;

    public float distance =5f;  // distance between camera and puzzle
    [SerializeField] private Camera LockCamera; // Reference to the camera
    [SerializeField] private GameObject Doors; 

    public float moveSpeed = 2f;

    private bool ShuffleComplete = false;

    private bool puzzleComplete = false;
    private bool doorFullyOpened =false;

    // Start is called before the first frame update
    void Start(){
        Init();
        Shuffle();
    }

    void Update (){
        openDoor();
    }


    void Init(){
        
        //get position in fornt of the camera
        Vector3 spawnPosition = LockCamera.transform.position + LockCamera.transform.forward  * distance;
        
        // Instantiate the puzzle in front of the camera
        GameObject puzzleObject = new GameObject("Puzzle");
        puzzleObject.transform.position = spawnPosition;

        //center the puzzle
        float puzzleWidth =4;
        float puzzleHeight =4;
        float cellSize  =1f;

        Vector3 puzzleCenter = new Vector3((puzzleWidth - 1) * cellSize / 2f, (puzzleHeight - 1) * cellSize / 2f, 0f);

        // Offset the puzzle object to center it in the camera view
        puzzleObject.transform.position -= puzzleCenter;

        int n = 0;
        for (int y = 3; y >= 0; y--){
            for (int x = 0; x < 4; x++){
                NumBox box = Instantiate(boxPrefab, new Vector2(x, y), Quaternion.identity,puzzleObject.transform);
                box.Init(x, y, n + 1, sprites[n], ClickToSwap);
                boxes[x, y] = box;
                n++;
            }
        }
    }

    void ClickToSwap(int x, int y){
        int dx = GetDx(x, y);
        int dy = GetDy(x, y);

        var selectTarget = boxes[x, y];
        var chargeTarget = boxes[x + dx, y + dy];

        // Swap the boxes
        boxes[x, y] = chargeTarget;
        boxes[x + dx, y + dy] = selectTarget;

        selectTarget.UpdatePos(x + dx, y + dy);
        chargeTarget.UpdatePos(x, y);

        if(ShuffleComplete == true){
            CheckComplete(); // Check for completion
        }
         
    }

    int GetDx(int x, int y){
        if (x < 3 && boxes[x + 1, y].IsEmpty()){
            return 1;
        }
        if (x > 0 && boxes[x - 1, y].IsEmpty()){
            return -1;
        }
        else{
            return 0;
        }
    }

    int GetDy(int x, int y){
        if (y < 3 && boxes[x, y + 1].IsEmpty()){
            return 1;
        }
        if (y > 0 && boxes[x, y - 1].IsEmpty()){
            return -1;
        }
        else{
            return 0;
        }
    }

    // Shuffle the puzzle
    void Shuffle(){
        for (int i = 0; i < 100; i++){
            int randomX = Random.Range(0, 4);
            int randomY = Random.Range(0, 4);
            if (!boxes[randomX, randomY].IsEmpty()){
                ClickToSwap(randomX, randomY);
            }
        }
        
        //Debug.Log("Puzzle is Shuffled");
        ShuffleComplete = true;
    }

    // Check if the puzzle is completed
    void CheckComplete(){
        for (int y = 0; y < 4; y++){
            for (int x = 0; x < 4; x++){
                if (boxes[x, y].originalX != x || boxes[x, y].originalY != y){
                    //Debug.Log("Puzzle is not complete yet!");
                    return;
                }
            }
        }
        Debug.Log("Puzzle is complete!"); 
        puzzleComplete =true;
    }

    private void openDoor(){
        if(puzzleComplete == true){
            //check door is fully open or not
            if(doorFullyOpened == false){
                Doors.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
                if( Doors.transform.position.z <=11.69){
                    doorFullyOpened = true;
                }
            }
        }
    }
}
