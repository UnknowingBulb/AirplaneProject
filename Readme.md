Проект по оформлению заказа

Как запустить:
0) Должен быть установлен докер
1) В powershell из корневой папки проекта выполнить:
```
docker build -f AirplaneProject/Dockerfile .
docker compose up
```
2) Потом в параллельном окне (для инициализации/обновления БД):

```
cd AirplaneProject
dotnet ef database update
```
По умолчанию работает на 85м порту - http://localhost:85/ .
Чтобы добавить данные в БД - зайти на страницу http://localhost:85/SpawnData и добавить данные.