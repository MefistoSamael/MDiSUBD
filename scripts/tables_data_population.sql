insert into color (name)
values ('red'), ('green'), ('pink');

insert into sweetness (name)
values ('sweet'), (' semi-sweet'), ('dry'), ('semi-dry');

insert into WineType (sparkling, color_id, sweetness_id)
values (true, 1, 1 ), (false, 2, 2), (false, 3, 1), (true, 1, 1);

insert into vintage (year)
values (2011), (2012), (2013), (2014);

insert into role (name)
values ('customer'), ('Sommelier'), ('Winery Owner'), ('Organizer'), ('Admin');

insert into user (username, email, password, profilePicture, FirstName, LastName, role_id)
values ('SiMple_Customer228', 'cust@mail.ru', '123', null, 'Ivan', 'Ivanov', '1'),
       ('SOMmelier336', 'som@gmail.com', '456', null, 'Petr', 'Petrov','2'),
       ('xxx_BIG OWNER_xxx', 'owner66@yahoo.com', '789', null, 'Genadiy', 'Gorin','3'), 
       ('TUSA_JUSA_Organizer1337', 'best_partyes@chtoto.ru', '135', null, 'Petr', 'Petrovich','4'), 
       ('OTEC', 'god_father@gmail.com', '123', null, 'Secret', 'Secret','5'),
       ('xxx_ANOTHER BIG OWNER_xxx', 'owner77@yahoo.com', '789', null, 'Terentiy', 'Gorin','3'),
       ('simple OWNER ', 'o99wner@yahoo.com', '789', null, 'Anton', 'Antonov','3')

insert into winery (name, phonenumber, location, region)
values ('Awesome winery', '8-800-555-35-35', 'somwhere in shmpaign', 'shampaign'),
       ('Brilliant winery', '8-800-666-66-66', 'somwhere in spain', 'spain region'),
       ('Wonderful winery', '8-800-111-22-333', 'in the middle of nowhere', 'nowhere')

insert into wineryUser (user_id, winery_id)
values (3, 1), (6, 2), (7, 3)

insert into wine (name, photo, description, vintage_id, wineType_id, winery_id)
values ('First Wine', null, 'first wine at the planet', 1, 1, 1),
       ('Second Wine', null, 'second favoriye wine of J. Kennedy', 2, 2, 2),
       ('Third Wine', null, 'third wine at world competition', 3, 3, 3)

insert into food (name, description)
values ('potato', 'tasteful product at the planet'),
       ('fish', 'good only in sushi'),
       ('chips', 'p-o-t-a-t-o chi-p-s'),
       ('chicken nugets', 'KFC - one love for Kniga'),
       ('chicken wings', 'SOOOO good')

insert into pairing (wine_id, description)
values (1, 'very good pairing'),
       (2, 'awesome pairing'),
       (3, 'brilliant pairing')

insert into foodpairing (food_id, pairing_id)
values (1, 1),
       (2, 1),
       (3, 1),
       (1, 2),
       (1, 3),
       (2, 3)

insert into event values ('1', 'funny event', '9-11-2001', 'World trade center, New Yowrk')

insert into wineevent values ('1', '1'), ('1', '2'), ('1', '3')

UserOrganiserEvent:
insert into userorganiserevent values ('4', '1')

insert into userparticipatorevent values ('1', '1'), ('2', '1'), ('3', '1')

insert into winerating (Rating, description, user_id, wine_id)
values ('10/10', 'awesome', 1, 1),
       ('1/10', 'awful', 2, 1),
       ('0/0', 'err: division by zero', 3, 3)

insert into collection (date, note, user_id)
values ('01-01-2001', 'awesome collection', 1),
       ('10-15-2023', 'i am tired', 2),
       ('02-04', 'dont forget to put poison in wine', 3) 

insert into winecollection 
values ('1', '1'),
       ('1', '2'),
       ('2', '2'),
       ('2', '3'),
       ('3', '1'),
       ('3', '3')