using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Base : MonoBehaviour
{
    public enum State
    {
        START,
        PLAYING,
        MOVING,
        GAMEOVER
    }
    public static State state = State.START;
    public List<Vector3> chessPosition = new List<Vector3>(16);
    public GameObject[] chesses = new GameObject[16];
    public Material[] materials = new Material[12];
    public Lattice[,] lattices = new Lattice[4, 4];
    public Animation planeScaleAnimation;
    public UILabel ScorePanel;
    public Animation ScorePanelAnimation;
    public Animation StartChessAnimation;
    public Animation ScoreLabelAnimation;
    public Animation BackGroundColor;
    public GameObject BackGround;
    
    public static int count = 0;
    public static int score = 0;
    public GameObject GameOverPanel;
    public GameObject RestartButton;

    private void Awake()
    {
        Screen.SetResolution(600, 960, false);
    }

    // Use this for initialization
    void Start()
    {
        SetChessPosition();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                lattices[i, j] = new Lattice();
                lattices[i, j].Position = chessPosition[i * 4 + j];
                lattices[i, j].Chess = new Chess();
                lattices[i, j].Chess.ChessGameObject = chesses[i * 4 + j];
                lattices[i, j].Chess.CObject = chesses[i*4+j].transform.Find("GameObject").gameObject;
                lattices[i, j].Chess.MyMaterial = lattices[i, j].Chess.CObject.transform.GetComponent<Renderer>();
                lattices[i, j].Chess.MyAnimation = lattices[i, j].Chess.CObject.transform.GetComponent<Animation>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.PLAYING)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (CanMoveDown() == true)
                {
                    MoveDown();
                    CreateNewChess();
                    if (IsGameOver() == true)
                    {
                        GameOver();
                    }
                }
                else
                {
                    planeScaleAnimation.Play("PlaneScaleAnimation");
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (CanMoveUp() == true)
                {
                    MoveUp();
                    CreateNewChess();
                    if (IsGameOver() == true)
                    {
                        GameOver();
                    }
                }
                else
                {
                    planeScaleAnimation.Play("PlaneScaleAnimation");

                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (CanMoveLeft() == true)
                {
                    MoveLeft();
                    CreateNewChess();
                    if (IsGameOver() == true)
                    {
                        GameOver();
                    }
                }
                else
                {
                    planeScaleAnimation.Play("PlaneScaleAnimation");
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if(CanMoveRight() == true)
                {
                    MoveRight();
                    CreateNewChess();
                    if (IsGameOver() == true)
                    {
                        GameOver();
                    }
                }
                else
                {
                    planeScaleAnimation.Play("PlaneScaleAnimation");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameOver();
            }
        }
    }

    private void LateUpdate()
    {

    }

    IEnumerator BackGroundMove()
    {
        Debug.Log("1");
        for(int i = 0; i < 10; i++)
        {
            BackGround.transform.Translate(Vector3.up * 20 * Time.deltaTime);
            yield return 0;
        }
        StopCoroutine(BackGroundMove());
    }

    bool IsGameOver()
    {
        if(CanMoveDown()==false&&CanMoveLeft()==false&&CanMoveRight()==false&&CanMoveUp()==false)
        {
            return true;
        }
        return false;
    }

    void GameOver()
    {
        state = State.GAMEOVER;
        planeScaleAnimation.Play("GameOverPlane");
        ScoreLabelAnimation.Play("ScoreLabel");
        ScorePanelAnimation.Play("ScoreGameOver",PlayMode.StopAll);
        RestartButton.SetActive(true);
    }
    public void Restart()
    {

        planeScaleAnimation.Play("RestartPlane");
        ScoreLabelAnimation.Play("RestartScoreLabel",PlayMode.StopAll);
        ScorePanelAnimation.PlayQueued("RestartScore",QueueMode.CompleteOthers);
        RestartButton.SetActive(false);
        CreateNewGame();
        state = State.PLAYING;
    }

    void SetChessPosition()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                chessPosition.Add(new Vector3(-126.9f + i * 42.3f * 2f, 126.9f - j * 42.3f * 2f, -5));
            }
        }
    }

    public void CreateNewGame()
    {
        StartChessAnimation.Stop();
        System.Random ran = new System.Random();
        int n1 = ran.Next(0, 15);
        int n2 = ran.Next(0, 15);
        while (n2 == n1)
        {
            n2 = ran.Next(0, 15);
        }
        for (int i = 0; i < 16; i++)
        {
            lattices[i / 4, i % 4].Exist = false;
            lattices[i / 4, i % 4].Number = 0;
            lattices[i / 4, i % 4].Level = 0;
            lattices[i / 4, i % 4].Chess.ChessGameObject.transform.localScale=new Vector3(1,1,1);
            lattices[i / 4, i % 4].Chess.ChessGameObject.transform.localPosition = lattices[i / 4, i % 4].Position;
            lattices[i / 4, i % 4].Chess.CObject.transform.localScale = new Vector3(73, 73, 5);
            lattices[i / 4, i % 4].Chess.ChessGameObject.SetActive(false);
        }
        lattices[n1 / 4, n1 % 4].SetToMin(materials[0]);
        lattices[n2 / 4, n2 % 4].SetToMin(materials[0]);
        score = 4;
        UpdateScore();
    }

    public void CreateNewChess()
    {
        System.Random ran = new System.Random();
        int n;
        do
        {           
            n = ran.Next(0, 15);
        } while (lattices[n/4,n%4].Exist);
        lattices[n / 4, n % 4].SetToMin(materials[0]);
        score += 2;
        UpdateScore();
    }

    public bool CanMoveRight()
    {
        for (int i = 3; i >= 0; i--)
        {
            for (int j = 2; j >= 0; j--)
            {
                if (lattices[i, j].Exist == true)
                {
                    if (lattices[i, j + 1].Exist == false || lattices[i, j + 1].Number == lattices[i, j].Number)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool CanMoveLeft()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 1; j < 4; j++)
            {
                if (lattices[i, j].Exist == true)
                {
                    if (lattices[i, j - 1].Exist == false || lattices[i, j].Number == lattices[i, j - 1].Number)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool CanMoveUp()
    {
        for (int i = 1; i < 4; i++)
        {
            for (int j = 3; j >= 0; j--)
            {
                if (lattices[i, j].Exist == true)
                {
                    if (lattices[i - 1, j].Exist == false || lattices[i, j].Number == lattices[i - 1, j].Number)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool CanMoveDown()
    {
        for (int i = 2; i >= 0; i--)
        {
            for (int j = 3; j >= 0; j--)
            {
                if (lattices[i, j].Exist == true)
                {
                    if (lattices[i + 1, j].Exist == false || lattices[i, j].Number == lattices[i + 1, j].Number)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void MoveRight()
    {
        int end = 3;
        for (int i = 3; i >= 0; i--)
        {
            end = 3;
            for (int j = 3; j >= 0; j--)
            {
                if (lattices[i, j].Exist == true)
                {
                    for (int k = end; k > j; k--)
                    {
                        if (lattices[i, k].Exist == false && NoBlockHorizontal(i, j, k))
                        {
                            UpdateLatticeValue(lattices[i, k], lattices[i, j], 0);
                            StartCoroutine(Move(lattices[i,j],lattices[i,k],0));
                            break;
                        }
                        else if (lattices[i, k].Number == lattices[i, j].Number && NoBlockHorizontal(i, j, k))
                        {
                            end = k - 1;
                            UpdateLatticeValue(lattices[i, k], lattices[i, j], 1);
                            StartCoroutine(Move(lattices[i, j], lattices[i, k],1));
                            break;
                        }
                    }
                }
            }
        }
    }

    void MoveLeft()
    {
        int end = 0;
        for (int i = 3; i >= 0; i--)
        {
            end = 0;
            for (int j = 0; j <= 3; j++)
            {
                if (lattices[i, j].Exist == true)
                {
                    for (int k = end; k < j; k++)
                    {
                        if (lattices[i, k].Exist == false && NoBlockHorizontal(i, k, j))
                        {
                            UpdateLatticeValue(lattices[i, k], lattices[i, j], 0);
                            StartCoroutine(Move(lattices[i, j], lattices[i, k], 0));

                            break;
                        }
                        else if (lattices[i, k].Number == lattices[i, j].Number && NoBlockHorizontal(i, k, j))
                        {
                            end = k+1;
                            UpdateLatticeValue(lattices[i, k], lattices[i, j], 1);
                            StartCoroutine(Move(lattices[i, j], lattices[i, k], 1));
                            break;
                        }
                    }
                }
            }
        }
        
    }

    void MoveUp()
    {
        int end = 0;
        for (int j = 3; j >= 0; j--)
        {
            end = 0;
            for (int i = 0; i <= 3; i++)
            {
                if (lattices[i, j].Exist == true)
                {
                    for (int k = end; k < i; k++)
                    {
                        if (lattices[k, j].Exist == false && NoBlockVertical(j, k, i))
                        {
                            UpdateLatticeValue(lattices[k, j], lattices[i, j], 0);
                            StartCoroutine(Move(lattices[i, j], lattices[k, j], 0));
                            break;
                        }
                        else if (lattices[k, j].Number == lattices[i, j].Number && NoBlockVertical(j, k, i))
                        {
                            end = k + 1;
                            UpdateLatticeValue(lattices[k, j], lattices[i, j], 1);
                            StartCoroutine(Move(lattices[i, j], lattices[k, j], 1));
                            break;
                        }
                    }
                }
            }
        }
    }

    void MoveDown()
    {
        int end = 3;
        for (int j = 3; j >= 0; j--)
        {
            end = 3;
            for (int i = 3; i >= 0; i--)
            {
                if (lattices[i, j].Exist == true)
                {
                    for (int k = end; k > i; k--)
                    {
                        if (lattices[k, j].Exist == false && NoBlockVertical(j, i, k))
                        {
                            UpdateLatticeValue(lattices[k, j], lattices[i, j], 0);
                            StartCoroutine(Move(lattices[i, j], lattices[k, j], 0));
                            break;
                        }
                        else if (lattices[k, j].Number == lattices[i, j].Number && NoBlockVertical(j, i, k))
                        {
                            end = k - 1;
                            UpdateLatticeValue(lattices[k, j], lattices[i, j], 1);
                            StartCoroutine(Move(lattices[i, j], lattices[k, j], 1));
                            break;
                        }
                    }
                }
            }
        }
    }

    IEnumerator Move(Lattice startLat,Lattice endLat,int flag)
    {
        Vector3 start = startLat.Position;
        Vector3 end = endLat.Position;
        Vector3 speed = (end - start) / 5;
        for (int i = 0; i < 5; i++)
        {
            startLat.Chess.ChessGameObject.transform.Translate(speed,Space.Self);
            yield return 0;
        }
        startLat.Chess.ChessGameObject.transform.localPosition = start;
        if (flag == 0)
        {
            UpdateLattice(endLat);
            UpdateLattice(startLat);
        }
        else
        {
            UpdateLattice(endLat);
            UpdateLattice(startLat);
            endLat.Chess.MyAnimation.Play("ChessScale");
        }

        StopCoroutine("Move");
    }

    bool NoBlockHorizontal(int row, int col1, int col2)
    {
        for (int i = col1 + 1; i < col2; i++)
        {
            if (lattices[row, i].Exist == true)
            {
                return false;
            }
        }
        return true;
    }

    bool NoBlockVertical(int col,int row1,int row2)
    {
        for(int i = row1 + 1; i < row2; i++)
        {
            if (lattices[i, col].Exist == true)
            {
                return false;
            }
        }
        return true;
    }

    void UpdateLatticeValue(Lattice lat1,Lattice lat2,int flag)
    {
        if (flag == 0)
        {
            lat1.Exist = true;
            lat1.Number = lat2.Number;
            lat1.Level = lat2.Level;
            lat2.Exist = false;
            lat2.Number = 0;
            lat2.Level = 0;
        }
        else
        {
            lat1.Exist = true;
            lat1.Number = lat1.Number * 2;
            lat1.Level = lat1.Level + 1;
            lat2.Exist = false;
            lat2.Number = 0;
            lat2.Level = 0;
        }

    }

    void UpdateLattice(Lattice lat)
    {
        if (lat.Exist== true)
        {
            lat.Chess.ChessGameObject.SetActive(true);
            lat.Chess.ChessGameObject.transform.localScale = new Vector3(1, 1, lat.Level);
            lat.Chess.MyMaterial.material = materials[lat.Level-1];
            lat.Chess.CObject.transform.localScale = new Vector3(73, 73, 5);
        }
        else
        {
            lat.Chess.ChessGameObject.SetActive(false);
        }
    }

    void UpdateScore()
    {
        ScorePanel.text = score.ToString();
        if (state == State.PLAYING)
        {
            ScorePanelAnimation.PlayQueued("ScorePanelAnimation",QueueMode.PlayNow);

        }
        else
        {
            ScorePanelAnimation.PlayQueued("ScorePanelAnimation", QueueMode.CompleteOthers,PlayMode.StopAll);

        }
        BackGround.transform.Translate(Vector3.up * 20);
    }

    public void BackGroundM()
    {
        StartCoroutine(Move());
    }
    IEnumerator Move()
    {
        Vector3 vec = new Vector3(0, (591982 - BackGround.transform.localPosition.y) / 20, 0);
        for (int i = 0; i < 20; i++)
        {
            BackGround.transform.localPosition+=(2-Math.Abs(2-0.2f*(i+1)))*vec;
            yield return 0;
        }
    }
}
