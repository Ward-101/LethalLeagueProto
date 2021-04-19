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
    [SerializeField] private float horizontalInput;
    [SerializeField] private bool attackInput;
    [SerializeField] private bool chargeInput;
    [SerializeField] private bool lobInput;

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
    [HideInInspector] public float attackDir = 0f;
    //private float lastHorizontalInput = 0f;
    private bool lockNormal;
    private bool lockSmash;
    private bool lockLob;
    private float currentTime;
    //private float hitTimeScale = 1f; 

    #endregion

    private void Awake()
    {
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

    }

    private void Lob()
    {
        if ((lobInput && !isSmash && !isNormal) || lockLob)
        {
            if (isLob != false)
            {
                isLob = true;
                lockLob = true;
                isStartup = true;
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
                    isStartup = true;
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
                if (currentTime > lobRecoveryTime)
                {
                    isRecovery = true;
                    processCollision = false;
                }
                else
                {
                    isRecovery = false;
                    lockLob = false;
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
            if (isStartup)
            {
                isActive = true;
                currentTime = normalActiveTime - normalActiveTime * 0.1f;
            }
            //CapsuleCollider2D[] hits = Physics2D.OverlapCapsuleAll(hitboxColliders[1].transform.position, hitboxColliders[1].size, hitboxColliders[1].direction, 0f, hitLayer);
        }
    }
}
