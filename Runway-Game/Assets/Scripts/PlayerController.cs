using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public GameController gameController;
    private Rigidbody2D rigidBody;
    private const float startPositionX = 14;
    private const float startPositionY = 0;
    private const float startRotation = 0;
    private float planeRotationInput;
    private const float rotationSpeedButton = 4f;
    private const float rotationSpeedTilt = 8f;
    private const float fallSpeed = 2f;
    private const float acceleration = 0.0003f;
    private const float deceleration = 0.005f; //old value which was good: 0.0025f
    private float moveSpeed = -0.3f;
    private float scaleX, scaleY, scaleZ;
    private float shrinkScale = 1f;
    private float shadowX, shadowY, shadowZ;
    private float shadowMoveSpeed = 0.035f;
    private float shadowAlpha = 1f;
    public Transform runway;
    public BoxCollider2D runwayBounds;
    private Vector2 planePositionRel;
    private float planeRotationRel;
    public BoxCollider2D planeCollider, jetCollider, shuttleCollider;
    private BoxCollider2D aircraftCollider;
    private bool planeLanded;
    private bool onRunway;
    private bool pressedLeft, pressedRight;
    public GameObject GameOverPage;
    public GameObject inputButtons;
    public Text scoreText, highscoreText, tenPointText;
    private static int score, highscore, tenPointLandings, totalLandings;
    private int level, plane1Type, plane2Type, plane3Type;
    private Sprite planeSprite, shadowSprite;
    public Sprite planeWhite, planeBlack, jetGrey, jetBlack, shuttleWhite, shuttleBlack;
    public Sprite planeShadow, jetShadow, shuttleShadow;
    public GameObject shadow;
    public GameObject arrow;
    public ParticleSystem explosion;

    private void Start()
    {
        InitialisePlane();
        InitialiseUI();
    }

    private void FixedUpdate()
    {
        CheckPlane();
        CheckRunway();
    }

    private void CheckRunway()
    {
        if (level == 0)
        {
            level = PlayerPrefs.GetInt("level", 1);
        }
        switch (level)
        {
            default:
            case 1:

                break;

            case 2:

                break;

            case 3:

                break;
        }
    }

    private void InitialisePlane()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        // Applying a plane skin depending on the user's selection
        level = PlayerPrefs.GetInt("level", 1);
        plane1Type = PlayerPrefs.GetInt("plane1", 0);
        plane2Type = PlayerPrefs.GetInt("plane2", 0);
        plane3Type = PlayerPrefs.GetInt("plane3", 0);
        switch (level)
        {
            default:
            case 1:
                if (plane1Type == 1)
                    planeSprite = planeBlack;
                else
                    planeSprite = planeWhite;
                shadowSprite = planeShadow;
                aircraftCollider = planeCollider;
                this.planeCollider.enabled = true;
                this.jetCollider.enabled = false;
                this.shuttleCollider.enabled = false;
                break;

            case 2:
                if (plane2Type == 1)
                    planeSprite = jetBlack;
                else
                    planeSprite = jetGrey;
                shadowSprite = jetShadow;
                aircraftCollider = jetCollider;
                this.planeCollider.enabled = false;
                this.jetCollider.enabled = true;
                this.shuttleCollider.enabled = false;
                break;

            case 3:
                if (plane3Type == 1)
                    planeSprite = shuttleBlack;
                else
                    planeSprite = shuttleWhite;
                shadowSprite = shuttleShadow;
                aircraftCollider = shuttleCollider;
                this.planeCollider.enabled = false;
                this.jetCollider.enabled = false;
                this.shuttleCollider.enabled = true;
                break;
        }
        GetComponent<SpriteRenderer>().sprite = planeSprite;
        shadow.GetComponent<SpriteRenderer>().sprite = shadowSprite;
    }

    private void InitialiseUI()
    {
        switch (level)
        {
            default:
            case 1:
                highscore = PlayerPrefs.GetInt("highscore1", 0);
                tenPointLandings = PlayerPrefs.GetInt("10PointLandings1", 0);
                totalLandings = PlayerPrefs.GetInt("landings1", 0);
                break;

            case 2:
                highscore = PlayerPrefs.GetInt("highscore2", 0);
                tenPointLandings = PlayerPrefs.GetInt("10PointLandings2", 0);
                totalLandings = PlayerPrefs.GetInt("landings2", 0);
                break;

            case 3:
                highscore = PlayerPrefs.GetInt("highscore3", 0);
                tenPointLandings = PlayerPrefs.GetInt("10PointLandings3", 0);
                totalLandings = PlayerPrefs.GetInt("landings3", 0);
                break;
        }
        highscoreText.text = "Best: " + highscore.ToString();
        tenPointText.text = "10-Point Landings: " + tenPointLandings.ToString();
        inputButtons.SetActive(true);
        arrow.SetActive(false);
    }

    private void CheckPlane()
    {
        if (!rigidBody.simulated || planeLanded)
            return;
        planeRotationInput = Input.GetAxisRaw("Horizontal");
        if (transform.position.z >= 0 && planeLanded == false)
        {
            LandPlane();
        }
        else
        {
            MovePlane(planeRotationInput);
            ShrinkPlane();
            RotatePlane();
            UpdateShadow();
            if (transform.position.x <= -20 || transform.position.x >= 20 || transform.position.y <= -13 || transform.position.y >= 13)
            {
                arrow.SetActive(true);
                arrow.transform.position = new Vector2(transform.position.x, transform.position.y);
                if (arrow.transform.position.x < -15)
                {
                    arrow.transform.position = new Vector2(-15, arrow.transform.position.y);
                }
                else if (arrow.transform.position.x > 15)
                {
                    arrow.transform.position = new Vector2(15, arrow.transform.position.y);
                }
                if (arrow.transform.position.y < -6)
                {
                    arrow.transform.position = new Vector2(arrow.transform.position.x, -6);
                }
                else if (arrow.transform.position.y > 6)
                {
                    arrow.transform.position = new Vector2(arrow.transform.position.x, 6);
                }
            }
            else
            {
                arrow.SetActive(false);
            }
        }
    }

    public void ButtonPressedLeft()
    {
        pressedLeft = true;
    }

    public void ButtonReleasedLeft()
    {
        pressedLeft = false;
    }

    public void ButtonPressedRight()
    {
        pressedRight = true;
    }

    public void ButtonReleasedRight()
    {
        pressedRight = false;
    }

    private void RotatePlane()
    {
        if (pressedLeft)
        {
            transform.Rotate(0, 0, 1 * rotationSpeedButton);
            shadow.transform.Rotate(0, 0, 1 * rotationSpeedButton);
            arrow.transform.Rotate(0, 0, 1 * rotationSpeedButton);
        }
        else if (pressedRight)
        {
            transform.Rotate(0, 0, -1 * rotationSpeedButton);
            shadow.transform.Rotate(0, 0, -1 * rotationSpeedButton);
            arrow.transform.Rotate(0, 0, -1 * rotationSpeedButton);
        }
        else
        {
            transform.Rotate(0, 0, -Input.acceleration.x * rotationSpeedTilt);
            shadow.transform.Rotate(0, 0, -Input.acceleration.x * rotationSpeedTilt);
            arrow.transform.Rotate(0, 0, -Input.acceleration.x * rotationSpeedTilt);
        }
    }

    private void MovePlane(float planeRotationInput)
    {
        transform.Rotate(0, 0, -planeRotationInput * rotationSpeedButton);
        shadow.transform.Rotate(0, 0, -planeRotationInput * rotationSpeedButton);
        arrow.transform.Rotate(0, 0, -planeRotationInput * rotationSpeedButton);
        transform.Translate(moveSpeed, 0, fallSpeed);
        moveSpeed += acceleration;
    } 
    private void ShrinkPlane()
    {
        if (shrinkScale > 0.2)
        {
            shrinkScale = -(transform.position.z) / 800 - 0.25f;
        }
        else
        {
            shrinkScale = shrinkScale - 0.0009f;
        }
        scaleX = shrinkScale;
        scaleY = shrinkScale;
        scaleZ = transform.localScale.z;
        transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
        shadow.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
    }

    private void UpdateShadow()
    {
        shadowX = transform.position.x + 0.05f + transform.position.z * shadowMoveSpeed / 2;
        shadowY = transform.position.y - 0.05f + transform.position.z * shadowMoveSpeed / 2;
        shadowZ = transform.position.z;
        shadow.transform.position = new Vector3(shadowX, shadowY, shadowZ);
        shadowAlpha = 0.75f / (-transform.position.z / 40);
        shadow.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, shadowAlpha);
    }

    private void CheckLandingPosition()
    {
        if (runwayBounds.IsTouching(aircraftCollider))
        {
            onRunway = true;
        }
        else
        {
            onRunway = false;
        }
    }

    private void Explode()
    {
        explosion.Stop();
        explosion.Clear();
        explosion.Play();
        GetComponent<SpriteRenderer>().sprite = null;
        shadow.GetComponent<SpriteRenderer>().sprite = null;
    }

    private void LandPlane()
    {
        moveSpeed += deceleration;
        CheckLandingPosition();
        if (!onRunway)
        {
            Explode();
            moveSpeed = 0;
        }
        if (moveSpeed < 0)
        {
            transform.Translate(moveSpeed, 0, 0);
        }
        else
        {
            planeLanded = true;
            GameOverPage.SetActive(true);
            inputButtons.SetActive(false);
            arrow.SetActive(false);
            ShowScore();
            gameController.EndMusic();
        }
    }

    private void ShowScore()
    {
        if (!onRunway)
        {
            score = 0;
        }
        else
        {
            // Converting the plane's global position and rotation to local equivalents relative to the runway
            planePositionRel = runway.InverseTransformPoint(transform.position);
            planeRotationRel = transform.rotation.eulerAngles.z - runway.rotation.eulerAngles.z;
            totalLandings++;
            print(planePositionRel.x);
            print(planeRotationRel);
            switch (level)
            {
                default:
                case 1:
                    if (planeRotationRel < 90 || planeRotationRel > 270)
                    {
                        // Determining the zone (hence points) the plane landed in
                        for (int i = 0; i < 10; i++)
                        {
                            if (planePositionRel.x > (2.51f - (0.502f * i)))
                            {
                                score = i + 1;
                                print("i: " + i);
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Determining the zone (hence points) the plane landed in
                        for (int i = 0; i < 9; i++)
                        {
                            if (planePositionRel.x < (-2.51 + (0.502 * i)))
                            {
                                score = i + 1;
                                print("i: " + i);
                                break;
                            }
                            else
                            {

                            }
                        }
                    }
                    if (score > highscore || highscore < 0)
                    {
                        highscore = score;
                        PlayerPrefs.SetInt("highscore1", highscore);
                    }
                    if (score == 10)
                    {
                        tenPointLandings = PlayerPrefs.GetInt("10PointLandings1", 0) + 1;
                        tenPointText.text = "10-Point Landings: " + tenPointLandings.ToString();
                        PlayerPrefs.SetInt("10PointLandings1", tenPointLandings);
                    }
                    PlayerPrefs.SetInt("landings1", totalLandings);
                    break;

                case 2:
                    if (score > highscore || highscore < 0)
                    {
                        highscore = score;
                        PlayerPrefs.SetInt("highscore2", highscore);
                        highscoreText.text = "Best Score: " + highscore.ToString();
                    }
                    if (score == 10)
                    {
                        tenPointLandings = PlayerPrefs.GetInt("10PointLandings2", 0) + 1;
                        tenPointText.text = "10-Point Landings: " + tenPointLandings.ToString();
                        PlayerPrefs.SetInt("10PointLandings2", tenPointLandings);
                    }
                    PlayerPrefs.SetInt("landings2", totalLandings);
                    break;

                case 3:
                    if (score > highscore || highscore < 0)
                    {
                        highscore = score;
                        PlayerPrefs.SetInt("highscore3", highscore);
                        highscoreText.text = "Best Score: " + highscore.ToString();
                    }
                    if (score == 10)
                    {
                        tenPointLandings = PlayerPrefs.GetInt("10PointLandings3", 0) + 1;
                        tenPointText.text = "10-Point Landings: " + tenPointLandings.ToString();
                        PlayerPrefs.SetInt("10PointLandings3", tenPointLandings);
                    }
                    PlayerPrefs.SetInt("landings3", totalLandings);
                    break;
            }
        }
        scoreText.text = "Score: " + score.ToString();
        PlayerPrefs.Save();
    }
}
