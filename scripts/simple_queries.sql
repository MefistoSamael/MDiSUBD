-- Получить все вина
select wine_id, Name from wine;

-- Получить всех пользователей
select * from user

-- Получить только неповторяющиеся вина из коллекций
select distinct wine_id from winecollection

-- Получить существующие коллекции пользователей

select user.Username, collection.note, collection.date from user join collection on collection.user_id = user.user_id

-- Получить существующие рейтинги пользователей

select user.Username, wine.name, winerating.rating  from winerating join user on winerating.user_id = user.user_id join wine on winerating.wine_id = wine.wine_id

-- Получить винодельни конкретного пользователя

select user.Username, winery.name  from wineryuser join user on wineryuser.user_id = user.user_id join winery on wineryuser.winery_id = winery.winery_id

-- Получить владельцев винодельни

-- Получить вино производимое конкретной винодельней

select wine.name as "wine name", winery.name as "winery name" from winery join wine on wine.winery_id = winery.winery_id

-- Получить игристое вино прозиводмое винодельней

select wine.name as "wine name", winery.name as "winery name" from winery join wine on wine.winery_id = winery.winery_id 
join winetype on wine.winetype_id = winetype.winetype_id where sparkling = true

-- Получить винодельню и ее средний, минимальный и максимальный рейтинг по вину

select winery.name as "winery name", AVG(winerating.rating) as "average rating", MIN(winerating.rating) as "min rating",
MAX(winerating.rating) as "max rating"
from winery join wine on winery.winery_id = wine.wine_id join winerating on winerating.wine_id = wine.wine_id
group by winery.winery_id

-- Получить винодельню со средним рейтингом по вину больше 8

select winery.name as "winery name", AVG(winerating.rating) as "average rating" from winery join wine on winery.winery_id = wine.wine_id join winerating on winerating.wine_id = wine.wine_id
group by winery.winery_id having AVG(winerating.rating) >= 8

-- Получить винодельню с 2 или больше винами, рейтинг которых равен 10

select winery.name as "winery name" from winery join wine on winery.winery_id = wine.winery_id join winerating on winerating.wine_id = wine.wine_id where winerating.rating = 10
group by winery.winery_id having COUNT(wine.wine_id) >= 2

-- Получить игристое вино для которого есть 2 и более сочетания с едой  

select wine.name as "wine name", food.name as "food to wine" from wine join winetype on wine.winetype_id = winetype.winetype_id
join pairing on pairing.wine_id = wine.wine_id join foodpairing on pairing.pairing_id = foodpairing.pairing_id join food on food.food_id = foodpairing.food_id
group by wine.wine_id
having COUNT(food.food_id) >=2 and winetype.sparkling 

-- Получить игристое вино для которого есть 2 и более сочетания с едой  другим способом

select wine.name as "wine name" from wine join winetype on wine.winetype_id = winetype.winetype_id 
where 
    EXISTS(select wine.name from pairing join foodpairing on pairing.pairing_id = foodpairing.pairing_id
        join food on food.food_id = foodpairing.food_id where pairing.wine_id = wine.wine_id
        group by wine_id having COUNT(food.food_id) >=2)
    and winetype.sparkling 

-- Использование кейса - получить всех пользователей и коллекциии у некоторых

select user.Username,
CASE 
    WHEN collection.note is null THEN "здесь не на что смотреть"
    ELSE collection.note
end as "note", 
CASE 
    WHEN collection.date is null THEN "здесь не на что смотреть"
    ELSE collection.date
end as "creation date" from user left outer join collection on collection.user_id = user.user_id

