using Domain;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptUsage.Services
{
    public class PGDBService
    {
        private readonly NpgsqlDataSource _source;

        public PGDBService()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=1221;Database=WineCellarDb;Include Error Detail=true";

            _source = NpgsqlDataSource.Create(connectionString);
        }

        #region User CRUD
        public async Task<bool> CreateUser(string username, string password, string email, string profilePicture, string firstName, string lastName, int roleId, Role? role = null)
        {

            _source.OpenConnection();

            await using var command = _source.CreateCommand($"CALL createuser('{roleId}'," +
                                                $"'{username}', " +
                                                $"'{email}', " +
                                                $"'{password}', " +
                                                $"'{firstName}', " +
                                                $"'{lastName}', " +
                                                $"'{profilePicture}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteUser(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deleteuser('{id}')");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateUser(int id, string username, string password, string email, string profilePicture, string firstName, string lastName, int roleId, Role? role = null)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updateuser('{id}'," +
                                                $"'{roleId}', " +
                                                $"'{username}', " +
                                                $"'{email}', " +
                                                $"'{password}', " +
                                                $"'{firstName}', " +
                                                $"'{lastName}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }

        }

        public async Task<List<User>> GetUsers() 
        {
            var users = new List<User>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"select * from muser");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var id = (int)reader["Id"];
                var username = (string)reader["username"];
                var password = (string)reader["password"];
                var email = (string)reader["email"];

                string profilePicture = "";
                var temp = reader["profilepicture"];
                if (temp is System.DBNull)
                    profilePicture = string.Empty;
                else
                    profilePicture = (string)temp;

                var firstName = (string)reader["firstName"];
                var lastName = (string)reader["lastName"];
                var roleId = (int)reader["role_id"];
                users.Add(new User() { Id = id, Username = username, Password = password, Email = email, FirstName = firstName, LastName = lastName, ProfilePicture = profilePicture, RoleId = roleId});
            }

            return users;
        }
        #endregion

        #region Color
        // Создание цвета
        public async Task<bool> CreateColor(string name)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createcolor('{name}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление цвета
        public async Task<bool> UpdateColor(int id, string updatedName)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatecolor('{id}', '{updatedName}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление цвета по id
        public async Task<bool> DeleteColor(int id)
        {
            _source.OpenConnection();

            var color = GetColors().Result.Where(c => c.Id == id).FirstOrDefault();
            if (color is null) return false;

            var colorName = color.Name;
            await using var command = _source.CreateCommand($"CALL deletecolorbyname('{colorName}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех цветов
        public async Task<List<Color>> GetColors()
        {
            var colors = new List<Color>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM color");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var id = (int)reader["id"];
                var colorName = (string)reader["name"];
                colors.Add(new Color() { Id = id, Name = colorName });
            }

            return colors;
        }
        #endregion

        #region Event
        // Создание события
        public async Task<bool> CreateEvent(string name, DateTime date, string location)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createevent('{name}', '{date.ToString("yyyy-MM-dd")}', '{location}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление события
        public async Task<bool> UpdateEvent(int id, string updatedName, DateTime updatedDate, string updatedLocation)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updateevent('{id}', '{updatedName}', '{updatedDate.ToString("yyyy-MM-dd")}', '{updatedLocation}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление события по id
        public async Task<bool> DeleteEvent(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deleteeventbyid('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех событий
        public async Task<List<Event>> GetEvents()
        {
            var events = new List<Event>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM event");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var eventId = (int)reader["id"];
                var eventName = (string)reader["name"];
                var eventDate = (DateTime)reader["date"];
                var eventLocation = (string)reader["location"];
                events.Add(new Event() { Id = eventId, Name = eventName, Date = eventDate, Location = eventLocation });
            }

            return events;
        }
        #endregion

        #region Food

        // Создание еды
        public async Task<bool> CreateFood(string name, string photo, string description)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createfood('{name}', '{photo}', '{description}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление еды
        public async Task<bool> UpdateFood(int id, string updatedName, string updatedPhoto, string updatedDescription)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatefood('{id}', '{updatedName}', '{updatedPhoto}', '{updatedDescription}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление еды по id
        public async Task<bool> DeleteFood(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletefood('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех продуктов
        public async Task<List<Food>> GetFoods()
        {
            var foods = new List<Food>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM food");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var foodId = (int)reader["id"];
                var foodName = (string)reader["name"];
                var foodPhoto = reader["photo"] is DBNull ? string.Empty : (string)reader["photo"];
                var foodDescription = (string)reader["description"];
                foods.Add(new Food() { Id = foodId, Name = foodName, Photo = foodPhoto, Description = foodDescription });
            }

            return foods;
        }

        #endregion

        #region Role

        // Создание роли
        public async Task<bool> CreateRole(string name)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createrole('{name}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление роли
        public async Task<bool> UpdateRole(int id, string updatedName)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updaterole('{id}', '{updatedName}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление роли по id
        public async Task<bool> DeleteRole(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deleterole('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех ролей
        public async Task<List<Role>> GetRoles()
        {
            var roles = new List<Role>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM role");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var roleId = (int)reader["id"];
                var roleName = (string)reader["name"];
                roles.Add(new Role() { Id = roleId, Name = roleName });
            }

            return roles;
        }

        #endregion

        #region Sweetness

        // Создание сладости
        public async Task<bool> CreateSweetness(string name)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createsweetness('{name}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление сладости
        public async Task<bool> UpdateSweetness(int id, string updatedName)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatesweetness('{id}', '{updatedName}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление сладости по id
        public async Task<bool> DeleteSweetness(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletesweetness('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех сладостей
        public async Task<List<Sweetness>> GetSweetnesses()
        {
            var sweetnesses = new List<Sweetness>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM sweetness");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var sweetnessId = (int)reader["id"];
                var sweetnessName = (string)reader["name"];
                sweetnesses.Add(new Sweetness() { Id = sweetnessId, Name = sweetnessName });
            }

            return sweetnesses;
        }

        #endregion

        #region Vintage

        // Создание винтажа
        public async Task<bool> CreateVintage(short year)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createvintage('{year}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление винтажа
        public async Task<bool> UpdateVintage(int id, short updatedYear)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatevintage('{id}', '{updatedYear}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление винтажа по id
        public async Task<bool> DeleteVintage(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletevintage('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех винтажей
        public async Task<List<Vintage>> GetVintages()
        {
            var vintages = new List<Vintage>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM vintage");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var vintageId = (int)reader["id"];
                var vintageYear = (short)reader["year"];
                vintages.Add(new Vintage() { Id = vintageId, Year = vintageYear });
            }

            return vintages;
        }

        #endregion

        #region Winery

        // Создание винодельни
        public async Task<bool> CreateWinery(string name, string phonenumber, string location, string region)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createwinery('{name}', '{phonenumber}', '{location}', '{region}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление винодельни
        public async Task<bool> UpdateWinery(int id, string updatedName, string updatedPhonenumber, string updatedLocation, string updatedRegion)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatewinery('{id}', '{updatedName}', '{updatedPhonenumber}', '{updatedLocation}', '{updatedRegion}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление винодельни по id
        public async Task<bool> DeleteWinery(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletewinery('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех винодельней
        public async Task<List<Winery>> GetWineries()
        {
            var wineries = new List<Winery>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM winery");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var wineryId = (int)reader["id"];
                var wineryName = (string)reader["name"];
                var wineryPhonenumber = reader["phonenumber"] is DBNull ? string.Empty : (string)reader["phonenumber"];
                var wineryLocation = (string)reader["location"];
                var wineryRegion = (string)reader["region"];
                wineries.Add(new Winery() { Id = wineryId, Name = wineryName, PhoneNumber = wineryPhonenumber, Location = wineryLocation, Region = wineryRegion });
            }

            return wineries;
        }

        #endregion

        #region WineType

        // Создание типа вина
        public async Task<bool> CreateWineType(bool sparkling, int colorId, int sweetnessId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createwinetype('{sparkling}', '{colorId}', '{sweetnessId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление типа вина
        public async Task<bool> UpdateWineType(int id, bool updatedSparkling, int updatedColorId, int updatedSweetnessId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatewinetype('{id}', '{updatedSparkling}', '{updatedColorId}', '{updatedSweetnessId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление типа вина по id
        public async Task<bool> DeleteWineType(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletewinetype('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех типов вина
        public async Task<List<WineType>> GetWineTypes()
        {
            var wineTypes = new List<WineType>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM winetype");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var wineTypeId = (int)reader["id"];
                var wineTypeSparkling = (bool)reader["sparkling"];
                var wineTypeColorId = (int)reader["color_id"];
                var wineTypeSweetnessId = (int)reader["sweetness_id"];
                wineTypes.Add(new WineType() { Id = wineTypeId, Sparkling = wineTypeSparkling, ColorId = wineTypeColorId, SweetnessId = wineTypeSweetnessId });
            }

            return wineTypes;
        }

        #endregion

        #region Wine

        // Создание вина
        public async Task<bool> CreateWine(string name, string photo, string description, int vintageId, int wineTypeId, int wineryId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createwine('{name}', '{photo}', '{description}', '{vintageId}', '{wineTypeId}', '{wineryId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление вина
        public async Task<bool> UpdateWine(int id, string updatedName, string updatedPhoto, string updatedDescription, int updatedVintageId, int updatedWineTypeId, int updatedWineryId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatewine('{id}', '{updatedName}', '{updatedPhoto}', '{updatedDescription}', '{updatedVintageId}', '{updatedWineTypeId}', '{updatedWineryId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление вина по id
        public async Task<bool> DeleteWine(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletewine('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех вин
        public async Task<List<Wine>> GetWines()
        {
            var wines = new List<Wine>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM wine");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var wineId = (int)reader["id"];
                var wineName = reader["name"] is DBNull ? string.Empty : (string)reader["name"];
                var winePhoto = reader["photo"] is DBNull ? string.Empty : (string)reader["photo"];
                var wineDescription = reader["description"] is DBNull ? string.Empty : (string)reader["description"];
                var wineVintageId = (int)reader["vintage_id"];
                var wineWineTypeId = (int)reader["wineType_id"];
                var wineWineryId = (int)reader["winery_id"];
                wines.Add(new Wine() { Id = wineId, Name = wineName, Photo = winePhoto, Description = wineDescription, VintageId = wineVintageId, WineTypeId = wineWineTypeId, WineryId = wineWineryId });
            }

            return wines;
        }

        #endregion

        #region Pairing

        // Создание пары блюдо-вино
        public async Task<bool> CreatePairing(string description, int wineId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createpairing('{description}', '{wineId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление пары блюдо-вино
        public async Task<bool> UpdatePairing(int id, string updatedDescription, int updatedWineId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatepairing('{id}', '{updatedDescription}', '{updatedWineId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление пары блюдо-вино по id
        public async Task<bool> DeletePairing(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletepairing('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех пар блюдо-вино
        public async Task<List<Pairing>> GetPairings()
        {
            var pairings = new List<Pairing>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM pairing");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var pairingId = (int)reader["id"];
                var pairingDescription = reader["description"] is DBNull ? string.Empty : (string)reader["description"];
                var pairingWineId = (int)reader["wine_id"];
                pairings.Add(new Pairing() { Id = pairingId, Description = pairingDescription, WineId = pairingWineId });
            }

            return pairings;
        }

        #endregion

        #region FoodPairing

        // Добавление связи между блюдом и парой блюдо-вино
        public async Task<bool> CreateFoodPairing(int foodId, int pairingId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createfoodpairing('{foodId}', '{pairingId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление связи между блюдом и парой блюдо-вино
        public async Task<bool> DeleteFoodPairing(int foodId, int pairingId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletefoodpairing('{foodId}', '{pairingId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        #endregion

        #region Collection

        // Добавление в коллекцию
        public async Task<bool> AddToCollection(DateTime date, string note, int userId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createcollection('{date:yyyy-MM-dd}', '{note}', '{userId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление коллекции
        public async Task<bool> UpdateCollection(int id, DateTime updatedDate, string updatedNote, int updatedUserId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatecollection('{id}', '{updatedDate:yyyy-MM-dd}', '{updatedNote}', '{updatedUserId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление из коллекции по id
        public async Task<bool> RemoveCollection(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletecollection('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех записей в коллекции
        public async Task<List<Collection>> GetCollection()
        {
            var collections = new List<Collection>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM collection");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var collectionId = (int)reader["id"];
                var collectionDate = (DateTime)reader["date"];
                var collectionNote = reader["note"] is DBNull ? string.Empty : (string)reader["note"];
                var collectionUserId = (int)reader["user_id"];
                collections.Add(new Collection() { Id = collectionId, Date = collectionDate, Note = collectionNote, UserId = collectionUserId });
            }

            return collections;
        }

        #endregion

        #region UserOrganiserEvent

        // Добавление пользователя в организаторы события
        public async Task<bool> AddUserOrganiserEvent(int userId, int eventId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createuserorganiserevent('{userId}', '{eventId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление пользователя из организаторов события
        public async Task<bool> RemoveUserOrganiserEvent(int userId, int eventId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deleteuserorganiserevent('{userId}', '{eventId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        #endregion

        #region UserParticipatorEvent

        // Добавление пользователя в участники события
        public async Task<bool> AddUserParticipatorEvent(int userId, int eventId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createuserparticipatorevent('{userId}', '{eventId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление пользователя из участников события
        public async Task<bool> RemoveUserParticipatorEvent(int userId, int eventId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deleteuserparticipatorevent('{userId}', '{eventId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        #endregion

        #region WineCollection

        // Добавление вина в коллекцию
        public async Task<bool> AddWineToCollection(int wineId, int collectionId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createwinecollection('{wineId}', '{collectionId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление вина из коллекции
        public async Task<bool> RemoveWineFromCollection(int wineId, int collectionId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletewinecollection('{wineId}', '{collectionId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        //получение всех коллекций
        // Получение всех рейтингов вина
        public async Task<List<Wine>> GetWinesCollection(int collectionId)
        {
            var wines = new List<Wine>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"select * from WineCollection join Wine on WineCollection.wine_id = wine.id and WineCollection.collection_id = {collectionId}");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var wineId = (int)reader["id"];
                var wineName = reader["name"] is DBNull ? string.Empty : (string)reader["name"];
                var winePhoto = reader["photo"] is DBNull ? string.Empty : (string)reader["photo"];
                var wineDescription = reader["description"] is DBNull ? string.Empty : (string)reader["description"];
                var wineVintageId = (int)reader["vintage_id"];
                var wineWineTypeId = (int)reader["wineType_id"];
                var wineWineryId = (int)reader["winery_id"];
                wines.Add(new Wine() { Id = wineId, Name = wineName, Photo = winePhoto, Description = wineDescription, VintageId = wineVintageId, WineTypeId = wineWineTypeId, WineryId = wineWineryId });
            }

            return wines;
        }

        #endregion

        #region WineEvent

        // Добавление вина в событие
        public async Task<bool> AddWineToEvent(int eventId, int wineId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createwineevent('{eventId}', '{wineId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление вина из события
        public async Task<bool> RemoveWineFromEvent(int eventId, int wineId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletewineevent('{eventId}', '{wineId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        #endregion

        #region WineRating

        // Добавление рейтинга вина
        public async Task<bool> AddWineRating(int rating, string description, int userId, int wineId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createwinerating('{rating}', '{description}', '{userId}', '{wineId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Обновление рейтинга вина
        public async Task<bool> UpdateWineRating(int id, int updatedRating, string updatedDescription, int updatedUserId, int updatedWineId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL updatewinerating('{id}', '{updatedRating}', '{updatedDescription}', '{updatedUserId}', '{updatedWineId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление рейтинга вина по id
        public async Task<bool> RemoveWineRating(int id)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deletewinerating('{id}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Получение всех рейтингов вина
        public async Task<List<WineRating>> GetWineRatings()
        {
            var wineRatings = new List<WineRating>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM WineRating");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var ratingId = (int)reader["id"];
                var wineRating = (int)reader["rating"];
                var ratingDescription = reader["description"] is DBNull ? string.Empty : (string)reader["description"];
                var ratingUserId = (int)reader["user_id"];
                var ratingWineId = (int)reader["wine_id"];
                wineRatings.Add(new WineRating() { Id = ratingId, Rating = wineRating, Description = ratingDescription, UserId = ratingUserId, WineId = ratingWineId });
            }

            return wineRatings;
        }

        #endregion

        #region WineryUser

        // Добавление пользователя к винодельне
        public async Task<bool> AddUserToWinery(int userId, int wineryId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL createwineryuser('{userId}', '{wineryId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        // Удаление пользователя из винодельни
        public async Task<bool> RemoveUserFromWinery(int userId, int wineryId)
        {
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"CALL deleteuserwinery('{userId}', '{wineryId}');");

            try
            {
                await command.ExecuteNonQueryAsync();
                return true;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("Error: " + ex.Message);
                return false;
            }
        }

        #endregion
        public async Task<IEnumerable<Food>> GetFoodOfPairing(int id)
        {
            var foods = new List<Food>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"select * from FoodPairing join Food on FoodPairing.food_id = food.id and FoodPairing.pairing_id = {id}");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var foodId = (int)reader["id"];
                var foodName = (string)reader["name"];
                var foodPhoto = reader["photo"] is DBNull ? string.Empty : (string)reader["photo"];
                var foodDescription = (string)reader["description"];
                foods.Add(new Food() { Id = foodId, Name = foodName, Photo = foodPhoto, Description = foodDescription });
            }

            return foods;

        }

        public async Task<IEnumerable<Winery>> GetUserWinaries(int id)
        {
            var wineries = new List<Winery>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"select * from wineryuser join winery on wineryuser.winery_id = winery.id and wineryuser.user_id = {id}");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var wineryId = (int)reader["id"];
                var wineryName = (string)reader["name"];
                var wineryPhonenumber = reader["phonenumber"] is DBNull ? string.Empty : (string)reader["phonenumber"];
                var wineryLocation = (string)reader["location"];
                var wineryRegion = (string)reader["region"];
                wineries.Add(new Winery() { Id = wineryId, Name = wineryName, PhoneNumber = wineryPhonenumber, Location = wineryLocation, Region = wineryRegion });
            }

            return wineries;
        }

        public async Task<IEnumerable<User>> GetParticipantsForEvent(int eventId)
        {
            var users = new List<User>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"select * from muser join userparticipatorevent on muser.id = userparticipatorevent.user_id and userparticipatorevent.event_id = {eventId}");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var id = (int)reader["Id"];
                var username = (string)reader["username"];
                var password = (string)reader["password"];
                var email = (string)reader["email"];

                string profilePicture = "";
                var temp = reader["profilepicture"];
                if (temp is System.DBNull)
                    profilePicture = string.Empty;
                else
                    profilePicture = (string)temp;

                var firstName = (string)reader["firstName"];
                var lastName = (string)reader["lastName"];
                var roleId = (int)reader["role_id"];
                users.Add(new User() { Id = id, Username = username, Password = password, Email = email, FirstName = firstName, LastName = lastName, ProfilePicture = profilePicture, RoleId = roleId });
            }

            return users;
        }

        public async Task<IEnumerable<User>> GetOrganisersForEvent(int eventId)
        {
            var users = new List<User>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"select * from muser join userorganiserevent on muser.id = userorganiserevent.user_id and userorganiserevent.event_id = {eventId}");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var id = (int)reader["Id"];
                var username = (string)reader["username"];
                var password = (string)reader["password"];
                var email = (string)reader["email"];

                string profilePicture = "";
                var temp = reader["profilepicture"];
                if (temp is System.DBNull)
                    profilePicture = string.Empty;
                else
                    profilePicture = (string)temp;

                var firstName = (string)reader["firstName"];
                var lastName = (string)reader["lastName"];
                var roleId = (int)reader["role_id"];
                users.Add(new User() { Id = id, Username = username, Password = password, Email = email, FirstName = firstName, LastName = lastName, ProfilePicture = profilePicture, RoleId = roleId });
            }

            return users;
        }

        public async Task<IEnumerable<Wine>> GetWinesForEvent(int eventId)
        {
            var wines = new List<Wine>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM WineEvent join wine on WineEvent.wine_id = wine.id and WineEvent.event_id = {eventId}");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var wineId = (int)reader["id"];
                var wineName = reader["name"] is DBNull ? string.Empty : (string)reader["name"];
                var winePhoto = reader["photo"] is DBNull ? string.Empty : (string)reader["photo"];
                var wineDescription = reader["description"] is DBNull ? string.Empty : (string)reader["description"];
                var wineVintageId = (int)reader["vintage_id"];
                var wineWineTypeId = (int)reader["wineType_id"];
                var wineWineryId = (int)reader["winery_id"];
                wines.Add(new Wine() { Id = wineId, Name = wineName, Photo = winePhoto, Description = wineDescription, VintageId = wineVintageId, WineTypeId = wineWineTypeId, WineryId = wineWineryId });
            }

            return wines;
        }

        public async Task<IEnumerable<User>> GetUsersForWinery(int wineryId)
        {
            var users = new List<User>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"select * from muser join wineryuser on muser.id = wineryuser.user_id and wineryuser.winery_id = {wineryId}");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var id = (int)reader["Id"];
                var username = (string)reader["username"];
                var password = (string)reader["password"];
                var email = (string)reader["email"];

                string profilePicture = "";
                var temp = reader["profilepicture"];
                if (temp is System.DBNull)
                    profilePicture = string.Empty;
                else
                    profilePicture = (string)temp;

                var firstName = (string)reader["firstName"];
                var lastName = (string)reader["lastName"];
                var roleId = (int)reader["role_id"];
                users.Add(new User() { Id = id, Username = username, Password = password, Email = email, FirstName = firstName, LastName = lastName, ProfilePicture = profilePicture, RoleId = roleId });
            }

            return users;
        }

        public async Task<IEnumerable<Event>> GetEventsForParticipator(int userId)
        {
                var events = new List<Event>();
                _source.OpenConnection();
                await using var command = _source.CreateCommand($"SELECT * FROM event join userparticipatorevent on event.id = userparticipatorevent.event_id and userparticipatorevent.user_id = {userId}");

                var reader = command.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    var eventId = (int)reader["id"];
                    var eventName = (string)reader["name"];
                    var eventDate = (DateTime)reader["date"];
                    var eventLocation = (string)reader["location"];
                    events.Add(new Event() { Id = eventId, Name = eventName, Date = eventDate, Location = eventLocation });
                }

                return events;
        }

        public async Task<IEnumerable<Event>> GetEventsForOrganizer(int userId)
        {
            var events = new List<Event>();
            _source.OpenConnection();
            await using var command = _source.CreateCommand($"SELECT * FROM event join userorganiserevent on event.id = userorganiserevent.event_id and userorganiserevent.user_id = {userId}");

            var reader = command.ExecuteReader();
            while (await reader.ReadAsync())
            {
                var eventId = (int)reader["id"];
                var eventName = (string)reader["name"];
                var eventDate = (DateTime)reader["date"];
                var eventLocation = (string)reader["location"];
                events.Add(new Event() { Id = eventId, Name = eventName, Date = eventDate, Location = eventLocation });
            }

            return events;
        }
    }
}
