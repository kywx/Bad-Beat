using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementStats MoveStats;
    [SerializeField] private Collider2D _bodyCollider;
    [SerializeField] private Collider2D _feetCollider;

    private Rigidbody2D _rb;

    private Vector2 _moveVelocity;
    private bool _isFacingRight;

    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    public float VerticalVelocity { get; private set; }
    private bool _isJumping;
    private bool _isFastFalling;
    private bool _isFalling;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private int _numberOfJumpsUsed;

    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    private float _coyoteTimer;

    private void Awake()
    {
        _isFacingRight = true;
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CountTimers();
        JumpChecks();
    }

    private void FixedUpdate()
    {
        CollisionCheck();
        Jump();

        if (_isGrounded)
        {
            Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, InputManager.Movement);
        } else
        {
            Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, InputManager.Movement);
        }
    }

    #region Movement
    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        TurnCheck(moveInput);

        float maxSpeed = InputManager.RunIsHeld ? MoveStats.MaxRunSpeed : MoveStats.MaxWalkSpeed;
        float targetSpeed = moveInput.x * maxSpeed;
        float speedDifference = targetSpeed - _rb.linearVelocity.x;
        float accelRate = (moveInput.x != 0) ? acceleration : deceleration;
        float movement = speedDifference * accelRate;
        _rb.AddForce(new Vector2(movement, 0f), ForceMode2D.Force);
        if (Mathf.Abs(_rb.linearVelocity.x) > maxSpeed)
        {
            _rb.linearVelocity = new Vector2(Mathf.Sign(_rb.linearVelocity.x) * maxSpeed, _rb.linearVelocity.y);
        }
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);
        }

    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight)
        {
            Turn(false);
        } else if (!_isFacingRight && moveInput.x > 0)
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        } else
        {
            _isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
        }
    }
    #endregion

    #region Jump
    private void JumpChecks()
    {
        // when jump pressed
        if (InputManager.JumpWasPressed)
        {
            _jumpBufferTimer = MoveStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
        }
        // when released
        if (InputManager.JumpWasReleased)
        {
            if (_jumpBufferTimer > 0f)
            {
                _jumpReleasedDuringBuffer = true;
            }
            if (_isJumping && VerticalVelocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = MoveStats.TimeForUpwardsCancel;
                    VerticalVelocity = 0f;
                } else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = VerticalVelocity;
                }
            }
        }
        // Jump logic
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            // Normal grounded jump
            InitiateJump(1);
            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = VerticalVelocity;
            }
        }
        else if (_jumpBufferTimer > 0f && _isJumping && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed)
        {
            // While jumping -> double jump
            _isFastFalling = false;
            InitiateJump(1);
        }
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < MoveStats.NumberOfJumpsAllowed - 1)
        {
            // While falling -> air jump
            _isFastFalling = false;
            InitiateJump(2);
        }
        // Landed
        if ((_isJumping || _isFalling) && _isGrounded && VerticalVelocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _fastFallTime = 0f;
            _isPastApexThreshold = false;
            _numberOfJumpsUsed = 0;

            VerticalVelocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int numberOfJumpsUsed)
    {
        if (!_isJumping) _isJumping = true;
        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumpsUsed;
        VerticalVelocity = MoveStats.InitialJumpVelocity;
    }

    private void Jump()
    {
        // Apply gravity while jumping
        if (_isJumping)
        {
            if (_bumpedHead)
            {
                _isFastFalling = true;
            }

            // Jumping gravity
            if (VerticalVelocity >= 0f)
            {
                _apexPoint = Mathf.InverseLerp(MoveStats.InitialJumpVelocity, 0f, VerticalVelocity);
                if (_apexPoint > MoveStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }
                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < MoveStats.ApexHangTime)
                        {
                            VerticalVelocity = 0f;
                        } else
                        {
                            VerticalVelocity = -0.01f;
                            _isPastApexThreshold = false;
                            _isJumping = false;
                            _isFalling = true;
                        }
                    }
                } else
                {
                    VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                    {
                        _isPastApexThreshold = false;
                    }
                }
            }
        } else if (!_isFastFalling)
        {
            // Falling gravity
            VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
        } else if (VerticalVelocity < 0f)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
        }

        // Jump cut
        if (_isFastFalling)
        {
            if (_fastFallTime >= MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity += MoveStats.Gravity * MoveStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            } else if (_fastFallTime < MoveStats.TimeForUpwardsCancel)
            {
                VerticalVelocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / MoveStats.TimeForUpwardsCancel));
            }
            _fastFallTime += Time.fixedDeltaTime;
        }

        // Normal falling gravity
        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
            {
                _isFalling = true;
            }
            VerticalVelocity += MoveStats.Gravity * Time.fixedDeltaTime;
        }

        // Clamp velocity
        VerticalVelocity = Mathf.Clamp(VerticalVelocity, -MoveStats.MaxFallSpeed, 50f);  // 50 is max, can change

        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, VerticalVelocity);

    }

    #endregion

    #region Collision Checks
    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider.bounds.center.x, _feetCollider.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetCollider.bounds.size.x, MoveStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, MoveStats.GroundDetectionRayLength, MoveStats.GroundLayer);
        if (_groundHit.collider != null)
        {
            _isGrounded = true;
        } else
        {
            _isGrounded = false;
        }

        #region Debug Visualization
        if (MoveStats.DebugShowIsGroundedBox)
        {
            Color rayColor;
            if (_isGrounded)
            {
                rayColor = Color.green;
            } else
            {
                rayColor= Color.red;
            }

            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * MoveStats.GroundDetectionRayLength, rayColor);
            Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y - MoveStats.GroundDetectionRayLength), Vector2.right * boxCastSize.x, rayColor);
        }
        #endregion
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetCollider.bounds.center.x, _bodyCollider.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetCollider.bounds.size.x * MoveStats.HeadWidth, MoveStats.HeadDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up, MoveStats.HeadDetectionRayLength, MoveStats.GroundLayer);
        if (_headHit.collider != null)
        {
            _bumpedHead = true;
        } else
        {
            _bumpedHead = false;
        }
        #region Debug Visualization
        if (MoveStats.DebugShowHeadBumpBox)
        {
            float headWidth = MoveStats.HeadWidth;

            Color rayColor;
            if (_bumpedHead)
            {
                rayColor = Color.green;
            } else
            {
                rayColor = Color.red;
            }

            Debug.DrawLine(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawLine(new Vector2(boxCastOrigin.x + (boxCastSize.x / 2) * headWidth, boxCastOrigin.y), Vector2.up * MoveStats.HeadDetectionRayLength, rayColor);
            Debug.DrawLine(new Vector2(boxCastOrigin.x - boxCastSize.x / 2 * headWidth, boxCastOrigin.y + MoveStats.HeadDetectionRayLength), Vector2.right * boxCastSize.x * headWidth, rayColor);
        }
        #endregion
    }

    private void CollisionCheck()
    {
        IsGrounded();
        BumpedHead();
    }
    #endregion

    #region Timers
    private void CountTimers()
    {
        _jumpBufferTimer -= Time.fixedDeltaTime;
        if (!_isGrounded)
        {
            _coyoteTimer -= Time.fixedDeltaTime;
        } else
        {
            _coyoteTimer = MoveStats.JumpCoyoteTime;
        }
    }
    #endregion
}
