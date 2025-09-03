## Networking

Сетевой модуль для VRF на основе Mirror. Достаточно гибко настраивается, относительно просто расширяется и 
позволяет другим модулям (привет Scenario) его использовать

`VRFNetworkManager` - основной класс для работы с сетевыми взаимодействиями. Отвечает за всю основную работу,
хранит в себе основные данные и на нём завязаны почти все остальные сетевые сервисы. Вот примерно чем он занимается
- Игроками и их подключениями
- Сценами
- Сетевыми сообщениями

## Identities
`IdentityService` - Сервис идентификации, прокидывает все нужные данные для идентификации игроков и их обликов. Работает на всех игроках, данные берёт локально  
`VRFAuthenticator` - Кастомный идентификатор, поддерживает различные способы инициализации (это определяется внутри самого PlayerIdentity)

## Interfaces
`VRFNetworkManager` реализует интерфейсы:
- `INetworkClientMessages` - подписка/отписка на клиентские сетевые сообщения
- `INetworkServerMessages` - подписка/отписка на серверные сетевые сообщения
- `INetworkClientEvents` - клиентские событыя (start, stop, connect, disconnect)
- `INetworkServerEvents` - серверные события (start, stop)

## Server
`ServerAppearancesContainer` - контейнер всех изменённых внешностей, синхронизирует внешний вид всех изменённых игроков и всех клиентов.
Подаисывается на события `VRFNetworkManager` для выдачи представлений для игроков, 
а также подписывается на серверные сообщения:
- `NetAppearanceUpdateRequestMessage` - для отправки сообщения изменения представления игрока во время игры;
- `NetNetAppearanceResetRequestMessage` - для отрпавки сообщения сброса представления игрока во время игры.

Использует `IdentityService` для определения `PlayerIdentityConfig` по его `IdentityCode`

---
`ServerCounterPlayers` - класс для проверки ограничения на количество подключений для одной роли.
Для отслеживания подписывается на события `VRFNetworkManager`: `OnServerClientAuthorized`, `OnServerDisconnectEvent`.
Используется в `VRFAuthenticator`.
---
`ServerItemsContainer` - Контейнер всех созданных сетевых предметов, создаёт и удаляет их для всех игроков.
Подписывается на серверные сообщения запросы по созданию и уничтожению предметов, а также пересылает всем клиентам сообщения связанные с действиями над предметами (enable, disable, Toggle MonoBehaviour(s)).
Использует `ViewsNetSourcesContainer` для получения View предметов и `ProjectViewSpawnerService` для их спавна.
---
`ServerScenesService` - In Progress

## Client
`ClientItemsService` - клиентский сервис для обработки сообщений, связаных с предметами. Принимает сообщение и вызывает соответсвующее событие.  
`ClientMessagesRepeater` - Общий сервис для приёма клиентских сообщений.  
`ClientPlayerAppearances` - Контейнер отображений, синхронизирующий своё состояние с серверным.

# Initializers
Инициализация и очистка памяти для сети и сетевых игроков
## Net Players
`NetPlayersDestroyer` - Деспавнит игроков при Dispose  
`NetPlayersSpawner` - Спавнит игроков при старте сервера
## Network
`NetworkDisposeService` - Выключает сеть при Dispose  
`NetworkInitializeService` - Инициализирует и запускает сеть, инициализирует идентификацию  

# Messages
Пакеты, пересылаемые по сети во время её работы
## AuthMessages - Пакеты для авторизации клиентов в VRF Authenticator
`AuthNicknameRequestMessage` - Запрос на никнейм игрока  
`AuthNicknamePasswordRequestMessage` - Запрос на никнейм и пароль игрока  
`AuthIDRequestMessage` - Запрос на ID игрока  
`AuthResponseMessage` - Ответ от сервера (пока что только после авторизации)  
## NetAppearanceMessages - Пакеты, связанные с изменением внешнего вида сетевых игроков
`NetAppearanceUpdateRequestMessage` - Пакет отправляется на сервер для обновления IK объекта  
`NetNetAppearanceResetRequestMessage` - Пакет отправляется на сервер для сброса IK объекта  
`NetAppearanceUpdateMessage` - Пакет отправляется на клиентов для обновления IK объекта  
`NetAppearanceResetMessage` - Пакет отправляется на клиентов для сброса IK объекта    
## NetItemsMessages - Пакеты, связанные со спауном сестевых предметво игроков
`NetItemSpawnRequestMessage` - Запрос на сервер для спавна предмета    
`NetItemDestroyRequestMessage` - Запрос на сервер для уничтожения предмета  
`NetItemEnableRequestMessage` - Запрос на сервер для включения предмета  
`NetItemDisableRequestMessage` - Запрос на сервер для выключения предмета  
`NetItemToggleBehaviourMessage` - Запрос на сервер для указания активности Behaviour компонента  
`NetItemToggleBehavioursMessage` - Запрос на сервер для указания активности всех Behaviour компонентов  
## NetPlayerConfig - Стартовая конфигурация нового игрока, посылается один раз при авторизации игрока
`IPlayerConfigMessage` - Интерфейс для сообщений содержащих NetPlayerConfig 
## NetPlayerMessages - Пакеты для идентификации созданных сетевых игроков
`SpawnNetPlayerRequestMessage` - Пакет отправляется на сервер для инициализации сетевого игрока от каждого нового клиента  
`SpawnNetPlayerMessage` - Пакет отправляется на конкретного клиента для инициализации сетевого игрока и содержит в себе код роли на которую он должен ссылаться  
## ScenesMessages - Пакеты, связанные со сменой сцен
`SceneUpdateMessage` - Уведомление о том, что сцена у локального игрока загружена  

# Models
Модели, описывающие сетевые объекты  
`NetMode` - Режимы передачи сети, используется для самоидентификации сетевых модулей
## AuthIdentity
`AuthIdentityConfig` - Реализация AuthIdentityModel для Scriptables  
`AuthIdentityModel` - Модель для идентификации игрока. Встроен в систему DataSources. Настроен на правильную сериализацию в JSON  
`AuthIdentityModelCmdParser` - Реализация AuthIdentityModel для CommandLine  
## Net
`NetConfig` - Реализация NetModel для Scriptables  
`NetModel` - Модель со всеми данными, чтобы запустить сервер в определённом режиме. Используется в DataSources. Оптимизирован для сериализации в JSON    
`NetModelCmdParser` - Реализация NetModel для CommandLine  

# Players
Сервисы и классы, связанные с сетевыми игроками  
`NetPlayerViewInitializer` - Конфигуратор для начала работы сетевого игрока. Используется только для инициализации своей роли и делает инициализацию NetPlayerView  
`NetPlayersContainerService` - Контейнер созданных и проинициализированных игроков. Добавление идёт из самих net игроков, так как инициализация происходит в них же  
# Scriptables
Сетевые данные, хранимые в виде `ScriptableObject`

# Services
Вспомогательные сетевые сервисы
