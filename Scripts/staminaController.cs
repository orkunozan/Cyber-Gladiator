using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class staminaController : MonoBehaviour
{
    public RawImage staminaBar;
    public static float stamina;
    public static bool isOverStamina;


    void Awake()
    {
        stamina = 100;
    }


    void Update()
    {
        if (!isOverStamina)
        {
            staminabar();
        }
        staminabarManager();
    }


    void staminabar()
    {

        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            if (stamina == 0)
            {
                karakter.movementSpeed = 6;

            }
            else
            {

                karakter.movementSpeed = 10;

            }


        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {

            karakter.movementSpeed = 6;

        }

    }


    void staminabarManager()
    {
        if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            if (!isOverStamina)
            {
                stamina -= 10 * Time.deltaTime;
                staminaBar.rectTransform.localScale = new Vector3(stamina / 70.2f, 1.4225f, 1.4225f);

            }
            else
            {
                stamina += 30 * Time.deltaTime;
                if (stamina > 100)
                {
                    stamina = 100;

                }
                staminaBar.rectTransform.localScale = new Vector3(stamina / 70.2f, 1.4225f, 1.4225f);


            }

            if (stamina < 0)
            {
                stamina = 0;
                isOverStamina = true;

                karakter.movementSpeed = 6;

            }
        }
        else
        {
            stamina += 30 * Time.deltaTime;

            if (stamina > 100)
            {
                stamina = 100;

                isOverStamina = false;
            }

            staminaBar.rectTransform.localScale = new Vector3(stamina / 70.2f, 1.4225f, 1.4225f);


        }
    }

}