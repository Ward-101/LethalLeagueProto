using UnityEngine;

public class Scr_CharacterAttack : MonoBehaviour
{

    #region Variables
    [Header("Normal")]
    [SerializeField] private float normalStartupTime;
    [SerializeField] private float normalActiveTime;
    [SerializeField] private float normalRecoveryTime;
    [SerializeField] private float normalActiveStartupTime;
    [SerializeField] private float timeBeforeLockDirection;

    [Header("Smash")]
    [SerializeField] private float smashStartupTime;
    [SerializeField] private float smashActiveTime;
    [SerializeField] private float smashRecoveryTime;
    [SerializeField] private float smashActiveStartupTime;

    [Header("Lob")]
    [SerializeField] private float lobStartupTime;
    [SerializeField] private float lobActiveTime;
    [SerializeField] private float lobRecoveryTime;
    [SerializeField] private float lobActiveStartupTime;

    [Header("Edit")]
    [SerializeField] private LayerMask hitLayer;

    [Header("Inputs")]
    [SerializeField] private bool attackInput = false;
    [SerializeField] private bool chargeInput = false;
    [SerializeField] private bool lobInput = false;

    [Header("States")]
    public bool isNormal = false;
    public bool isSmash = false;
    public bool isLob = false;

    public bool isStartup = false;
    public bool isActive = false;
    public bool isRecovery = false;

    public bool processCollision = false;

    private Scr_CharacterMovement movement;
    private CapsuleCollider2D[] hitboxColliders;
    private CapsuleCollider2D selectedHitbox = null;
    [HideInInspector] public float attackDir = 0f;
    private bool lockNormal;
    private bool lockSmash;
    private bool lockLob;
    private float currentTime;
    //private float hitTimeScale = 1f; 

    #endregion

    private void Awake()
    {
        movement = GetComponent<Scr_CharacterMovement>();

        hitboxColliders = new CapsuleCollider2D[transform.GetChild(0).childCount];

        for (int i = 0; i < hitboxColliders.Length; i++)
        {
            hitboxColliders[i] = transform.GetChild(0).GetChild(i).GetComponent<CapsuleCollider2D>();
        }
    }

    private void Update()
    {
        GetInput();

        Normal();
        Smash();
        Lob();

        CollisionDetection();
    }

    private void GetInput()
    {
        attackInput = Input.GetButtonDown("P1_X");
        chargeInput = Input.GetButton("P1_X");
        lobInput = Input.GetButtonDown("P1_B");
    }

    private void Normal()
    {
        
    }

    private void Smash()
    {
        if ((attackInput && !isNormal && !isLob && !movement.isGrounded && movement.horizontalInput != 0 && !movement.isWallRide) || lockSmash)
        {
            //Ce joue une seul fois au début du l'attaque
            if (!isSmash)
            {
                isSmash = true;
                lockSmash = true;
                isStartup = true;

                //Need des comments du cette partie parce que c'est pas clair du tout
                if (!movement.isWallRide && !movement.isWallJump)
                {
                    attackDir = movement.lastXVelocity;
                }
                else
                {
                    attackDir = -movement.lastXVelocity;
                }

            }

            if (movement.isGrounded)
            {
                movement.canHorizontalMovement = false;
                movement.velocity.x = 0f;
            }
            else
            {
                movement.canHorizontalMovement = true;
            }

            currentTime += Time.deltaTime;

            if (isStartup)
            {
                if (currentTime < smashActiveStartupTime)
                {
                    processCollision = true;
                }
                else if (currentTime < smashStartupTime)
                {
                    processCollision = false;
                }
                else
                {
                    isStartup = false;
                    isActive = true;
                    currentTime = 0f;
                }

            }
            else if (isActive)
            {
                if (currentTime < smashActiveTime)
                {
                    isActive = true;
                    processCollision = true;
                }
                else
                {
                    isActive = false;
                    isRecovery = true;
                    currentTime = 0f;
                }
            }
            else if (isRecovery)
            {
                if (currentTime < smashRecoveryTime)
                {
                    isRecovery = true;
                    processCollision = false;
                }
                else
                {
                    isRecovery = false;
                    lockSmash = false;
                    movement.canHorizontalMovement = true;
                    currentTime = 0f;
                }
            }
        }
        else
        {
            isSmash = false;
        }
    }

    private void Lob()
    {
        if ((lobInput && !isSmash && !isNormal) || lockLob)
        {
            //Ce joue une seul fois au début du l'attaque
            if (!isLob)
            {
                isLob = true;
                lockLob = true;
                isStartup = true;

                //Need des comments du cette partie parce que c'est pas clair du tout
                if (movement.velocity.x != 0)
                {
                    if (!movement.isWallRide && !movement.isWallJump)
                    {
                        attackDir = movement.lastXVelocity;
                    }
                    else
                    {
                        attackDir = -movement.lastXVelocity;
                    }
                }
                else
                {
                    attackDir = movement.lastXVelocity;
                }

            }

            if (movement.isGrounded)
            {
                movement.canHorizontalMovement = false;
                movement.velocity.x = 0f;
            }
            else
            {
                movement.canHorizontalMovement = true;
            }

            currentTime += Time.deltaTime;

            if (isStartup)
            {
                if (currentTime < lobActiveStartupTime)
                {
                    processCollision = true;
                }
                else if (currentTime < lobStartupTime)
                {
                    processCollision = false;
                }
                else
                {
                    isStartup = false;
                    isActive = true;
                    currentTime = 0f;
                }

            }
            else if (isActive)
            {
                if (currentTime < lobActiveTime)
                {
                    isActive = true;
                    processCollision = true;
                }
                else
                {
                    isActive = false;
                    isRecovery = true;
                    currentTime = 0f;
                }
            }
            else if (isRecovery)
            {
                if (currentTime < lobRecoveryTime)
                {
                    isRecovery = true;
                    processCollision = false;
                }
                else
                {
                    isRecovery = false;
                    lockLob = false;
                    movement.canHorizontalMovement = true;
                    currentTime = 0f;
                }
            }
        }
        else
        {
            isLob = false;
        }
    }

    private void CollisionDetection()
    {
        if (processCollision)
        {
            //Choisi la capsule prit en référence pour le CapsuleOverlapAll
            if (isNormal || isSmash)
            {
                if (attackDir >= 0f)
                {
                    selectedHitbox = hitboxColliders[0];
                }
                else
                {
                    selectedHitbox = hitboxColliders[1];
                }
            }
            else if (isLob)
            {
                if (attackDir >= 0f)
                {
                    selectedHitbox = hitboxColliders[2];
                }
                else
                {
                    selectedHitbox = hitboxColliders[3];
                }
            }

            Collider2D[] hits = Physics2D.OverlapCapsuleAll(selectedHitbox.transform.position, selectedHitbox.size, selectedHitbox.direction, 0f, hitLayer);


            //foreach (collider2d hit in hits)
            //{
            //    debug.log("oui");

            //    if (isstartup)
            //    {
            //        isstartup = false;
            //        currenttime = 0f;
            //        isactive = true;
            //    }
            //}

               

        }
    }
}
