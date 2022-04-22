using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : MonoBehaviour
{
    public Material mat_Transparent;
    public Material mat_Ground;
    public Material mat_Gloss;
    public Material mat_Slime;
    public Material[] mat_Character;
    public Material mat_Cloud;
    public Material mat_Enemy;

    public Shader sdToon, sdNotToon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            return;
        if (other.tag == "Player")
            return;
        if (other.tag == "Star")
            return;
        if (other.tag == "Pass")
            return;

        other.gameObject.GetComponent<MeshRenderer>().material = mat_Transparent;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Water")
            return;
        if (other.tag == "Player")
            return;
        if (other.tag == "Star")
            return;
        if (other.tag == "Pass")
            return;
        if (other.tag == "Ground")
            other.gameObject.GetComponent<MeshRenderer>().material = mat_Ground;
        else if (other.tag == "Trap_Gloss")
            other.gameObject.GetComponent<MeshRenderer>().material = mat_Gloss;
        else if (other.tag == "Trap_Slime")
            other.gameObject.GetComponent<MeshRenderer>().material = mat_Slime;
        else
            return;
    }

    public void ToonShaderOption()
    {
        if (!PlayerPrefs.HasKey(Settings_dh.strGraphicOption))
        {
            print("못가져옴");
            return;
        }
        

        if (PlayerPrefs.GetInt(Settings_dh.strGraphicOption) == 1)
        {
            mat_Ground.shader = sdNotToon;
            mat_Cloud.shader = sdNotToon;
            mat_Enemy.shader = sdNotToon;

            int nSize = mat_Character.Length;
            for (int i = 0; i < nSize; i++)
            {
                mat_Character[i].shader = sdNotToon;
            }

            PlayerPrefs.SetInt(Settings_dh.strGraphicOption, 0);
        }
        else
        {
            mat_Ground.shader = sdToon;
            mat_Cloud.shader = sdToon;
            mat_Enemy.shader = sdToon;

            int nSize = mat_Character.Length;
            for (int i = 0; i < nSize; i++)
            {
                mat_Character[i].shader = sdToon;
            }

            PlayerPrefs.SetInt(Settings_dh.strGraphicOption, 1);
        }
    }

    public void ToonShaderStartFix()
    {
        if (!PlayerPrefs.HasKey(Settings_dh.strGraphicOption))
        {
            print("못가져옴");
            return;
        }

        if (PlayerPrefs.GetInt(Settings_dh.strGraphicOption) == 1)
        {
            mat_Ground.shader = sdToon;
            mat_Cloud.shader = sdToon;
            mat_Enemy.shader = sdToon;

            int nSize = mat_Character.Length;
            for (int i = 0; i < nSize; i++)
            {
                mat_Character[i].shader = sdToon;
            }
        }
        else
        {
            mat_Ground.shader = sdNotToon;
            mat_Cloud.shader = sdNotToon;
            mat_Enemy.shader = sdNotToon;

            int nSize = mat_Character.Length;
            for (int i = 0; i < nSize; i++)
            {
                mat_Character[i].shader = sdNotToon;
            }
        }
    }
}
