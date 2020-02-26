using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public GameController gameController;
    private Rigidbody2D rigidBody;
    private Quaternion startRotation;
    private float planeRotationInput;
    private const float rotationSpeedButton = 4f;
    private const float rotationSpeedTilt = 16f; // First Number: 8;
    private float tiltStatus;
    private const float fallSpeed = 2f;
    private float acceleration;
    private const float accelerationlvl1 = 0.0003f;
    private const float accelerationlvl2 = 0.0001f;
    private const float accelerationlvl3 = 0.0003f;
    private float deceleration;
    private const float decelerationlvl1 = 0.005f;
    private const float decelerationlvl2 = 0.005f;
    private const float decelerationlvl3 = 0.01f;
    private const float carrierMoveSpeed = 0.045f;
    private float planetRotationSpeed = 0.4f;
    private float moveSpeed = -0.3f;
    private float scaleX, scaleY, scaleZ;
    private float shrinkScale = 1f;
    private float shadowX, shadowY, shadowZ;
    private float shadowMoveSpeed = 0.035f;
    private float shadowAlpha = 1f;
    public Transform runway;
    public Transform runwayCarrier;
    private BoxCollider2D runwayBounds;
    public BoxCollider2D carrierBounds;
    public BoxCollider2D runway1Bounds, runway2Bounds, runway3Bounds;
    private Vector2 planePositionRel;
    private float planeRotationRel;
    public BoxCollider2D planeCollider, jetCollider, shuttleCollider;
    private BoxCollider2D aircraftCollider;
    private bool planeLanded = false;
    private bool planeLanding = false;
    private bool onRunway = false;
    private bool pressedLeft, pressedRight;
    public GameObject GameOverPage;
    public GameObject inputButtons;
    public Text scoreText;
    public TextMeshProUGUI highscoreText, totalLandingsText;
    private static int score, highscore, tenPointLandings, totalLandings;
    private int level, plane1Type, plane2Type, plane3Type;
    private Sprite planeSprite;
    public Sprite planeWhite, planeBlack, jetGrey, jetBlack, shuttleWhite, shuttleBlack;
    public GameObject shadow;
    public GameObject arrow;
    public ParticleSystem explosion;
    public ParticleSystem splash;
    public GameObject wakeMajor, wakeMinor;
    public GameObject spriteMasklvl2, spriteMasklvl3;
    public GameObject blackHole;
    public CircleCollider2D planetBounds;
    private bool implode = false;
    private Vector3 startSize = new Vector3(0, 0, 0);
    private Vector3 endSize = new Vector3(1, 1, 1);

    private void Start()
    {
        level = PlayerPrefs.GetInt("level", 1);
        tiltStatus = PlayerPrefs.GetInt("tiltEnabled", 0);
        InitialiseUI();
        InitialisePlane();
        InitialiseRunway();
    }

    private void FixedUpdate()
    {
        CheckPlane();
        CheckRunway();
        if (implode)
            Implode();
    }

    private void InitialiseUI()
    {
        // Updating the UI to show player data related to scores by level
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
                // Lowering the Game Over score text so it is more visible (lvl 2 only)
                scoreText.transform.position = new Vector2(0, 0.5f);
                break;

            case 3:
                highscore = PlayerPrefs.GetInt("highscore3", 0);
                tenPointLandings = PlayerPrefs.GetInt("10PointLandings3", 0);
                totalLandings = PlayerPrefs.GetInt("landings3", 0);
                break;
        }
        // If a 10-point landing has been achieved, replace Best Score: with 10-Point Landings:
        if (highscore < 10)
            highscoreText.text = "Best Score: " + highscore.ToString();
        else
            highscoreText.text = "10-Point Landings: " + tenPointLandings.ToString();
        totalLandingsText.text = "Total Landings: " + totalLandings.ToString();
        // Enable Left and Right plane control buttons
        inputButtons.SetActive(true);
        // Initially hide the arrow that shows when you exit the screen area
        arrow.SetActive(false);
    }

    private void InitialisePlane()
    {
        rigidBody = GetComponent<Rigidbody2D>();

        // Applying a plane skin and collider depending on the user's selection and level
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
                aircraftCollider = planeCollider;
                this.planeCollider.enabled = true;
                this.jetCollider.enabled = false;
                this.shuttleCollider.enabled = false;
                acceleration = accelerationlvl1;
                deceleration = decelerationlvl1;
                break;

            case 2:
                if (plane2Type == 1)
                    planeSprite = jetBlack;
                else
                    planeSprite = jetGrey;
                aircraftCollider = jetCollider;
                this.planeCollider.enabled = false;
                this.jetCollider.enabled = true;
                this.shuttleCollider.enabled = false;
                acceleration = accelerationlvl2;
                deceleration = decelerationlvl2;
                break;

            case 3:
                if (plane3Type == 1)
                    planeSprite = shuttleBlack;
                else
                    planeSprite = shuttleWhite;
                aircraftCollider = shuttleCollider;
                this.planeCollider.enabled = false;
                this.jetCollider.enabled = false;
                this.shuttleCollider.enabled = true;
                acceleration = accelerationlvl3;
                deceleration = decelerationlvl3;
                break;
        }
        // Actually applying the plane and shadow skin
        GetComponent<SpriteRenderer>().sprite = planeSprite;
        shadow.GetComponent<SpriteRenderer>().sprite = planeSprite;
        // Randomise the plane's starting position
        transform.position = new Vector3(Random.Range(10.0f, 12.0f), Random.Range(-3.0f, 3.0f), -1000);
        // Randomise the plane's starting rotation
        if (transform.position.y > 0)
            transform.Rotate(0, 0, Random.Range(0.0f, 45.0f));
        else
            transform.Rotate(0, 0, Random.Range(315.0f, 360.0f));
        // Move and rotate the shadow to face the same direction as the plane
        shadow.transform.position = transform.position;
        shadow.transform.rotation = transform.rotation;

        arrow.transform.rotation = transform.rotation;
        // This line is required since the arrow points up by default
        arrow.transform.Rotate(0, 0, 90);
    }

    private void InitialiseRunway()
    {
        runway.position = new Vector2(0, 0);
        // Initialise the lvl 2 wake sprite objects to invisible
        wakeMajor.SetActive(false);
        wakeMinor.SetActive(false);
        // Initialise the lvl 2 and 3 sprite mask objects to invisible
        spriteMasklvl2.SetActive(false);
        spriteMasklvl3.SetActive(false);
        // Ensure the shadow by default is not hidden by sprite masks
        shadow.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
        if (level == 2)
        {
            runway.position = new Vector2(-12, 0);
            runwayBounds = runway2Bounds;
            wakeMajor.SetActive(true);
            wakeMinor.SetActive(true);
            spriteMasklvl2.SetActive(true);
        }
        else if (level == 3)
        {
            runwayBounds = runway3Bounds;
            spriteMasklvl3.SetActive(true);
            shadow.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            runway.Rotate(0, 0, Random.Range(0f, 360f));
        }
        else
        {
            runwayBounds = runway1Bounds;
        }
    }

    private void CheckPlane()
    {
        if (!rigidBody.simulated || planeLanded)
            return;
        planeRotationInput = Input.GetAxisRaw("Horizontal");
        if (transform.position.z >= 0 && !planeLanded)
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

    private void CheckRunway()
    {
        switch (level)
        {
            case 2:
                if (rigidBody.simulated && !planeLanded)
                {
                    runway.Translate(carrierMoveSpeed, 0, 0);
                }
                break;

            case 3:
                if (rigidBody.simulated && !planeLanding)
                {
                    runway.Rotate(0, 0, planetRotationSpeed);
                }
                break;
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
            if (tiltStatus == 1)
            {
                transform.Rotate(0, 0, -Input.acceleration.x * rotationSpeedTilt);
                shadow.transform.Rotate(0, 0, -Input.acceleration.x * rotationSpeedTilt);
                arrow.transform.Rotate(0, 0, -Input.acceleration.x * rotationSpeedTilt);
            }
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

    // Gives the appearance of the plane falling
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
        shadow.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, shadowAlpha);
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

    private void LandPlane()
    {
        moveSpeed += deceleration;
        CheckLandingPosition();
        if (!onRunway)
        {
            Explode();
            moveSpeed = 0;
        }
        if ((moveSpeed < 0 && (level == 1 || level == 3)) || (moveSpeed < -0.07 && level == 2))
        {
            transform.Translate(moveSpeed, 0, 0);
            planeLanding = true;
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

    // Explodes the plane when crashing
    private void Explode()
    {
        switch (level)
        {
            default:
            case 1:
                explosion.Stop();
                explosion.Clear();
                explosion.Play();
                break;

            case 2:
                if (carrierBounds.IsTouching(aircraftCollider))
                {
                    explosion.Stop();
                    explosion.Clear();
                    explosion.Play();
                }
                else
                {
                    splash.Stop();
                    splash.Clear();
                    splash.Play();
                }
                break;

            case 3:
                implode = true;
                break;
        }
        
        GetComponent<SpriteRenderer>().sprite = null;
        shadow.GetComponent<SpriteRenderer>().sprite = null;
    }

    private void Implode()
    {
        blackHole.SetActive(true);
        blackHole.transform.position = new Vector2(transform.position.x, transform.position.y);
        if (blackHole.transform.localScale.x < 0.96f)
        {
            blackHole.transform.localScale = Vector3.Lerp(blackHole.transform.localScale, endSize, Time.deltaTime);
            runway.position = Vector3.Lerp(runway.position, blackHole.transform.position, Time.deltaTime);
            runway.localScale = Vector3.Lerp(runway.localScale, startSize, Time.deltaTime);
        }
        blackHole.transform.Rotate(0, 0, -planetRotationSpeed * 2);
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

            totalLandings++;
            switch (level)
            {
                default:
                case 1:
                    planePositionRel = runway.InverseTransformPoint(transform.position);
                    planeRotationRel = transform.rotation.eulerAngles.z - runway.rotation.eulerAngles.z;
                    if (planeRotationRel < 90 || planeRotationRel > 270)
                    {
                        // Determining the zone (hence points) the plane landed in
                        for (int i = 0; i <= 10; i++)
                        {
                            if (planePositionRel.x > (2.51f - (0.502f * i)))
                            {
                                score = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Determining the zone (hence points) the plane landed in
                        for (int i = 0; i <= 10; i++)
                        {
                            if (planePositionRel.x < (-2.51 + (0.502 * i)))
                            {
                                score = i;
                                break;
                            }
                        }
                    }
                    /*if (onRunway && score == 0)
                        score = 10;*/
                    if (score > highscore)
                    {
                        highscore = score;
                        PlayerPrefs.SetInt("highscore1", highscore);
                        if (highscore < 10)
                            highscoreText.text = "Best Score: " + highscore.ToString();
                    }
                    if (score == 10)
                    {
                        tenPointLandings = PlayerPrefs.GetInt("10PointLandings1", 0) + 1;
                        highscoreText.text = "10-Point Landings: " + tenPointLandings.ToString();
                        PlayerPrefs.SetInt("10PointLandings1", tenPointLandings);
                    }
                    PlayerPrefs.SetInt("landings1", totalLandings);
                    totalLandingsText.text = "Total Landings: " + totalLandings.ToString();
                    break;

                case 2:
                    planePositionRel = runwayCarrier.InverseTransformPoint(transform.position);
                    planeRotationRel = transform.rotation.eulerAngles.z - runwayCarrier.rotation.eulerAngles.z;
                    if (planeRotationRel < 90 || planeRotationRel > 270)
                    {
                        // Determining the zone (hence points) the plane landed in
                        for (int i = 0; i <= 10; i++)
                        {
                            if (planePositionRel.x > (4.2f - (0.84f * i)))
                            {
                                score = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Determining the zone (hence points) the plane landed in
                        for (int i = 0; i <= 10; i++)
                        {
                            if (planePositionRel.x < (-4.2f + (0.84f * i)))
                            {
                                score = i;
                                break;
                            }
                        }
                    }
                    /*if (onRunway && score == 0)
                        score = 10;*/
                    if (score > highscore || highscore < 0)
                    {
                        highscore = score;
                        PlayerPrefs.SetInt("highscore2", highscore);
                        if (highscore < 10)
                            highscoreText.text = "Best Score: " + highscore.ToString();
                    }
                    if (score == 10)
                    {
                        tenPointLandings = PlayerPrefs.GetInt("10PointLandings2", 0) + 1;
                        highscoreText.text = "10-Point Landings: " + tenPointLandings.ToString();
                        PlayerPrefs.SetInt("10PointLandings2", tenPointLandings);
                    }
                    PlayerPrefs.SetInt("landings2", totalLandings);
                    totalLandingsText.text = "Total Landings: " + totalLandings.ToString();
                    break;

                case 3:
                    planePositionRel = runway.InverseTransformPoint(transform.position);
                    planeRotationRel = transform.rotation.eulerAngles.z - runway.rotation.eulerAngles.z;
                    if ((planeRotationRel > -90 && planeRotationRel < 90) || (planeRotationRel > 270 && planeRotationRel < 450))
                    {
                        // Determining the zone (hence points) the plane landed in
                        for (int i = 0; i <= 10; i++)
                        {
                            if (planePositionRel.x > (1.85f - (0.37f * i)))
                            {
                                score = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // Determining the zone (hence points) the plane landed in
                        for (int i = 0; i <= 10; i++)
                        {
                            if (planePositionRel.x < (-1.85f + (0.37f * i)))
                            {
                                score = i;
                                break;
                            }
                        }
                    }
                    /*if (onRunway && score == 0)
                        score = 10;*/
                    if (score > highscore || highscore < 0)
                    {
                        highscore = score;
                        PlayerPrefs.SetInt("highscore3", highscore);
                        if (highscore < 10)
                            highscoreText.text = "Best Score: " + highscore.ToString();
                    }
                    if (score == 10)
                    {
                        tenPointLandings = PlayerPrefs.GetInt("10PointLandings3", 0) + 1;
                        highscoreText.text = "10-Point Landings: " + tenPointLandings.ToString();
                        PlayerPrefs.SetInt("10PointLandings3", tenPointLandings);
                    }
                    PlayerPrefs.SetInt("landings3", totalLandings);
                    totalLandingsText.text = "Total Landings: " + totalLandings.ToString();
                    break;
            }
            PlayerPrefs.Save();
        }
        scoreText.text = "Score: " + score.ToString();
        score = 0;
    }
}
