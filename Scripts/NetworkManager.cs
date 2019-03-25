using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : Photon.MonoBehaviour
{
    #region Values
    public InputField oyuncu_Adı;
    public InputField oda_Adı;
    public InputField max_Oyuncu;
    public Toggle oda_Gorunurluk;
    public Toggle oda_Gırılebılme;
    public GameObject[] spawnerlar;
    public GameObject lobi_Ekrani;
    public GameObject lobi_Kamerasi;
    public GameObject olum_ekrani;
    public Text olum_bilgisi;
    public InputField oda_listesi;
    public GameObject[] ammoPoints;
    public GameObject[] healthPoints;
    public GameObject healthBar;
    public GameObject staminaBar;
    public GameObject[] Ammo;
    public GameObject[] Health;
    int ammoSpawnCheck = 0;
    int healthSpawnCheck = 0;
    bool isOnGame = false;
    float Timer = 3;
    #endregion
    private void Awake()
    {
        Ammo = GameObject.FindGameObjectsWithTag("Ammo");
        Health = GameObject.FindGameObjectsWithTag("Health");
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.autoJoinLobby = true;

    }

    private void Update()
    {
        if (isOnGame)
        {
            collectableItems();


        }
    }

    void OnJoinedRoom()
    {
        isOnGame = true;
        lobi_Ekrani.SetActive(false);
        lobi_Kamerasi.SetActive(false);
        healthBar.SetActive(true);
        staminaBar.SetActive(true);
        PhotonNetwork.playerName = oyuncu_Adı.text;
        int r = Random.Range(0, spawnerlar.Length);
        PhotonNetwork.Instantiate("Sci-Fi_Soldier", spawnerlar[r].transform.position, transform.rotation, 0);



        Debug.Log("Joined Room" + "" + PhotonNetwork.room.Name + " " + PhotonNetwork.playerName + " " + PhotonNetwork.room.MaxPlayers + "visibilty:" + oda_Gorunurluk.isOn + "girilebilme:" + oda_Gırılebılme.isOn);
    }

    public void JoinedRoom()
    {
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = (byte)int.Parse(max_Oyuncu.text),
            IsVisible = oda_Gorunurluk.isOn,
            IsOpen = oda_Gırılebılme.isOn

        };

        PhotonNetwork.JoinOrCreateRoom(oda_Adı.text, roomOptions, TypedLobby.Default);

    }


    public void spawnButton()
    {
        olum_ekrani.SetActive(false);
        int r = Random.Range(0, spawnerlar.Length);
        PhotonNetwork.Instantiate("Sci-Fi_Soldier", spawnerlar[r].transform.position, transform.rotation, 0);
    }
    public void QuitButton()
    {
        Application.Quit();
    }

    void collectableItems()
    {


        foreach (GameObject y in Ammo)
        {


            if (y.activeSelf == false)
            {
                Timer -= Time.deltaTime;

                if (Timer <= 0)
                {


                    y.SetActive(true);
                    y.GetComponent<PhotonView>().RPC("ActiveAmmo", PhotonTargets.All, true);
                    Timer = 3;

                }

            }
        }



        foreach (GameObject x in Health)
        {


            if (x.activeSelf == false)
            {
                Timer -= Time.deltaTime;

                if (Timer <= 0)
                {

                    x.SetActive(true);
                    x.GetComponent<PhotonView>().RPC("ActiveHealth", PhotonTargets.All, true);
                    Timer = 3;

                }

            }
        }





    }



    [PunRPC]
    void ActiveAmmo(bool setActive)
    {

        foreach (GameObject y in Ammo)
        {
            y.SetActive(setActive);
        }
    }
    [PunRPC]
    void ActiveHealth(bool setActive)
    {

        foreach (GameObject x in Health)
        {
            x.SetActive(setActive);
        }
    }

}
