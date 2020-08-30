using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lattice {

    public Chess Chess { get; set; }
    public bool Exist { get; set; }
    public int Number { get; set; }
    public Vector3 Position { get; set; }

    public int Level { get; set; }


    public void SetToMin(Material material)
    {
        Chess.ChessGameObject.SetActive(true);
        Exist = true;
        Level = 1;
        Number = 2;       
        Chess.MyMaterial.material = material;
        //chess.MyMaterial.color = new Color(0.3f, 0.5f, 1);
        Chess.ChessGameObject.transform.localPosition = Position;
        Chess.CObject.transform.localScale = new Vector3(73, 73, 5);
        Chess.ChessGameObject.transform.localScale = new Vector3(1, 1, 1);
        Chess.MyAnimation.Play("ChessCreate");
        Base.count++;
    }

}
