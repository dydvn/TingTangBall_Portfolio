    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
                {
                    isTouch_Player = IsPlayer_Touch(Input.GetTouch(0));

                    if (isTouch_Player && !isPinch_Zoom_Out)
                    {
                        //첫 터치한 지점의 위치 받기.
                        //v2Start_Pos = Input.GetTouch(0).position;
                        v2Start_Pos = cam_Main.WorldToScreenPoint(transform.position);

                        //드래그용 UI 첫 포인트 지정.
                        Manager_Ball_Drag.Instance.Set_Start_Position(v2Start_Pos);

                        //핀치줌 버튼 사라짐
                        go_Pinch_List.SetActive(false);
                        go_Pinch_List.transform.parent.gameObject.SetActive(false);

                        //플레이타임 측정 시작.
                        bPlay_Time_Count = true;
                    }
                    else
                    {
                        v2Start_Pos = Input.GetTouch(0).position;
                        UIOff();
                        go_Pinch_List.transform.parent.gameObject.SetActive(false);
                    }

                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    v2Present_Pos = Input.GetTouch(0).position;
                    fDistance = Vector3.Distance(v2Start_Pos, v2Present_Pos);

                    fDirectionX = (v2Start_Pos.x - v2Present_Pos.x) / fDistance;
                    fDirectionY = (v2Start_Pos.y - v2Present_Pos.y) / fDistance;
                    fDirectX = v2Start_Pos.x - v2Present_Pos.x;
                    fDirectY = v2Start_Pos.y - v2Present_Pos.y;

                    if (isTouch_Player && !isPinch_Zoom_Out)
                    {
                        //처음 점과 현재 점 사이의 거리 계산해주기
                        fPower_Distance = Mathf.Clamp((fDistance - 100) * fDrag_Scale, 0, fMaxPower);

                        //방향벡터 보정 임시구현 시작.
                        v3_direction.x = fDirectX * Mathf.Cos(cam_Main.transform.eulerAngles.y * Mathf.Deg2Rad) + fDirectY * Mathf.Sin(cam_Main.transform.eulerAngles.y * Mathf.Deg2Rad);
                        v3_direction.z = fDirectY * Mathf.Cos(cam_Main.transform.eulerAngles.y * Mathf.Deg2Rad) - fDirectX * Mathf.Sin(cam_Main.transform.eulerAngles.y * Mathf.Deg2Rad);
                        //방향벡터 보정 임시구현 끝.

                        //일정 거리 이상 당기면
                        if (fDistance > 200)
                        {
                            //당기는 UI 그려주기 시작.
                            Manager_Ball_Drag.Instance.On_UI();
                            Manager_Ball_Drag.Instance.Set_End_Position(v2Present_Pos);
                            Manager_Ball_Drag.Instance.Draw_Line();
                            //당기는 UI 그려주기 끝.

                            outline.outlineColor = clr_Outline[nTouch_Out];
                            outline.needsUpdate = true;
                            isBall_In = false;

                            //포물선 그려주렴.
                            isRenderArch = true;
                        }
                        else
                        {
                            outline.outlineColor = clr_Outline[nStop];
                            outline.needsUpdate = true;
                            isBall_In = true;
                            isRenderArch = false;

                            lr_Pabola.SetVertexCount(0);

                            //드래그 UI 꺼주기
                            Manager_Ball_Drag.Instance.Off_UI();
                        }
                    }
                    else
                    {
                        currentX += -fDirectionX * 2 * nOptionDragDir_Horizontal * fOptionDragScale;
                        currentY += fDirectionY * 2 * nOptionDragDir_Vertical * fOptionDragScale;

                        currentY = Mathf.Clamp(currentY, 10, 40);
                    }

                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    if (isTouch_Player && !isPinch_Zoom_Out)
                    {
                        //공통부분
                        isTouch_Player = false;

                        //드래그 UI 꺼주기
                        Manager_Ball_Drag.Instance.Off_UI();

                        //포물선 그려주지마
                        isRenderArch = false;

                        //Area_Shoot
                        //던질때
                        if (!isBall_In)
                        {
                            //현질부활용 변수들 임시 저장
                            if (!bNoSave)
                                Memory_Value();

                            //운동 가능하게 하기.
                            if (rb_Player.isKinematic)
                                rb_Player.isKinematic = false;

                            //뗀 순간의 날아갈 방향 적용해서 Impulse에 적용해주기.
                            Manager_Ball_Control.instance.ShotBall(rb_Player, fPower_Distance, fAngle, v3_direction);

                            //achievement.SetAchieve(THROWBALL);

                            //던진 횟수 증가.
                            nTemp_Throw_Count++;


                            //핀치줌 버튼 사라짐
                            go_Pinch_List.SetActive(false);

                            //포물선 지워주기
                            lr_Pabola.SetVertexCount(0);

                            //움직임 감지 코루틴 시작
                            if (!isBall_In)
                                StartCoroutine(PlayerMoveDetecting());
                        }
                        //안던질때
                        else
                        {
                            go_Pinch_List.transform.parent.gameObject.SetActive(true);
                        }

                        //각도 중간값으로 다시 세팅. 이건 맨 밑에 있어야함
                        fAngle = nAngle_Min + ((nAngle_Max - nAngle_Min) / 2);
                    }
                    else
                    {
                        go_Pinch_List.transform.parent.gameObject.SetActive(true);
                    }

                    fDirectionX = fDirectionY = 0;
                }
            }

            if (isRenderArch)
            {
                pointsList.Clear();
                Manager_Ball_Control.instance.RenderArc(gameObject.transform, fPower_Distance, rb_Player.mass, fAngle, v3_direction, out hitinfo, out pointsList);

                if (pointsList.Count != 0)
                    Manager_Ball_Control.instance.DrawLine(lr_Pabola, pointsList);
            }
        }
    }
    
    
    
    
    private bool IsPlayer_Touch(Touch _touch)
    {
        Ray ray_Touch = cam_Main.ScreenPointToRay(_touch.position);    // 터치한 좌표 레이로 바꾸기.

        RaycastHit hit;    // 정보 저장할 구조체 만들고            

        if (Physics.Raycast(ray_Touch, out hit, Mathf.Infinity, nLayerMask))    // 레이저 쏘기.
        {
            if (hit.collider.tag == "Player" && !isMoving)  //플레이어 터치함.
                return true;
            else // 아무것도 없을 시
                return false;         //플레이어 외 모든 터치.
        }

        return false;
    }
