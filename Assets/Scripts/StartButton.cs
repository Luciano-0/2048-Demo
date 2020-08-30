using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

    public Animation cameraAnimation;
    public Animation PanelAnimation;
    public Animation PlaneAnimation;
    public GameObject BackGround;
    public Base baseData;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Click()
    {
        cameraAnimation.Play("StartCameraAnimation");
        PanelAnimation.Play("StartPanelAnimation");
        PlaneAnimation.Play("FastRotate");
        StartCoroutine(Delay());

    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.8f);
        for (int i = 0; i < 16; i++)
        {
            baseData.lattices[i / 4, i % 4].Chess.MyAnimation.Play("ChessDestory");
        }
    }

    public void RestartClick()
    {
        baseData.BackGroundM();
        for (int i = 0; i < 16; i++)
        {
            baseData.lattices[i / 4, i % 4].Chess.MyAnimation.Play("ChessDestory");
        }
        baseData.Restart();
        
    }


}
