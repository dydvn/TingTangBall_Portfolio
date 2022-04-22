using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Create_Map : MonoBehaviour
{
    public Manager_Play_Scene manager_Play_Scene;

    public Vector3 v3_Size_Tile;
    public GameObject go_Player;
    public GameObject go_Star;
    public GameObject[] go_Tile_;
    public GameObject[] go_Grass_;
    public GameObject[] go_Rock_;
    public GameObject[] go_Enemy_;
    public GameObject[] go_Trap_;

    public int nMax_Width;
    public int nMax_Depth;

    public float fTile_Size;

    private GameObject go_Tile_Temp;
    private Vector3 v3Test;
    private Vector3 v3PlayerPostion;
    private Vector3 v3_Enemy_Postion;
    private string strPotal_Numbering;
    //임시 시작.
    private GameObject dddd;

    private string[] strTileMoving = new string[4];
    private bool isminus;
    //임시 끝.

    private static Manager_Create_Map _instance = null;
    public static Manager_Create_Map instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Manager_Create_Map();
            }
            return _instance;
        }
    }
    void Awake()
    {
        _instance = this;
    }
    
    //맵에 관한 문자열 배열을 인자로 받아온다.
    public void CreateMap(string[] _MapInfoArr, int nMass_Lv)
    {
        //임시 시작.
        dddd = GameObject.Find("Temp(Clone)");
        //임시 끝

        //적 개수 초기화.
        Settings_YW.nCount_Enemy = 0;
        Settings_YW.nStar_Count = 0;

        fTile_Size = 8.1f;

        int nWidth = 40;
        int nHeight = 40;
        int nTemp_k = 0;    //층수
        string strTemp;
        string strTemp_K = null;

        for (int j = nHeight - 1; j >= 0; j--)
        {
            for (int i = 0; i < nWidth; i++)
            {
                v3Test.x = i * fTile_Size;
                nTemp_k = 0;
                v3Test.z = j * -fTile_Size;

                strTemp = _MapInfoArr[j * nWidth + i];

                if (strTemp == "0")
                    continue;
                for (int k = 0; k < strTemp.Length; k++)
                {
                    if (strTemp[k] == '0')
                    {
                        if (k > 0)
                        {
                            nTemp_k++;
                        }

                        continue;
                    }

                    if (strTemp[k] == '/')
                    {
                        strTemp_K = null;
                        if (k != 0)
                            nTemp_k++;

                        while (true)
                        {
                            k++;

                            if (strTemp[k] == '/')
                                break;

                            strTemp_K += strTemp[k].ToString();
                        }

                        v3Test.y = nTemp_k * fTile_Size;
                        go_Tile_Temp = Instantiate(go_Tile_[int.Parse(strTemp_K) - 1]);
                        go_Tile_Temp.transform.position = v3Test;
                        go_Tile_Temp.transform.SetParent(dddd.transform);
                        continue;
                    }

                    if (strTemp[k] == '[')
                    {
                        if (strTemp[k + 1] != '/')
                            nTemp_k++;

                        while (true)
                        {
                            k++;

                            if (strTemp[k] == ']')
                                break;

                            strTemp_K = strTemp[k].ToString();

                            if (strTemp[k] == '/')
                            {
                                strTemp_K = null;
                                nTemp_k++;

                                while (true)
                                {
                                    k++;

                                    if (strTemp[k] == '/')
                                        break;

                                    strTemp_K += strTemp[k].ToString();
                                }
                            }

                            v3Test.y = nTemp_k * fTile_Size;
                            go_Tile_Temp = Instantiate(go_Enemy_[int.Parse(strTemp_K)]);

                            //공 종류에 따라 튕겨 나가는 정도 다르게 설정
                            go_Tile_Temp.GetComponent<Manager_Enemy>().fBounce = 1 - (int.Parse(strTemp_K) * 0.5f) + nMass_Lv;

                            go_Tile_Temp.transform.position = v3Test;
                            go_Tile_Temp.transform.SetParent(dddd.transform);
                            manager_Play_Scene.go_Enemy.Add(go_Tile_Temp);
                            Settings_YW.nCount_Enemy++;
                        }
                        continue;
                    }

                    if (strTemp[k] == '!')
                    {
                        if (strTemp[k + 1] != '/')
                        {
                            if (nTemp_k != 0)
                                nTemp_k++;

                            else if (k > 0)
                            {
                                nTemp_k++;
                                v3Test.y = nTemp_k * fTile_Size;
                            }
                        }

                        while (true)
                        {
                            k++;

                            if (strTemp[k] == '!')
                                break;
                            
                            strTemp_K = strTemp[k].ToString();

                            if (strTemp[k] == '_')
                            {
                                k++;
                                continue;
                            }

                            if (strTemp[k + 1] == '_')
                            {
                                strPotal_Numbering = strTemp[k + 2].ToString();
                            }

                            if (strTemp[k] == '/')
                            {
                                strTemp_K = null;
                                nTemp_k++;

                                while (true)
                                {
                                    k++;

                                    if (strTemp[k] == '/')
                                        break;

                                    strTemp_K += strTemp[k].ToString();
                                }
                            }

                            v3Test.y = nTemp_k * fTile_Size;
                            go_Tile_Temp = Instantiate(go_Trap_[int.Parse(strTemp_K)]);
                            go_Tile_Temp.name = go_Tile_Temp.name + strPotal_Numbering;
                            go_Tile_Temp.transform.position = v3Test;
                            go_Tile_Temp.transform.SetParent(dddd.transform);
                        }
                        continue;
                    }

                    v3Test.y = nTemp_k * fTile_Size;

                    if (strTemp[k] == 'P')
                    {
                        nTemp_k++;
                        v3Test.y = nTemp_k * fTile_Size;
                        go_Player.transform.position = v3Test;
                        continue;
                    }

                    if (strTemp[k] == 'S')
                    {
                        nTemp_k++;
                        v3Test.y = nTemp_k * fTile_Size;
                        go_Tile_Temp = Instantiate(go_Star);
                        go_Tile_Temp.transform.position = v3Test;
                        go_Tile_Temp.transform.SetParent(dddd.transform);
                        manager_Play_Scene.go_Star.Add(go_Tile_Temp);
                        Settings_YW.nStar_Count++;
                        continue;
                    }

                    if (strTemp[k] == 'G')
                    {
                        v3Test.y = nTemp_k * fTile_Size;
                        go_Tile_Temp = Instantiate(go_Grass_[Random.Range(0, go_Grass_.Length)]);
                        go_Tile_Temp.transform.position = v3Test;
                        go_Tile_Temp.transform.SetParent(dddd.transform);
                        continue;
                    }

                    if (strTemp[k] == 'R')
                    {
                        v3Test.y = nTemp_k * fTile_Size;
                        go_Tile_Temp = Instantiate(go_Rock_[Random.Range(0, go_Rock_.Length)]);
                        go_Tile_Temp.transform.position = v3Test;
                        go_Tile_Temp.transform.SetParent(dddd.transform);
                        continue;
                    }

                    if (strTemp[k] != ']')
                    {
                        if (nTemp_k != 0)
                        {
                            nTemp_k++;
                            v3Test.y = nTemp_k * fTile_Size;
                        }
                        else if (k > 0)
                        {
                            nTemp_k++;
                            v3Test.y = nTemp_k * fTile_Size;
                        }
                        
                        if (strTemp[k] == '4')
                        {
                            strTemp_K = null;
                            strTemp_K = strTemp[k].ToString();

                            for (int x = 0; x < 4; x++)
                            {
                                k += 2;
                                if (strTemp[k] == '-')
                                {
                                    k++;
                                    isminus = true;
                                }

                                strTileMoving[x] = strTemp[k].ToString();
                            }
                            k++;


                            go_Tile_Temp = Instantiate(go_Tile_[int.Parse(strTemp_K) - 1]);
                            go_Tile_Temp.transform.position = v3Test;
                            go_Tile_Temp.transform.SetParent(dddd.transform);

                            Tile_Moving tile_Moving = go_Tile_Temp.GetComponent<Tile_Moving>();
                            if(tile_Moving == null)
                            {
                                print("스크립트 못받아옴");
                                return;
                            }
                            tile_Moving.strDirection = strTileMoving[0];
                            if (isminus)
                                tile_Moving.nDistance = -int.Parse(strTileMoving[1]);
                            else
                                tile_Moving.nDistance = int.Parse(strTileMoving[1]);
                            tile_Moving.nSpeed = int.Parse(strTileMoving[2]);
                            tile_Moving.nDelay = int.Parse(strTileMoving[3]);
                            isminus = false;    
                        }
                        else if(strTemp[k] == '5')
                        {
                            go_Tile_Temp = Instantiate(go_Tile_[int.Parse(strTemp[k].ToString()) - 1]);

                            k += 2;
                            strTemp_K = null;
                            strTemp_K = strTemp[k].ToString();
                            int nAngle = -1;
                            nAngle = int.Parse(strTemp_K);
                            k++;
                            go_Tile_Temp.transform.rotation  = Quaternion.Euler(0, 90 * nAngle, 0);
                            go_Tile_Temp.transform.position = v3Test;
                            go_Tile_Temp.transform.SetParent(dddd.transform);
                        }
                        else
                        {
                            go_Tile_Temp = Instantiate(go_Tile_[int.Parse(strTemp[k].ToString()) - 1]);
                            go_Tile_Temp.transform.position = v3Test;
                            go_Tile_Temp.transform.SetParent(dddd.transform);
                        }
                    }
                }
            }
        }
    }
}
