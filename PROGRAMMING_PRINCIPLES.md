# Принципи програмування в проєкті ATM

У цьому документі описано застосування ключових принципів програмування в проєкті ATM.

---

## 1. Encapsulation (Інкапсуляція)

Клас `Account` приховує внутрішню логіку зміни балансу. 
https://github.com/ipz241vpm/DotNetLab2/blob/d664d50f544ceb1232f0437e7a9db6fc42ed432e/ClassLibrary1/Account.cs#L9-L37

Властивість `Balance` має приватний set, що не дозволяє змінювати баланс напряму ззовні класу.
Це гарантує, що зміна балансу відбувається тільки через методи `Deposit` та `Withdraw`.
https://github.com/ipz241vpm/DotNetLab2/blob/d664d50f544ceb1232f0437e7a9db6fc42ed432e/ClassLibrary1/Account.cs#L14

---

## 2. Single Responsibility Principle (SRP)

Клас `AtmService` відповідає виключно за бізнес-логіку банкомату:

- **Авторизація** – метод `Authenticate`  
  https://github.com/ipz241vpm/DotNetLab2/blob/master/ClassLibrary1/AtmService.cs#L26-L32

- **Перевірка балансу** – метод `CheckBalance`  
  https://github.com/ipz241vpm/DotNetLab2/blob/master/ClassLibrary1/AtmService.cs#L34-L38

- **Проведення транзакцій:**
  - Зняття коштів – метод `Withdraw`  
    https://github.com/ipz241vpm/DotNetLab2/blob/master/ClassLibrary1/AtmService.cs#L40-L55
  - Поповнення рахунку – метод `Deposit`  
    https://github.com/ipz241vpm/DotNetLab2/blob/master/ClassLibrary1/AtmService.cs#L57-L68
  - Переказ коштів – метод `Transfer`  
    https://github.com/ipz241vpm/DotNetLab2/blob/master/ClassLibrary1/AtmService.cs#L70-L82

Клас не відповідає за відображення інформації користувачу.
UI реалізований окремо в проєктах `AtmConsole` та `AtmWinForm`.

Таким чином дотримується принцип єдиної відповідальності —
клас відповідає лише за логіку роботи банкомату.

---

---

## 3. Dependency Injection (Впровадження залежностей)

Клас `AtmService` отримує свої залежності (`Bank` та `AutomatedTellerMachine`) через конструктор. Це дозволяє відокремити створення об'єктів від їх використання та полегшує тестування.

* **Приклад у коді:** Конструктор класу `AtmService`.
https://github.com/ipz241vpm/DotNetLab2/blob/master/ClassLibrary1/AtmService.cs#L14-L18

---

## 4. DRY (Don't Repeat Yourself)

Бізнес-логіка (авторизація, зняття коштів, перевірка балансу)
реалізована один раз у бібліотеці `ClassLibrary1`
та використовується як у `AtmConsole`, так і у `AtmWinForm`.

Це дозволяє уникнути дублювання алгоритмів у різних UI.

* **Приклад у коді:** Клас `AtmService`
https://github.com/ipz241vpm/DotNetLab2/blob/master/ClassLibrary1/AtmService.cs
