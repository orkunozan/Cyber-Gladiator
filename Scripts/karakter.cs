using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class karakter : Photon.MonoBehaviour
{

    #region Values
    float health;
    GameObject netM;
    GameObject dead_screen;
    Text dead_info;
    public Text Name;
    public RawImage bar;
    public GameObject canvas;
    GameObject LobbyName;
    GameObject LobbyHealthbar;
    private CharacterController characterController;
    public static float movementSpeed = 6;
    public AnimationCurve jumpFallOff;
    public float jumpMultiplier;
    public Text ammotext;
    public Text reloadtext;
    public Text shieldtext;
    int shieldleft; //Eklenmedi
    int bulletleft;
    public static int totalbullet;
    int currentbullet;
    int lastbullet;
    bool shieldcontrol = true; //EKLENMEDİ DAHA
    bool isRelod = false;
    GameObject shield;
    GameObject shield2;
    GameObject effectSended1;
    public Transform shieldpoint1;
    public Transform shieldpoint2;
    public Transform firepoint;
    Actions actions;
    public ParticleSystem muzzleFlash;
    bool isJumping;
    public AudioSource shotSound;

    #endregion




    void Awake()
    {

        actions = GetComponent<Actions>();
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        health = 100;
        shieldleft = 3;
        bulletleft = 15;
        totalbullet = 90;
        currentbullet = 15;
        jumpMultiplier = 10;




    }






    void Start()
    {

        netM = GameObject.Find("NetworkManager");
        dead_screen = netM.GetComponent<NetworkManager>().olum_ekrani;
        dead_info = netM.GetComponent<NetworkManager>().olum_bilgisi;
        LobbyName = GameObject.Find("Name");
        LobbyHealthbar = GameObject.Find("healthBarFront");

        if (photonView.isMine)
        {
            GetComponentInChildren<Camera>().enabled = true;
        }





    }

    void Update()
    {


        healthbar();

        if (photonView.isMine)
        {
            actions.Stay();
            animationControl();
            characterControl();
            raycastV();
            shieldOn();
            characterCanvas();
            GetComponent<PhotonView>().RPC("nameV", PhotonTargets.All, PhotonNetwork.playerName);
            canvas.SetActive(false);

        }



    }

    void animationControl()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            actions.Walk();
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {


            if (staminaController.isOverStamina)
            {
                actions.Walk();
            }
            if (!staminaController.isOverStamina)
            {
                actions.Run();

            }
            if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
            }
            if (health <= 0)
            {
                if (photonView.isMine)
                {
                    actions.Death();

                }
            }
        }

    }

    void characterControl()
    {
        float hori = Input.GetAxis("Horizontal") * movementSpeed;
        float vert = Input.GetAxis("Vertical") * movementSpeed;
        Vector3 moveForward = transform.forward * vert;
        Vector3 moveRight = transform.right * hori;
        characterController.SimpleMove(moveForward + moveRight);

        float mouseX = Input.GetAxis("Mouse X") * 100 * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            actions.Jump();

            isJumping = true;
            StartCoroutine(JumpEvent());
        }

        //Jump eklemek olsun mu olmasın mı
    }


    void raycastV()
    {
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0) && bulletleft > 0 && !isRelod && totalbullet != -15)
        {

            actions.Attack();
            GetComponent<PhotonView>().RPC("shootingEffect", PhotonTargets.All);
            if (Physics.Raycast(firepoint.position, transform.forward, out hit, 500f))
            {
                if (hit.collider.gameObject.tag == "Player" && !hit.collider.gameObject.GetComponent<PhotonView>().isMine)
                {
                    hit.collider.gameObject.GetComponent<PhotonView>().RPC("takeDamage", PhotonTargets.All, 10f, PhotonNetwork.player.NickName);


                    effectSended1 = PhotonNetwork.Instantiate("BulletImpactMetalEffect", hit.point, Quaternion.LookRotation(hit.normal), 0);
                }

            }

            effectSended1 = PhotonNetwork.Instantiate("BulletImpactMetalEffect", hit.point, Quaternion.LookRotation(hit.normal), 0);
            bulletleft--;

        }
        if (bulletleft < 15 && totalbullet != 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                isRelod = true;
                reloadtext.text = "RELOAD";
                StartCoroutine(Reloadtimer());
            }
        }
    }

    void shieldOn()
    {

        if (Input.GetMouseButtonDown(1) && shieldleft > 0 && shieldcontrol)
        {

            shield = PhotonNetwork.Instantiate("Sheld", shieldpoint1.position, Quaternion.identity, 0);
            shield2 = PhotonNetwork.Instantiate("Sheld", shieldpoint2.position, Quaternion.identity, 0);


            shieldcontrol = false;
            StartCoroutine(Shieldtimer());
            shieldleft--;


        }


        if (shield != null)
        {
            shield.transform.position = shieldpoint1.position;
            shield.transform.rotation = shieldpoint1.rotation;

            shield2.transform.position = shieldpoint2.position;
            shield2.transform.rotation = shieldpoint2.rotation;

        }
    }








    void healthbar()
    {

        if (photonView.isMine)
        {
            LobbyHealthbar.GetComponent<RawImage>().transform.localScale = new Vector3(health / 100, 1, 1);
            LobbyName.GetComponent<Text>().text = PhotonNetwork.playerName;
        }





    }



    void characterCanvas()
    {
        ammotext.text = bulletleft + " /" + totalbullet;
        shieldtext.text = shieldleft + "";
    }




    private void OnTriggerEnter(Collider other)
    {

        if (photonView.isMine)
        {

            if (other.name.Contains("Health"))
            {
                GetComponent<PhotonView>().RPC("takeHealth", PhotonTargets.All, 20);
            }

            else if (other.name.Contains("Ammo"))
            {
                totalbullet += 30;
            }
            else if (other.name.Contains("Space"))
            {
                GetComponent<PhotonView>().RPC("takeDamage", PhotonTargets.All, 100f, "Space");
            }
        }

    }

    [PunRPC]
    void shootingEffect()
    {
        muzzleFlash.Play();

        shotSound.Play();
    }


    [PunRPC]
    void takeDamage(float damage, string taken)
    {
        health -= damage;
        actions.Damage();

        if (health <= 0)
        {
            if (photonView.isMine)
            {
                dead_screen.SetActive(true);
                dead_info.text = "Killed by  " + taken;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            PhotonNetwork.Destroy(gameObject);
        }
    }


    [PunRPC]

    void takeHealth(int healtValue)
    {
        health += healtValue;


        if (health >= 100)
        {
            if (photonView.isMine)
            {
                health = 100;
            }


        }

    }










    [PunRPC]
    void nameV(string thisName)
    {

        Name.text = thisName;


    }



    private IEnumerator Reloadtimer()
    {
        yield return new WaitForSeconds(1.5f);
        reloadtext.text = " ";
        currentbullet -= bulletleft;
        if (totalbullet > currentbullet)
        {
            totalbullet = totalbullet - currentbullet;
        }

        else if (currentbullet > totalbullet)
        {
            lastbullet = bulletleft + totalbullet;

        }
        if (totalbullet < 15)
        {



            bulletleft += currentbullet;

            if (currentbullet > totalbullet)
            {
                bulletleft = lastbullet;
                if (bulletleft >= 15)
                {
                    bulletleft = 15;
                }

                totalbullet = 0;


            }



            if (bulletleft >= 15)
            {
                bulletleft = 15;
            }


        }

        else
        {
            bulletleft = 15;

        }
        currentbullet = 15;
        isRelod = false;
    }
    private IEnumerator Shieldtimer()
    {
        yield return new WaitForSeconds(2);
        PhotonNetwork.Destroy(shield);
        PhotonNetwork.Destroy(shield2);

        shieldcontrol = true;


    }
    private IEnumerator JumpEvent()
    {
        characterController.slopeLimit = 90f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            characterController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;

        } while (!characterController.isGrounded);


        isJumping = false;

        characterController.slopeLimit = 90f;

    }

}

