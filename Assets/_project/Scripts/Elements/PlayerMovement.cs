using System;
using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Camera mainCamera;
    public float walkSpeed;
    public float runSpeed;
    public float jumpForce;
    public float fallSpeedBonus;

    public SpaceKeyBehaviour spaceKeyBehaviour;

    private Rigidbody _rb;

    public LayerMask jumpLayers;
    public LayerMask lookLayers;

    private Animator _animator;

    private bool _isJumping;
    public bool isSwitchingWeapon;
    private Player _player;

    private Vector3 _direction;

    public float startStamina;
    private float _currentStamina;
    public HealthBar staminaBar;

    public bool isThrowingGrenade;


    private void Awake()
    {
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void RestartPlayerMovement()
    {
        _rb.constraints = RigidbodyConstraints.FreezeRotation;
        ChangeAnimationState("Idle");
        _currentStamina = startStamina;
    }

    private void Update()
    {
        _direction = Vector3.zero;

        if (_player.gameDirector.gameState != GameState.GamePlay || _player.isDead)
        {
            _rb.linearVelocity = Vector3.zero;
            return;
        }        
        if (Input.GetKey(KeyCode.W))
        {
            _direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _direction += Vector3.right;
        }

        var speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_currentStamina > 0)
            {
                speed = runSpeed;
                if (_direction.magnitude > 0)
                {
                    _currentStamina -= Time.deltaTime;
                }
            }            
        }
        else if (_currentStamina < startStamina)
        {
            if (_direction.magnitude == 0)
            {
                _currentStamina += Time.deltaTime;
            }
            else
            {
                _currentStamina += Time.deltaTime * .5f;
            }
        }

        _currentStamina = Mathf.Clamp(_currentStamina, 0, startStamina);

        staminaBar.SetFillBar(_currentStamina / startStamina);

        staminaBar.transform.position = transform.position + Vector3.right + Vector3.up;

        _isJumping = !CheckIfLanded();
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CheckIfLanded() && spaceKeyBehaviour == SpaceKeyBehaviour.Jump)
            {
                Jump();
            }
            else if (!_isDashing)
            {
                StartCoroutine(Dash());
            }
        }

        if (!_isDashing)
        {
            MovePlayer(_direction, speed);
        }
        LookAtMouse();

        SetWalkDirection(Vector3.SignedAngle(transform.forward, _direction, Vector3.up));
        dashPS.transform.position = transform.position + Vector3.up;
        if (spaceKeyBehaviour == SpaceKeyBehaviour.Dash && transform.position.y > 0.1f)
        {
            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }
    }

    public float dashDuration;
    public float dashForce;
    private bool _isDashing;
    public ParticleSystem dashPS;

    IEnumerator Dash()
    {
        _isDashing = true;
        _rb.AddForce(dashForce * _direction);
        dashPS.transform.LookAt(dashPS.transform.position - _direction);
        dashPS.Play();
        _player.gameDirector.audioManager.PlayDashAS();
        yield return new WaitForSeconds(dashDuration);
        _isDashing = false;
    }

    void SetWalkDirection(float angle)
    {
        _animator.SetFloat("WalkDirection", angle);
    }

    private void LookAtMouse()
    {
        if (Physics.Raycast(mainCamera.transform.position,
            mainCamera.ScreenPointToRay(Input.mousePosition).direction,
            out var hit,
            50,
            lookLayers))
        {
            var lookPos = hit.point;
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
        }
    }

    private bool CheckIfLanded()
    {
        if (Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, out RaycastHit hit, 3f, jumpLayers))
        {
            _player.EnableShadow();
            _player.shadow.transform.position = new Vector3(_player.transform.position.x, .1f, _player.transform.position.z);
        }
        else
        {
            _player.DisableShadow();
        }
        if (Physics.Raycast(transform.position + Vector3.up * .1f, Vector3.down, .3f, jumpLayers))
        {
            return true;
        }
        return false;
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce);
        _isJumping = true;
        ChangeAnimationState("Jump");
    }

    void MovePlayer(Vector3 dir, float speed)
    {
        var yVelocity = _rb.linearVelocity;

        yVelocity.x = 0;
        yVelocity.z = 0;

        if (yVelocity.y < 0)
        {
            yVelocity.y -= fallSpeedBonus * Time.deltaTime;
        }

        if (!_isJumping && !isSwitchingWeapon && !isThrowingGrenade)
        {
            if (dir.magnitude > 0)
            {
                ChangeAnimationState("Run");
            }
            else
            {
                ChangeAnimationState("Idle");
            }
        }        

        _rb.linearVelocity = dir.normalized * speed + yVelocity;
    }

    public void ChangeAnimationState(string key)
    {
        _animator.SetBool("Idle", false);
        _animator.SetBool("Run", false);
        _animator.SetBool("Jump", false);
        _animator.SetBool("Die", false);
        _animator.SetBool("Die2", false);
        _animator.SetBool("Win", false);
        _animator.SetBool("ChangeWeapon", false);
        _animator.SetBool("ThrowGrenade", false);
        _animator.SetBool(key, true);
    }

    public void PlayAlternativeFailAnimation()
    {
        ChangeAnimationState("Die2");
    }

    public Vector3 GetCurrentDirection()
    {
        return _direction;
    }
    public void SetUpperBodyLayerWeightTo1()
    {
        _animator.SetLayerWeight(1, 1);
    }
    public void SetUpperBodyLayerWeightTo0()
    {
        _animator.SetLayerWeight(1, 0);
    }
}

public enum SpaceKeyBehaviour
{
    Jump,
    Dash,
}