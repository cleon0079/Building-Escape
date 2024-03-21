using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{   
    public NumBox boxPrefab;

    public NumBox[,] boxes= new NumBox[4,4];

    public Sprite[] sprites;



    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void Init()
    {   
        int n=0;
        for(int y =3; y>= 0; y--){
            for(int x=0; x<4; x++){
                NumBox box =Instantiate(boxPrefab, new Vector2(x,y),Quaternion.identity);
                box.Init(x,y, n+1, sprites[n],ClickToSwap);
                boxes[x,y] = box;
                n++;
            }
        }
    }

    void ClickToSwap(int x, int y){
        int dx =getDx(x,  y);
        int dy =getDy(x,  y);

        var SelectTarget = boxes[x,y];
        var chargeTarget = boxes[x+dx,y+dy];

        //swap this 2 boxes
        boxes[x,y] = chargeTarget;
        boxes[x+dx,y+dy] = SelectTarget;

        SelectTarget.UpdatePos(x+ dx,y+dy);
        chargeTarget.UpdatePos(x,y);
    }
    
    //check is oudside the
    int getDx(int x, int y){
        //check right side is empty
        if(x< 3 && boxes[x +1,y].IsEmpty() ){
            return 1;
        }
        //check Left side is empty
        if(x>0 && boxes[x -1,y].IsEmpty() ){
            return -1;
        }
        else{
            return 0;
        }
    }

    int getDy(int x, int y){
        //check top side is empty
        if(y< 3 && boxes[x,y +1].IsEmpty() ){
            return 1;
        }
        //check bottom side is empty
        if(y>0 && boxes[x,y -1].IsEmpty() ){
            return -1;
        }
        else{
            return 0;
        }
    }
}
