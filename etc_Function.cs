    //플레이어 터치했는지 판단.
    private bool IsPlayer_Touch(Touch _touch)
    {
        Ray ray_Touch = cam_Main.ScreenPointToRay(_touch.position);    // 터치한 좌표 레이로 바꾸기.

        RaycastHit hit;    // 정보 저장할 구조체 만들고            

        if (Physics.Raycast(ray_Touch, out hit, Mathf.Infinity, nLayerMask))    // 레이저 쏘기.
        {
            if (hit.collider.tag == "Player" && !isMoving)
            {
                return true;          //플레이어 터치함.
            }
            else // 아무것도 없을 시
            {
                return false;         //플레이어 외 모든 터치.
            }
        }

        return false;
    }

    //플레이타임 재는 함수
    private void Count_PlayTime()
    {
        if (bPlay_Time_Count)
        {
            fPlay_Time_Sec += Time.deltaTime;
            fPlay_Time_Sec_Temp += Time.deltaTime;

            if (fPlay_Time_Sec_Temp >= 60)
            {
                fPlay_Time_Sec_Temp = 0;
                fPlay_Time_Min++;
            }
        }
    }

    //플레이중인 스테이지 정보 표시 함수
    private void Display_Game_Info()
    {
        //플레이 시간 출력
        txt_Play_Time.text = fPlay_Time_Min + " : " + string.Format("{0:0.##}", fPlay_Time_Sec_Temp);
        //현재 남은 적 수 / 스테이지 총 적 수 출력
        txt_Enemy_Count.text = Settings_YW.nCount_Enemy + " / " + nTemp_Count_Total_Enemy;
        //던진 횟수 출력
        txt_Throw_Count.text = "" + nTemp_Throw_Count;
        //테마와 스테이지 번호 출력
        txt_StageNumber.text = "" + (Settings_YW.nSelected_Theme + 1) + " - " + (Settings_YW.nSelected_Stage + 1);
    }

    //각종 정보 초기화 함수
    void InitGameInfo()
    {
        //각종 초기화 시작.
        rb_Player = GetComponent<Rigidbody>();
        lr_Pabola = GetComponent<LineRenderer>();
        cam_Main = Camera.main;

        //머터리얼 설정
        transparent.ToonShaderStartFix();

        //선택된 스테이지 번호 설정.
        nTemp_Selected_Stage = Settings_YW.nSelected_Stage + Settings_YW.nSelected_Theme * 10;

        //fBall_PosX = Screen.width * 0.5f;
        //fBall_PosY = cam_Main.WorldToScreenPoint(transform.position).y;

        //씬 시작되자마자는 아웃라인 꺼줌.
        outline.enabled = false;

        //게임 끝날 때 뜨는 팝업 미리 꺼줌.
        go_GameClear.SetActive(false);
        go_GameOver.SetActive(false);

        //코루틴 담기
        c_End_Game = _End_Game(true, 2);

        //공 스텟 할당
        GetStone_Info();

        //부가목표 결정.
        GetSub_Goal();

        //맵 생성
        Creat_Map();

        //적들 개수만큼 임시벡터 생성.
        v3_Prev_Enemy_Pos = new Vector3[Settings_YW.nCount_Enemy];
        for (int i = 0; i < Settings_YW.nCount_Enemy; i++)
        {
            v3_Prev_Enemy_Pos[i] = go_Enemy[i].transform.position;
        }

        //별 개수만큼 임시벡터 생성.
        v3_Prev_Star_Pos = new Vector3[Settings_YW.nStar_Count];
        for (int i = 0; i < Settings_YW.nStar_Count; i++)
        {
            v3_Prev_Star_Pos[i] = go_Star[i].transform.position;
        }


        nTrap_Count_Memory = new int[nTrap_Count.Length];
        if (nTrap_Count_Memory == null)
        {
            return;
        }

        for (int i = 0; i < Settings_YW.nAchiv_Count; i++)
        {
            nAchiv[i] = 0;
        }

        //이번 스테이지의 적 수 담아두기.
        nTemp_Delete_Enemy = Settings_YW.nCount_Enemy;

        bActive_Enemy = new bool[Settings_YW.nCount_Enemy];
        bActive_Star = new bool[Settings_YW.nStar_Count];

        nPinch_Mode_Temp = Settings_YW.nPinchModeMiddle; //Pinch Zoom middle모드

        //플레이 시간 초기화.
        fPlay_Time_Sec = 0;
        fPlay_Time_Min = 0;

        //현재 맵 총 적 수 저장.
        nTemp_Count_Total_Enemy = Settings_YW.nCount_Enemy;

        //던진 횟수 초기화.
        nTemp_Throw_Count = 0;

        //현질부활용 변수들 임시 저장.
        Memory_Value();

        //텍스트 설정
        SetText();

        //옵션 설정
        if (!PlayerPrefs.HasKey(Settings_YW.strOptionDragDir_Horizontal))
        {
            nOptionDragDir_Horizontal = 1;
            PlayerPrefs.SetInt(Settings_YW.strOptionDragDir_Horizontal, nOptionDragDir_Horizontal);
        }
        else
        {
            if (PlayerPrefs.GetInt(Settings_YW.strOptionDragDir_Horizontal) == 1 || PlayerPrefs.GetInt(Settings_YW.strOptionDragDir_Horizontal) == -1)
                nOptionDragDir_Horizontal = PlayerPrefs.GetInt(Settings_YW.strOptionDragDir_Horizontal);
            else
                return;
        }

        if (!PlayerPrefs.HasKey(Settings_YW.strOptionDragDir_Vertical))
        {
            nOptionDragDir_Vertical = 1;
            PlayerPrefs.SetInt(Settings_YW.strOptionDragDir_Vertical, nOptionDragDir_Vertical);
        }
        else
        {
            if (PlayerPrefs.GetInt(Settings_YW.strOptionDragDir_Vertical) == 1 || PlayerPrefs.GetInt(Settings_YW.strOptionDragDir_Vertical) == -1)
                nOptionDragDir_Vertical = PlayerPrefs.GetInt(Settings_YW.strOptionDragDir_Vertical);
            else
                return;
        }

        if (!PlayerPrefs.HasKey(Settings_YW.strOptionDragScale))
        {
            fOptionDragScale = 1;
            PlayerPrefs.SetFloat(Settings_YW.strOptionDragScale, fOptionDragScale);
        }
        else
        {
            if (PlayerPrefs.GetFloat(Settings_YW.strOptionDragScale) >= 0.1f || PlayerPrefs.GetFloat(Settings_YW.strOptionDragScale) < 2)
                fOptionDragScale = PlayerPrefs.GetFloat(Settings_YW.strOptionDragScale);
            else
                return;
        }

        //옵션 설정 끝
        Settings_YW.isCommon_PopUp = false;
    }

    //플레이어 세팅 함수.
    private void GetStone_Info()
    {
        //공 외형 설정.
        mf_Player.mesh = mesh_Player[Settings_YW.nSelected_Stone];
        mr_Player.material = mat_Player_[Settings_YW.nSelected_Stone];

        //공에 맞는 파티클 설정
        ps_Crush = ps_Cruch_[Settings_YW.nSelected_Stone];

        int nSeleted_Stone = int.Parse(ServerData.server.GetTableContents("LastStone", "character"));

        for (int i = 2; i < 7; i++)
        {
            _List_Stone_Status[i - 2] = ServerData.RockInfoList[nSeleted_Stone, i];
        }

        //파워
        fMaxPower = 270 + 12.5f * int.Parse(_List_Stone_Status[Settings_YW.nStatus_Power]);
        //무게
        //rb_Player.mass = 2 + int.Parse(_List_Stone_Status[Settings_YW.nStatus_Mass]);
        rb_Player.mass = 3;
        //마찰력
        rb_Player.angularDrag = 3 + 2 * int.Parse(_List_Stone_Status[Settings_YW.nStatus_Friction]);
        //탄성
        pm_Player.bounciness = 0.2f * int.Parse(_List_Stone_Status[Settings_YW.nStatus_Elasticity]);
        //각도
        nAngle_Min = (int.Parse(_List_Stone_Status[Settings_YW.nStatus_Angle]) * 25) + 1;
        nAngle_Max = (int.Parse(_List_Stone_Status[Settings_YW.nStatus_Angle]) * 25) + 30;
        fAngle = nAngle_Min + ((nAngle_Max - nAngle_Min) / 2);
    }

    //맵생성
    private void Creat_Map()
    {
        //Excel Map_Info = new Excel();

        //내가 맵 제작할 때 쓰는 코드
        //if (!Settings_YW.isBuildVer)
        //{
        //    Excel Map_Info = new Excel();
        //    string strMapExcel = "Assets\\Excel\\Theme_" + ServerData.server.GetTableContents("LastTheme", "character") + ".xlsx";
        //    Manager_Create_Map.instance.CreateMap(Map_Info.Read(strMapExcel, Settings_YW.nSelected_Stage), int.Parse(_List_Stone_Status[Settings_YW.nStatus_Mass]));
        //}
        //else
        {
            //ServerData에서 맵정보 불러오는 코드
            string[] dfdf = new string[1600];
            if (ServerData.MapInfo[nTemp_Selected_Stage] == null)
            {
                dfdf = ServerData.MapInfo[nTemp_Selected_Stage] = ServerData.server.GetMapInfo(nTemp_Selected_Stage);
            }
            else
            {
                for (int i = 0; i < 1600; i++)
                {
                    dfdf[i] = ServerData.MapInfo[nTemp_Selected_Stage][i];
                }
            }
            Manager_Create_Map.instance.CreateMap(dfdf, int.Parse(_List_Stone_Status[Settings_YW.nStatus_Mass]));
        }
        //서버에 맵 업로드 할 때 쓰는 코드. 
        //int nLastStage = int.Parse(ServerData.server.GetTableContents("LastStage", "character"));
        //for (int i = 0; i < 10; i++)
        //{
        //    for (int j = 0; j < 5; j++)
        //    {
        //        string strMapExcel = "Assets\\Excel\\Theme_" + j + ".xlsx";
        //        ServerData.server.InsertStageInfo(j*10 + i, Map_Info.Read(strMapExcel, i));
        //    }
        //}
    }

    //부가목적 불러오는 함수.
    private void GetSub_Goal()
    {
        string[] strSubGoal = new string[Settings_YW.nSub_Goal_Count];
        string[] strTemp = new string[Settings_YW.nSub_Goal_Count];

        if (ServerData.SubGoalList[nTemp_Selected_Stage] == null)
        {
            strTemp = ServerData.SubGoalList[nTemp_Selected_Stage] = ServerData.server.GetSubGoalInfo(nTemp_Selected_Stage);
        }
        else
        {
            for (int i = 0; i < Settings_YW.nSub_Goal_Count; i++)
            {
                strTemp[i] = ServerData.SubGoalList[nTemp_Selected_Stage][i];
            }
        }

        int nSize = strTemp.Length;
        int nCount = 0;

        for (int i = 0; i < nSize; i++)
        {
            if (int.Parse(strTemp[i]) >= 0)
            {
                Goal_Index[nCount] = i;
                Goal_Value[nCount] = int.Parse(strTemp[i]);
                nCount++;
            }
        }
        //CSV로 언어별로 받아와야함. -1은 Flag
        for (int i = 0; i < Settings_YW.nSub_Goal_Count; i++)
        {
            strSubGoal[i] = Manager_Function_dh.Read(i, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));
        }        

        txt_Sub_Goal[0].text = Manager_Function_dh.Read(10, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));
        for (int i = 1; i < 3; i++)
        {
            strSubGoal[Goal_Index[i - 1]] = strSubGoal[Goal_Index[i - 1]].Replace("-1", Goal_Value[i - 1].ToString());
            txt_Sub_Goal[i].text = strSubGoal[Goal_Index[i - 1]];
        }

    }

    //부가목적 함수.
    private void Stage_Goal(int goal_Index, int goal_Value)
    {
        switch (goal_Index)
        {
            case 0: //공 던진 횟수 체크
                if (nTemp_Throw_Count < goal_Value)
                    nTemp_Star_Count++;
                break;
            case 1: //던진 횟수를 조금 더 타이트하게 제한.
                if (nTemp_Throw_Count < goal_Value)
                    nTemp_Star_Count++;
                break;
            case 2: //N회에 맞춰 던져서 클리어.
                if (nTemp_Throw_Count == goal_Value)
                    nTemp_Star_Count++;
                break;
            case 3: //적 숫자와 동일한 횟수로 던져서 클리어.
                if (nTemp_Throw_Count == go_Enemy.Count)
                    nTemp_Star_Count++;
                break;
            case 4: //필드에 모든 별 먹기.
                if (nTemp_Star_Eat == go_Star.Count)
                    nTemp_Star_Count++;
                break;
            case 5: //함정 안 밟고 클리어.
                if (nTemp_Trap_Count <= 0)
                    nTemp_Star_Count++;
                break;
            case 6: //함정 N회 밟고 클리어.
                if (nTemp_Trap_Count == goal_Value)
                    nTemp_Star_Count++;
                break;
            case 7: //적을 물에 빠트려 죽이기.
                if (isWater)
                    nTemp_Star_Count++;
                break;
        }
    }

    public void Memory_Value()
    {
        //플레이어 위치 임시저장.
        v3_Prev_Player_Pos = transform.position;
        //시점 임시 저장.
        fPrev_CurrentX = currentX;
        fPrev_CurrentY = currentY;

        //오브젝트 활성상황 저장.
        for (int i = 0; i < go_Enemy.Count; i++)
        {
            bActive_Enemy[i] = go_Enemy[i].activeSelf;
        }
        for (int i = 0; i < go_Star.Count; i++)
        {
            bActive_Star[i] = go_Star[i].activeSelf;
        }

        //적 오브젝트들 위치값 임시 저장.
        for (int i = 0; i < go_Enemy.Count; i++)
        {
            if (!go_Enemy[i].activeSelf)
                continue;
            else
                v3_Prev_Enemy_Pos[i] = go_Enemy[i].transform.position;
        }
        //별 오브젝트들 위치값 임시 저장.
        for (int i = 0; i < go_Star.Count; i++)
        {
            if (!go_Star[i].activeSelf)
                continue;
            else
                v3_Prev_Star_Pos[i] = go_Star[i].transform.position;
        }

        //먹은 별 개수 임시 저장.
        nTemp_Star_Eat_Memory = nTemp_Star_Eat;
        //돌 던진 횟수 임시 저장.
        nTemp_Throw_Count_Memory = nTemp_Throw_Count;
        //함정 밟은 횟수 임시 저장.
        nTemp_Trap_Count_Memory = nTemp_Trap_Count;
        //함정 종류별로 밟은 횟수 임시 저장.
        int nSize = nTrap_Count.Length;
        for (int i = 0; i < nSize; i++)
        {
            nTrap_Count_Memory[i] = nTrap_Count[i];
        }

        //플레이 타이머 임시 저장.
        fPlay_Time_Sec_Memory = fPlay_Time_Sec;
        fPlay_Time_Sec_Temp_Memory = fPlay_Time_Sec_Temp;
        fPlay_Time_Min_Memory = fPlay_Time_Min;
    }

    private void Set_Achiv_Date(bool isClear)
    {
        if (isClear)
        {
            //업적값 갱신해주기.
            nAchiv[1] = nTemp_Throw_Count;                     //공 던진 횟수 업로드
            nAchiv[2] = nTemp_Delete_Enemy;                    //적 죽인 횟수 업로드
            nAchiv[4] = nTrap_Count[Settings_YW.nTrap_Sticky]; //끈끈이 밟은 횟수 업로드.
            nAchiv[5] = nTrap_Count[Settings_YW.nTrap_Oil];    //기름 밟은 횟수 업로드.
            nAchiv[6] = nTrap_Count[Settings_YW.nTrap_Spring]; //스프링 밟은 횟수 업로드.
            nAchiv[7] = nTrap_Count[Settings_YW.nTrap_Portal]; //포탈 밟은 횟수 업로드.
            nAchiv[8] = nTemp_Rebirth_Count;                   //재화부활한 횟수 횟수 업로드.
            nAchiv[Settings_YW.nSelected_Stone + 14] = 1;               //공 플레이 횟수 업로드.
        }

        else
        {
            nAchiv[3] = nTemp_Death_Count;                     //죽은 횟수 업로드.
            nAchiv[8] = nTemp_Rebirth_Count;                   //재화부활한 횟수 업로드.
        }
    }

    private void UIOff()
    {
        go_Mini_Menu.SetActive(false);
        go_Star_List.SetActive(false);
        go_Pinch_List.SetActive(false);
        
    }

    private void SetText()
    {
        txt_Throw_Count_Explain.text = Manager_Function_dh.Read(9, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));

        txt_BTN_Back_[0].text = Manager_Function_dh.Read(21, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));
        txt_BTN_Back_[1].text = Manager_Function_dh.Read(21, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));
        txt_BTN_Retry_[0].text = Manager_Function_dh.Read(22, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));
        txt_BTN_Retry_[1].text = Manager_Function_dh.Read(22, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));
        txt_BTN_NextStage.text = Manager_Function_dh.Read(23, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));
        txt_BTN_RebirthPiece.text = Manager_Function_dh.Read(24, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));
        txt_BTN_RebirthGold.text = Manager_Function_dh.Read(25, 5, PlayerPrefs.GetInt(Settings_dh.strLangaugeIndex));
    }
