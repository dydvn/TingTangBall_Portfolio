❗ TingTangBall 프로젝트에서 제가 작성했고, 불필요한 부분은 제거한 스크립트를 올리는 리포지토리입니다. ❗

🎮 Android download link : 현재 스토어에서 내려간 상태입니다.😅


------------------------------------------------------------------------

Release date : 2019.08

Platform : Mobile (Google play)

다운로드 수 : 300 이상

------------------------------------------------------------------------


🛠 저는 이 게임에서 대표적으로 이런 걸 구현했습니다!

- 플레이어 조작
- Excel을 이용한 맵 생성 알고리즘
- 플레이어와 카메라 사이의 오브젝트 투명화
- 파티클, 카메라 흔들림, 물리적 작동
- Canvas의 전반적인 구성, 버튼의 각 기능

------------------------------------------------------------------------

🛠 플레이어 조작



https://user-images.githubusercontent.com/62327209/232223670-588317f3-3b81-42e4-8667-074bdd6b3cfb.mp4




- 공(플레이어)을 터치 후 당기면 당기고 있는 방향 UI와 포물선이 등장합니다.
- Code - [https://github.com/dydvn/TingTangBall/blob/main/BallControl.cs](https://github.com/dydvn/TingTangBall/blob/main/BallControl.cs)


------------------------------------------------------------------------

🛠 Excel을 이용한 맵 생성 알고리즘


![1](https://user-images.githubusercontent.com/62327209/232223090-8d740699-f326-4956-b58b-7880a7ed57ff.png)

Excel에 맵 정보를 입력 후 Unity에서 Excel importer maker를 이용해 읽어 들입니다.

![2](https://user-images.githubusercontent.com/62327209/232223100-0e361e5d-bdbe-43c8-a859-2efd9967bd8e.png)

위 정보로 생성한 맵 전경입니다.

- 엑셀에 기입된 문자열을 이용하여 맵을 생성합니다.
- 예를 들어 0이면 공백, 1이면 바닥타일, 3이면 나무타일입니다. 오른쪽으로 갈 수록 층이 증가합니다.
- Code - [https://github.com/dydvn/TingTangBall/blob/main/CreateMap.cs](https://github.com/dydvn/TingTangBall/blob/main/CreateMap.cs)

------------------------------------------------------------------------

🛠 플레이어와 카메라 사이의 오브젝트 투명화

![3](https://user-images.githubusercontent.com/62327209/232223219-c14d7017-5d39-4b1f-ab2c-d2dd1655fea3.png)
![4](https://user-images.githubusercontent.com/62327209/232223222-9f0220ae-6f63-4711-b965-8b981cdde674.png)


- 카메라와 플레이어 사이에 왼쪽과 같은 콜라이더 배치 후 콜라이더와 닿는 오브젝트는 투명한 머터리얼을 적용하도록 구현했습니다.
- Code - [https://github.com/dydvn/TingTangBall/blob/main/Transparent.cs](https://github.com/dydvn/TingTangBall/blob/main/Transparent.cs)


https://user-images.githubusercontent.com/62327209/232223748-b41d5747-d919-4e42-89b7-1dea1df07deb.mp4



------------------------------------------------------------------------

🛠 파티클, 카메라 흔들림, 물리적 작동



https://user-images.githubusercontent.com/62327209/232223819-33248faa-2b38-49ee-a904-c0538283ad56.mp4


- 카메라 움직임을 제어하는 부분입니다.
- Code - https://github.com/dydvn/TingTangBall_Portfolio/blob/main/CameraMove.txt
- 플레이어가 다른 오브젝트와 충돌하면 파티클이 나오고 카메라가 흔들리도록 구현했습니다.
- Physic Material을 이용하여 탄성을 구현했습니다.
- Code - [https://github.com/dydvn/TingTangBall/blob/main/CamShake.cs](https://github.com/dydvn/TingTangBall/blob/main/CamShake.cs)

------------------------------------------------------------------------

🛠 기타 함수 모음

Code - https://github.com/dydvn/TingTangBall/blob/main/etc_Function.cs
