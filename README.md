Групповой учебный проект реализованный по шаблонам grasp. Программа представляет из себя систему учета планов-графиков отлова животных.

Мы работали над проектом втроем и фиксировали изменения в другом репозитории, этот является его копией.

Моя часть работы:
  1. Классы:
     * PlanScheduleController  - Контроллер для моей части работы
     * ExportMaster            - Экспорт данных в эксель
     * ImportMaster            - Импорт pdf
  3. Формы:
     * PlanScheduleCardForm    - Форма карточки для выбранной записи
       
Общая часть работы:
  1. Классы:
     * DB                       - Подключение и все запросы к БД
     * Классы в папке Role      - Классы с ролями пользователей в которых переопреледяются методы для доступа к данным
  3. Формы:
     * MainForm                - Общая форма / Моя часть: Реестр планов отлова
  5. База данных:
     * db.sqlite3
    
Данные для авторизации в приложении Логин/Пароль: 13/13
