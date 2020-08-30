using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour {

    public Base control;
    public Animation scoreLabel;
    public Animation score;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateNewGame()
    {
        control.CreateNewGame();
        Base.state = Base.State.PLAYING;
        score.Play("ScoreStart");
        scoreLabel.Play("ScoreLabelStart");
    }

}
