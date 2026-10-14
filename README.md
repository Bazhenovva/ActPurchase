Баженова Виктория Димтриевна

ИП-24-4

Задание:ASP.NET Core Web API

## Пример акта

![Акт закупки имущества](images/act_img.jpg)


## Схема сущностей

```mermaid
classDiagram
    Act --> Counterparty : Buyer
    Act --> Counterparty : Seller
    Act --> Director : Director
    Act --> ActItem : Items
```
