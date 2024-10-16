# Домашние задание по курсу Highload Architect(Otus)

1. Заготовка для социальной сети

    **Цель:**
    В результате выполнения ДЗ вы создадите базовый скелет социальной сети, который будет развиваться в дальнейших ДЗ.

    **В данном задании тренируются навыки:**

    декомпозиции предметной области;
    построения элементарной архитектуры проекта

    **Описание/Пошаговая инструкция выполнения домашнего задания:**
    Требуется разработать создание и просмотр анкет в социальной сети.

    **Функциональные требования:**

    - Простейшая авторизация пользователя.
    - Возможность создания пользователя, где указывается следующая информация:
    - Имя
    - Фамилия
    - Дата рождения
    - Пол
    - Интересы
    - Город
    - Страницы с анкетой.

    **Нефункциональные требования:**

    Любой язык программирования
    В качестве базы данных использовать PostgreSQL (при желании и необходимости любую другую SQL БД)
    Не использовать ORM
    Программа должна представлять из себя монолитное приложение.
    Не рекомендуется использовать следующие технологии:
    Репликация
    Шардирование
    Индексы
    Кэширование

    **Для удобства разработки и проверки задания можно воспользоваться этой спецификацией и реализовать в ней методы:**
    - */login*
    - */user/register*
    - */user/get/{id}*

    Фронт опционален.
    Сделать инструкцию по локальному запуску приложения, приложить Postman-коллекцию.
    ДЗ принимается в виде исходного кода на github и Postman-коллекции.

    **Критерии оценки:**
    Оценка происходит по принципу зачет/незачет.

    **Требования:**
    - Есть возможность авторизации, регистрации, получение анкет по ID.
    - Отсутствуют SQL-инъекции.
    - Пароль хранится безопасно.

## Инструкция по запуску приложения

### Запуск при помощи Docker

1. Скачать и установить Docker
2. Если используется ОС Windows, запустить файл *deploy\up-dev-compose.cmd*
   - Если используется иная, открыть файл и запустить содержимое в терминале

### Запуск при помощи dotnet

1. Скачать dotnet SDK .NET8 последней версии <https://dotnet.microsoft.com/en-us/download/dotnet/8.0>
2. Скачать и установить PostgreSQL <https://www.postgresql.org/download/>
3. Создать DB и применить скрипты из папки *deploy\init-db*
4. Настроить в *src\Otus-SocialNetwork\appsettings.json* путь до БД и логин/пароль для подключения
5. Запустить

    ``` sh
    cd src\Otus-SocialNetwork
    dotnet run
    ```

6. Приложение доступно по адресу *<http://localhost:5160>*

    *Swagger*: *<http://localhost:5160/swagger/index.html>*
