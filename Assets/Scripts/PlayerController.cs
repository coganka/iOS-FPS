using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Transform viewPoint;
    public float sens = 1f;

    private float vertRotStore;
    private Vector2 mouseInput;

    public float moveSpeed = 5f, runSpeed = 8f;
    private float activeMoveSpeed;
    private Vector3 moveDir, movement;

    public CharacterController charCon;

    private Camera cam;

    public float jumpForce = 12f, gravityMod = 2.5f;

    public Transform groundCheck;
    private bool isGrounded;
    public LayerMask groundLayer;

    public GameObject impactGround;
    public GameObject bloodEffect;

    //public float timeBetweenShots = .1f;
    private float shotCounter;

    public float maxHeat = 10f, coolRate = 4f, overheatCoolRate = 5f;
    private float heatCounter;
    private bool overheated;

    //public Transform muzzle;
    public TrailRenderer tracer;
    public GameObject muzzleFlash;

    public Guns[] allGuns;
    private int selectedGun = 0;

    [HideInInspector]
    public Vector2 runAxis, lookAxis;
    [HideInInspector]
    public bool jumpAxis;



    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //SwitchGun();

        cam = Camera.main;

        Screen.orientation = ScreenOrientation.LandscapeLeft;


        //UIController.instance.energySlider.maxValue = maxHeat;

        //Transform newTrans = SpawnManager.instance.GetSpawnPoint();
        //transform.position = newTrans.position;
        //transform.rotation = newTrans.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //mouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * sens;
        mouseInput = lookAxis * sens;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseInput.x, transform.rotation.eulerAngles.z);

        vertRotStore += mouseInput.y;
        vertRotStore = Mathf.Clamp(vertRotStore, -60f, 60f);

        viewPoint.rotation = Quaternion.Euler(-vertRotStore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);

        moveDir = new Vector3(runAxis.x, 0f, runAxis.y);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runSpeed;
        } else
        {
            activeMoveSpeed = moveSpeed;
        }

        float yVel = movement.y;

        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized * activeMoveSpeed;
        movement.y = yVel;

        if (charCon.isGrounded)
        {
            movement.y = 0f;
        }

        isGrounded = Physics.Raycast(groundCheck.position, Vector3.down, .25f, groundLayer);

        if (jumpAxis && isGrounded)
        {
            movement.y = jumpForce;
        }
        movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;

        charCon.Move(movement * Time.deltaTime);


        // Shooting
        
        if (!overheated)
        {

            if (Input.GetMouseButtonDown(0) && ShootButton.instance.isPressed)
            {
                Shoot();
            }

            if (Input.GetMouseButton(0) && allGuns[selectedGun].isAuto && ShootButton.instance.isPressed)
            {
                shotCounter -= Time.deltaTime;
                if (shotCounter <= 0)
                {
                    Shoot();
                }
            }

            heatCounter -= coolRate * Time.deltaTime;
        } else
        {
            heatCounter -= overheatCoolRate * Time.deltaTime;

            if (heatCounter <= 0)
            {
                overheated = false;
                //UIController.instance.overheatImage.gameObject.SetActive(false);
            }
        }

        if (heatCounter < 0)
        {
            heatCounter = 0f;
        }

        //UIController.instance.energySlider.value = heatCounter;
        

        /*
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f)
        {
            selectedGun++;

            if (selectedGun >= allGuns.Length)
            {
                selectedGun = 0;
            }
            SwitchGun();
        } else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f)
        {
            selectedGun--;
            if (selectedGun < 0)
            {
                selectedGun = allGuns.Length - 1;
            }
            SwitchGun();
        }
        */
        /*
        for (int i = 0; i < allGuns.Length; i++)
        {
            if (Input.GetKeyDown((i+1).ToString()))
            {
                selectedGun = i;
                SwitchGun();
            }
        }*/

        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        } else if (Cursor.lockState == CursorLockMode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }*/
    }

    
    private void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        ray.origin = cam.transform.position;
        //ray.origin = raycastOrigin.position;

        //var tracerEffect = Instantiate(tracer, allGuns[selectedGun].muzzlePos.position, Quaternion.identity);
        //tracerEffect.AddPosition(allGuns[selectedGun].muzzlePos.position);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // hit
            if (hit.transform.tag == "Ground")
            {
                Instantiate(impactGround, hit.point, Quaternion.LookRotation(hit.normal));
            }
            if (hit.transform.tag == "Enemy")
            {
                Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }
        AudioManager.instance.PlayGunSound();

        Recoil.instance.Fire();
        //tracerEffect.transform.position = hit.point;
        Instantiate(muzzleFlash, allGuns[selectedGun].muzzlePos.position, allGuns[selectedGun].muzzlePos.rotation);

        shotCounter = allGuns[selectedGun].timeBetweenShots;

        heatCounter += allGuns[selectedGun].heatPerShot;

        if (heatCounter >= maxHeat)
        {
            heatCounter = maxHeat;
            overheated = true;

            //UIController.instance.overheatImage.gameObject.SetActive(true);
        }
    }

    /*
    void SwitchGun()
    {
        foreach (Guns gun in allGuns)
        {
            gun.gameObject.SetActive(false);
        }
        allGuns[selectedGun].gameObject.SetActive(true);
    } */


    private void LateUpdate()
    {
        cam.transform.position = viewPoint.position;
        cam.transform.rotation = viewPoint.rotation;
    }
}
