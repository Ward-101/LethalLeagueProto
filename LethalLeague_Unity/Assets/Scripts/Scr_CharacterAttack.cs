using UnityEngine;

public class Scr_CharacterAttack : MonoBehaviour
{
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

    public bool collisionIsActive = false;

    private Collider2D[] hitboxColliders;
    [HideInInspector] public float attackDir = 0f;

    private void Awake()
    {
        hitboxColliders = new Collider2D[transform.GetChild(0).childCount];

        for (int i = 0; i < hitboxColliders.Length; i++)
        {
            hitboxColliders[i] = transform.GetChild(0).GetChild(i).GetComponent<Collider2D>();
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
        horizontalInput = GetComponent<Scr_CharacterMovement>().horizontalInput;

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

    }


    private void CollisionDetection()
    {
        if (collisionIsActive)
        {

            if (isNormal && isSmash)
            {

            }

            else if (isLob)
            {

            }

            else
            {

            }
        }
    }
}
