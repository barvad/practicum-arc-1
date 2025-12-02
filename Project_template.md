# Project_template

Это шаблон для решения проектной работы. Структура этого файла повторяет структуру заданий. Заполняйте его по мере работы над решением.

# Задание 1. Анализ и планирование

<aside>

Чтобы составить документ с описанием текущей архитектуры приложения, можно часть информации взять из описания компании и условия задания. Это нормально.

</aside

### 1. Описание функциональности монолитного приложения

**Управление отоплением:**

- Пользователи могут управлять отоплением.
- Система поддерживает датчики температуры с API warmhous v 1.0
- На данный момент система позволяет только отображение текущих показаний датчиков.

**Мониторинг температуры:**

- Пользователи могут смотреть текущие показания датчиков.
- Система поддерживает отображение информации о датчиках включая статус.

### 2. Анализ архитектуры монолитного приложения

- Язык программирования GO
- База данных postgres
- Взаимодействие с пользователями REST API
- API датчика REST API

### 3. Определение доменов и границы контекстов

API - позволяет принимать запросы от пользователей и датчиков (на регистрацию в системе)
DeviceRegistry - добавляет датчик в БД при регистрации в системе и возвращает текущую информацию о датчиках из БД
DeviceStatus - опрашивает датчики снимая текущие показания, отправляет им комманды

### **4. Проблемы монолитного решения**

На данный момент проблем с монолитным решением нет, но поскольку его планируют расширять,
могут возникнуть следующие проблемы:

- Проблемы смасштабируемостью
- Проблемы с отказоустойчивостью
- Проблемы с развертыванием отдельных компонентов, в которых произошли изменения

### 5. Визуализация контекста системы — диаграмма С4

Добавьте сюда диаграмму контекста в модели C4.

Чтобы добавить ссылку в файл Readme.md, нужно использовать синтаксис Markdown. Это делают так:

```markdown
[Context diagram](https://github.com/barvad/practicum-arc-1/blob/warmhouse/Docs/Monolit/Context.puml)
```


# Задание 2. Проектирование микросервисной архитектуры

В этом задании вам нужно предоставить только диаграммы в модели C4. Мы не просим вас отдельно описывать получившиеся микросервисы и то, как вы определили взаимодействия между компонентами To-Be системы. Если вы правильно подготовите диаграммы C4, они и так это покажут.

**Диаграмма контейнеров (Containers)**

```markdown
[Containers diagram](https://github.com/barvad/practicum-arc-1/blob/warmhouse/Docs/Microservices/Container.puml)
```

**Диаграмма компонентов (Components)**

```markdown
[Components diagram](https://github.com/barvad/practicum-arc-1/blob/warmhouse/Docs/Microservices/Component.puml)
```


**Диаграмма кода (Code)**

```markdown
[Code diagram](https://github.com/barvad/practicum-arc-1/blob/warmhouse/Docs/Microservices/DeviceRegistryService_Code.puml)
```

```markdown
[Code diagram](https://github.com/barvad/practicum-arc-1/blob/warmhouse/Docs/Microservices/DeviceStatuseService_Code.puml)
```

# Задание 3. Разработка ER-диаграммы

```markdown
[ER](https://github.com/barvad/practicum-arc-1/blob/warmhouse/Docs/Microservices/ER.puml)
```

# Задание 4. Работа с docker и docker-compose

Перейдите в apps.

Там находится приложение-монолит для работы с датчиками температуры. В README.md описано как запустить решение.

Вам нужно:

1) сделать простое приложение temperature-api на любом удобном для вас языке программирования, которое при запросе /temperature?location= будет отдавать рандомное значение температуры.

Locations - название комнаты, sensorId - идентификатор названия комнаты

```
	// If no location is provided, use a default based on sensor ID
	if location == "" {
		switch sensorID {
		case "1":
			location = "Living Room"
		case "2":
			location = "Bedroom"
		case "3":
			location = "Kitchen"
		default:
			location = "Unknown"
		}
	}

	// If no sensor ID is provided, generate one based on location
	if sensorID == "" {
		switch location {
		case "Living Room":
			sensorID = "1"
		case "Bedroom":
			sensorID = "2"
		case "Kitchen":
			sensorID = "3"
		default:
			sensorID = "0"
		}
	}
```

2) Приложение следует упаковать в Docker и добавить в docker-compose. Порт по умолчанию должен быть 8081

3) Кроме того для smart_home приложения требуется база данных - добавьте в docker-compose файл настройки для запуска postgres с указанием скрипта инициализации ./smart_home/init.sql

Для проверки можно использовать Postman коллекцию smarthome-api.postman_collection.json и вызвать:

- Create Sensor
- Get All Sensors

Должно при каждом вызове отображаться разное значение температуры

Ревьюер будет проверять точно так же.

--------
Пятое задание — дополнительное. Его можно сделать по желанию. Чтобы ревьюер быстрее проверил ваше решение, укажите, сделали вы это задание или нет. Для этого оставьте нужный эмодзи около заголовка задания:

✅ — вы выполнили задание.

❌ — вы пропустили задание.

# ✅  Задание 5. Создание и документирование API

### 1. Тип API

Для взаимодействия с DeviceRegistry REST API так как данные нужны для отображения в веб интерфейсе.
Для взаимодействия DeviceStatusService kafka т.к команда просто отправляется на датчик и они могут долго отвечать, что бы например не держать поток мы используем очередь.

### 2. Документация API

```markdown
[kafka](https://github.com/barvad/practicum-arc-1/blob/warmhouse/Docs/AsyncApi.yml)
```
```markdown
[DeviceRegistry](https://github.com/barvad/practicum-arc-1/blob/warmhouse/Docs/DeviceRegistryService.json)
```
```markdown
[DeviceStatus](https://github.com/barvad/practicum-arc-1/blob/warmhouse/Docs/DeviceStatusServiceApiSwagger.json)
```
