using System;
using System.Collections;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;

    public bool PaddleIsTransforming { get; set; }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    private Camera mainCamera;
    private float paddleInitialY;
    private float defaultPaddleWidthInPixels = 200;
    private float defaultLeftClamp = 135;
    private float defaultRightClamp = 410;
    private SpriteRenderer sr;
    private BoxCollider2D boxCol;

    public float extendShrinkDuration = 10;
    public float paddleWidth = 2;
    public float paddleHeight = 0.28f;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        paddleInitialY = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();
        boxCol = GetComponent<BoxCollider2D>();

    }

    private void Update()
    {
        PaddleMovement();
    }

    public void StartWidthAnimation(float newWidth)
    {
        StartCoroutine(AnimatePaddleWidth(newWidth));
    }

    public IEnumerator AnimatePaddleWidth(float width)
    {
        this.PaddleIsTransforming = true;
        this.StartCoroutine(ResetPaddleWidthAfterTime(this.extendShrinkDuration));

        if (width > this.sr.size.x)
        {
            float currentWidth = this.sr.size.x;
            while (currentWidth < width)
            {
                currentWidth += Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }
        else
        {
            float currentWidth = this.sr.size.x;
            while (currentWidth > width)
            {
                currentWidth -= Time.deltaTime * 2;
                this.sr.size = new Vector2(currentWidth, paddleHeight);
                boxCol.size = new Vector2(currentWidth, paddleHeight);
                yield return null;
            }
        }

        this.PaddleIsTransforming = false;
    }

    private IEnumerator ResetPaddleWidthAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        this.StartWidthAnimation(this.paddleWidth);
    }

    private void PaddleMovement()
    {
        float paddleShift = (defaultPaddleWidthInPixels - ((defaultPaddleWidthInPixels / 2) * this.sr.size.x)) / 2;
        float leftClamp = defaultLeftClamp - paddleShift;
        float rightClamp = defaultRightClamp + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(mousePositionWorldX, paddleInitialY, 0);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = coll.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = coll.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;

            if (hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * 200)), BallsManager.Instance.initialBallSpeed));
            }
        }
    }
}
