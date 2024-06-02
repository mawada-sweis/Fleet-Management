using Newtonsoft.Json;
using Npgsql;
using System.Collections.Concurrent;
using FleetManagementShared;
using System;
using System.Threading.Tasks;

namespace FleetManagementLibrary
{
    
    public class GVAR
    {
        //for sending and receiving data to and from the backend
        public ConcurrentDictionary<string, ConcurrentDictionary<string, string>> DicOfDic =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, string>>();
        public ConcurrentDictionary<string, ConcurrentDictionary<string, string>[]> DicOfDT =
            new ConcurrentDictionary<string, ConcurrentDictionary<string, string>[]>();
    }

    public class EndpointsRepository
    {
        private string _connectionString;
        private readonly WebSocketsManager _webSocketManager;
        private enum VehicleStatus
        {
            Stopped = 0,
            Moving = 1,
            Idle = 2,
            OutOfService = 3,
            Maintenance = 4
        }
        public EndpointsRepository(string connectionString, 
                                WebSocketsManager webSocketManager)
        {
            this._connectionString = connectionString;
            this._webSocketManager = webSocketManager;
        }

        private ConcurrentDictionary<string, string>[] RetrieveData(string tableName)
        {
            var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM {tableName}";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var rowDictionary = new ConcurrentDictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    rowDictionary[reader.GetName(i)] = reader[i].ToString();
                                }
                                result.Add(rowDictionary);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return result.ToArray();
        }

        private long GetEpochTime(string date)
        {
            Console.WriteLine(date);
            long epochTime = 0;
            try
            {
                // Step 1: Parse the input date string to DateTime
                DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss",
                    System.Globalization.CultureInfo.InvariantCulture);

                // Step 2: Convert the DateTime to DateTimeOffset
                DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.Zero);

                // Step 3: Calculate the epoch time in milliseconds
                epochTime = dateTimeOffset.ToUnixTimeSeconds();

                return epochTime;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occures when parsing date to epoch time {ex.Message}");
                return epochTime;
            }
        }

        public void GetVehicles(ref GVAR gvarData)
        {

            var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();


                    string query = "SELECT * " +
                        "FROM \"Vehicles\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();
                            if (reader != null)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            }
                            else gvarData.DicOfDic["Tags"].TryAdd("STS", "0");

                            while (reader.Read())
                            {
                                var rowDictionary = new ConcurrentDictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    rowDictionary[reader.GetName(i)] = reader[i].ToString();
                                }
                                result.Add(rowDictionary);
                            }
                            gvarData.DicOfDT["Vehicles"] = result.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void AddVehicle(ref GVAR gvarData)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("VehicleNumber", out var vehicleNumberStr) &&
                        tagsData.TryGetValue("VehicleType", out var vehicleType))
                    {

                        if (long.TryParse(vehicleNumberStr, out long vehicleNumber))
                        {
                            var cmd = new NpgsqlCommand("INSERT INTO \"Vehicles\" (\"VehicleNumber\", \"VehicleType\") " +
                                "VALUES (@VehicleNumber, @VehicleType)" +
                                "ON CONFLICT (\"VehicleNumber\", \"VehicleType\") DO NOTHING " +
                                "RETURNING \"VehicleNumber\";", connection);
                            cmd.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                            cmd.Parameters.AddWithValue("@VehicleType", vehicleType);

                            if (cmd.ExecuteScalar() != null)
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }

                            var vehiclesData = RetrieveData("\"Vehicles\"");
                            gvarData.DicOfDT["Vehicles"] = vehiclesData;
                        }
                    }
                }
            }
        }

        public void UpdateVehicle(ref GVAR gvarData)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("VehicleNumber", out var vehicleNumberStr) &&
                        tagsData.TryGetValue("VehicleType", out var vehicleType) &&
                        tagsData.TryGetValue("VehicleID", out var vehicleIDStr))
                    {
                        if (long.TryParse(vehicleNumberStr, out long vehicleNumber) &&
                            long.TryParse(vehicleIDStr, out long vehicleID))
                        {
                            var cmd = new NpgsqlCommand("UPDATE \"Vehicles\" " +
                                "SET \"VehicleNumber\" = @VehicleNumber, " +
                                "\"VehicleType\" = @VehicleType " +
                                "WHERE \"VehicleID\" = @VehicleID " +
                                "AND (\"VehicleNumber\" IS DISTINCT FROM @VehicleNumber " +
                                "OR \"VehicleType\" IS DISTINCT FROM @VehicleType) " +
                                "RETURNING \"VehicleNumber\";",
                                connection);

                            cmd.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                            cmd.Parameters.AddWithValue("@VehicleType", vehicleType);
                            cmd.Parameters.AddWithValue("@VehicleID", vehicleID);

                            if (cmd.ExecuteScalar() != null)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            }
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }

                            var vehiclesData = RetrieveData("\"Vehicles\"");
                            gvarData.DicOfDT["Vehicles"] = vehiclesData;
                        }
                    }
                }
            }
        }

        public void DeleteVehicle(ref GVAR gvarData)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("VehicleID", out var vehicleIDStr))
                    {

                        if (long.TryParse(vehicleIDStr, out long vehicleID))
                        {
                            var cmd = new NpgsqlCommand("DELETE FROM \"Vehicles\" " +
                                "WHERE \"VehicleID\" = @VehicleID " +
                                "RETURNING \"VehicleID\";", connection);
                            cmd.Parameters.AddWithValue("@VehicleID", vehicleID);

                            if (cmd.ExecuteScalar() != null)
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }
                            var vehiclesData = RetrieveData("\"Vehicles\"");
                            gvarData.DicOfDT["Vehicles"] = vehiclesData;
                        }
                    }
                }
            }
        }

        public void GetDrivers(ref GVAR gvarData)
        {

            var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();


                    string query = "SELECT d.*, ve.* " +
                        "FROM \"Driver\" d " +
                        "JOIN \"VehiclesInformations\" ve ON d.\"DriverID\" = ve.\"DriverID\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();
                            if (reader != null)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            }
                            else gvarData.DicOfDic["Tags"].TryAdd("STS", "0");

                            while (reader.Read())
                            {
                                var rowDictionary = new ConcurrentDictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    rowDictionary[reader.GetName(i)] = reader[i].ToString();
                                }
                                result.Add(rowDictionary);
                            }
                            gvarData.DicOfDT["Driver"] = result.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        public void GetDriver(ref GVAR gvarData)
        {

            var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();


                    string query = "SELECT *  FROM \"Driver\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();
                            if (reader != null)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            }
                            else gvarData.DicOfDic["Tags"].TryAdd("STS", "0");

                            while (reader.Read())
                            {
                                var rowDictionary = new ConcurrentDictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    rowDictionary[reader.GetName(i)] = reader[i].ToString();
                                }
                                result.Add(rowDictionary);
                            }
                            gvarData.DicOfDT["Driver"] = result.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void AddDriver(ref GVAR gvarData)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("DriverName", out var driverName) &&
                        tagsData.TryGetValue("PhoneNumber", out var driverPhoneNumberSTR))
                    {

                        if (long.TryParse(driverPhoneNumberSTR, out long driverPhoneNumber))
                        {
                            var cmd = new NpgsqlCommand("INSERT INTO \"Driver\" " +
                                "(\"DriverName\", \"PhoneNumber\") " +
                                "VALUES (@DriverName, @PhoneNumber)" +
                                "ON CONFLICT (\"PhoneNumber\") DO NOTHING " +
                                "RETURNING \"DriverID\";", connection);
                            cmd.Parameters.AddWithValue("@DriverName", driverName);
                            cmd.Parameters.AddWithValue("@PhoneNumber", driverPhoneNumber);

                            if (cmd.ExecuteScalar() != null)
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }

                            var vehiclesData = RetrieveData("\"Driver\"");
                            gvarData.DicOfDT["Driver"] = vehiclesData;
                        }
                    }
                }
            }
        }

        public void UpdateDriver(ref GVAR gvarData)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("PhoneNumber", out var phoneNumberStr) &&
                        tagsData.TryGetValue("DriverName", out var driverName) &&
                        tagsData.TryGetValue("DriverID", out var driverIDStr))
                    {
                        if (long.TryParse(phoneNumberStr, out long phoneNumber) &&
                            long.TryParse(driverIDStr, out long driverID))
                        {
                            var cmd = new NpgsqlCommand("UPDATE \"Driver\" " +
                                "SET \"DriverName\" = @DriverName, " +
                                "\"PhoneNumber\" = @PhoneNumber " +
                                "WHERE \"DriverID\" = @DriverID " +
                                "AND (\"DriverName\" IS DISTINCT FROM @DriverName " +
                                "OR \"PhoneNumber\" IS DISTINCT FROM @PhoneNumber) " +
                                "RETURNING \"DriverID\";",
                                connection);

                            cmd.Parameters.AddWithValue("@DriverName", driverName);
                            cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            cmd.Parameters.AddWithValue("@DriverID", driverID);

                            if (cmd.ExecuteScalar() != null)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            }
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }

                            var vehiclesData = RetrieveData("\"Driver\"");
                            gvarData.DicOfDT["Driver"] = vehiclesData;
                        }
                    }
                }
            }
        }

        public void DeleteDriver(ref GVAR gvarData)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("DriverID", out var driverIDStr))
                    {

                        if (long.TryParse(driverIDStr, out long driverID))
                        {
                            var cmd = new NpgsqlCommand("DELETE FROM \"Driver\" " +
                                "WHERE \"DriverID\" = @DriverID " +
                                "RETURNING \"DriverID\";", connection);
                            cmd.Parameters.AddWithValue("@DriverID", driverID);

                            if (cmd.ExecuteScalar() != null)
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }
                            var vehiclesData = RetrieveData("\"Driver\"");
                            gvarData.DicOfDT["Driver"] = vehiclesData;
                        }
                    }
                }
            }
        }

        public void GetVehicleInfo(ref GVAR gvarData)
        {

            var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();


                    string query = "SELECT *  FROM \"VehiclesInformations\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();
                            if (reader != null)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            }
                            else gvarData.DicOfDic["Tags"].TryAdd("STS", "0");

                            while (reader.Read())
                            {
                                var rowDictionary = new ConcurrentDictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    rowDictionary[reader.GetName(i)] = reader[i].ToString();
                                }
                                result.Add(rowDictionary);
                            }
                            gvarData.DicOfDT["VehiclesInformations"] = result.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void AddVehiclesInformations(ref GVAR gvarData)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("DriverID", out var driverIDStr) &&
                        tagsData.TryGetValue("VehicleMake", out var vehicleMake) &&
                        tagsData.TryGetValue("VehicleModel", out var vehicleModel) &&
                        tagsData.TryGetValue("PurchaseDate", out var purchaseDateStr) &&
                        tagsData.TryGetValue("VehicleID", out var vehicleIDStr))
                    {

                        if (long.TryParse(driverIDStr, out long driverID) &&
                            long.TryParse(vehicleIDStr, out long vehicleID))
                        {
                            long purchaseDate = GetEpochTime(purchaseDateStr);

                            var cmd = new NpgsqlCommand("INSERT INTO \"VehiclesInformations\" " +
                                "(\"DriverID\", \"VehicleMake\", \"VehicleModel\", \"PurchaseDate\", \"VehicleID\") " +
                                "VALUES (@DriverID, @VehicleMake, @VehicleModel, @PurchaseDate, @VehicleID)" +
                                "ON CONFLICT (\"DriverID\", \"VehicleID\") DO NOTHING " +
                                "RETURNING \"ID\";", connection);

                            cmd.Parameters.AddWithValue("@DriverID", driverID);
                            cmd.Parameters.AddWithValue("@VehicleMake", vehicleMake);
                            cmd.Parameters.AddWithValue("@VehicleModel", vehicleModel);
                            cmd.Parameters.AddWithValue("@PurchaseDate", purchaseDate);
                            cmd.Parameters.AddWithValue("@VehicleID", vehicleID);

                            if (cmd.ExecuteScalar() != null)
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }

                            var vehiclesData = RetrieveData("\"VehiclesInformations\"");
                            gvarData.DicOfDT["VehiclesInformations"] = vehiclesData;
                        }
                    }
                }
            }
        }

        public void UpdateVehiclesInformation(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("DriverID", out var driverIDStr) &&
                        tagsData.TryGetValue("VehicleMake", out var vehicleMake) &&
                        tagsData.TryGetValue("VehicleModel", out var vehicleModel) &&
                        tagsData.TryGetValue("PurchaseDate", out var purchaseDateStr) &&
                        tagsData.TryGetValue("VehicleID", out var vehicleIDStr) &&
                        tagsData.TryGetValue("ID", out var IDStr))
                    {

                        if (long.TryParse(driverIDStr, out long driverID) &&
                            long.TryParse(vehicleIDStr, out long vehicleID) &&
                            long.TryParse(IDStr, out long ID))
                        {
                            long purchaseDate = GetEpochTime(purchaseDateStr);

                            var cmd = new NpgsqlCommand("UPDATE \"VehiclesInformations\" " +
                                "SET \"DriverID\" = @DriverID, " +
                                "\"VehicleMake\" = @VehicleMake, " +
                                "\"VehicleModel\" = @VehicleModel, " +
                                "\"PurchaseDate\" = @PurchaseDate, " +
                                "\"VehicleID\" = @VehicleID " +
                                "WHERE \"ID\" = @ID " +
                                "AND (\"DriverID\" IS DISTINCT FROM @DriverID " +
                                "OR \"VehicleMake\" IS DISTINCT FROM @VehicleMake " +
                                "OR \"VehicleModel\" IS DISTINCT FROM @VehicleModel " +
                                "OR \"PurchaseDate\" IS DISTINCT FROM @PurchaseDate " +
                                "OR \"VehicleID\" IS DISTINCT FROM @VehicleID )" +
                                "RETURNING \"ID\";", connection);

                            cmd.Parameters.AddWithValue("@DriverID", driverID);
                            cmd.Parameters.AddWithValue("@VehicleMake", vehicleMake);
                            cmd.Parameters.AddWithValue("@VehicleModel", vehicleModel);
                            cmd.Parameters.AddWithValue("@PurchaseDate", purchaseDate);
                            cmd.Parameters.AddWithValue("@VehicleID", vehicleID);
                            cmd.Parameters.AddWithValue("@ID", ID);

                            if (cmd.ExecuteScalar() != null)
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }

                            var vehiclesData = RetrieveData("\"VehiclesInformations\"");
                            gvarData.DicOfDT["VehiclesInformations"] = vehiclesData;
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DeleteVehiclesInformation(ref GVAR gvarData)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("ID", out var IDStr))
                    {

                        if (long.TryParse(IDStr, out long ID))
                        {
                            var cmd = new NpgsqlCommand("DELETE FROM \"VehiclesInformations\" " +
                                "WHERE \"ID\" = @ID " +
                                "RETURNING \"ID\";", connection);

                            cmd.Parameters.AddWithValue("@ID", ID);

                            if (cmd.ExecuteScalar() != null)
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }

                            var vehiclesData = RetrieveData("\"VehiclesInformations\"");
                            gvarData.DicOfDT["VehiclesInformations"] = vehiclesData;
                        }
                    }
                }
            }
        }

        public async Task<GVAR> AddRouteHistoryAsync(GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // Set the search path to public
                    using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                    {
                        if (tagsData.TryGetValue("VehicleDirection", out var vehicleDirectionStr) &&
                            tagsData.TryGetValue("Status", out var status) &&
                            tagsData.TryGetValue("VehicleSpeed", out var vehicleSpeed) &&
                            tagsData.TryGetValue("Address", out var address) &&
                            tagsData.TryGetValue("Latitude", out var latitudeStr) &&
                            tagsData.TryGetValue("Longitude", out var longitudeStr) &&
                            tagsData.TryGetValue("Epoch", out var epochStr) &&
                            tagsData.TryGetValue("VehicleID", out var vehicleIDStr)
                            )
                        {
                            if (int.TryParse(vehicleDirectionStr, out int vehicleDirection) &&
                                long.TryParse(vehicleIDStr, out long vehicleID) &&
                                float.TryParse(latitudeStr, out float latitude) &&
                                float.TryParse(longitudeStr, out float longitude))
                            {
                                var vehiclesPastData = RetrieveData("\"RouteHistory\"");
                                if (vehicleDirection < 0 || vehicleDirection > 360)
                                {
                                    Console.WriteLine("Vehicle Direction must be between 0 and 360.");
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                    gvarData.DicOfDT["RouteHistory"] = vehiclesPastData;
                                    return gvarData;
                                }
                                if (latitude < -90.0 || latitude > 90.0)
                                {
                                    Console.WriteLine("Latitude must be between -90.0 and 90.0.");
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                    gvarData.DicOfDT["RouteHistory"] = vehiclesPastData;
                                    return gvarData;
                                }
                                if (longitude < -180.0 || longitude > 180.0)
                                {
                                    Console.WriteLine("Longitude must be between -180.0 and 180.0.");
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                    gvarData.DicOfDT["RouteHistory"] = vehiclesPastData;
                                    return gvarData;
                                }
                                if (!Enum.TryParse(status, out VehicleStatus statusCode))
                                {
                                    Console.WriteLine("Status value must be either Stopped, Moving, " +
                                        "Idle, OutOfService, or Maintenance.");
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                    gvarData.DicOfDT["RouteHistory"] = vehiclesPastData;
                                    return gvarData;
                                }
                                long epoch = GetEpochTime(epochStr);

                                var cmd = new NpgsqlCommand("INSERT INTO \"RouteHistory\" " +
                                    "(\"VehicleDirection\", \"Status\", \"VehicleSpeed\", \"Address\", " +
                                    "\"Latitude\", \"Longitude\", \"Epoch\", \"VehicleID\") " +
                                    "VALUES (@VehicleDirection, @Status, @VehicleSpeed, @Address, @Latitude, " +
                                    "@Longitude, @Epoch, @VehicleID) " +
                                    "RETURNING \"RouteHistoryID\";", connection);

                                cmd.Parameters.AddWithValue("@VehicleDirection", vehicleDirection);
                                cmd.Parameters.AddWithValue("@Status", statusCode.ToString());
                                cmd.Parameters.AddWithValue("@VehicleSpeed", vehicleSpeed);
                                cmd.Parameters.AddWithValue("@Address", address);
                                cmd.Parameters.AddWithValue("@Latitude", latitude);
                                cmd.Parameters.AddWithValue("@Longitude", longitude);
                                cmd.Parameters.AddWithValue("@Epoch", epoch);
                                cmd.Parameters.AddWithValue("@VehicleID", vehicleID);

                                if (cmd.ExecuteScalar() != null)
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                else
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                }

                                var vehiclesData = RetrieveData("\"RouteHistory\"");
                                gvarData.DicOfDT["RouteHistory"] = vehiclesData;

                                // Broadcast the GVAR data to all connected clients
                                var jsonData = JsonConvert.SerializeObject(gvarData);
                                await _webSocketManager.BroadcastMessageAsync(jsonData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return gvarData;
        }

        public void GetVehicleInformations(ref GVAR gvarData)
        {

            var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    string query =
                        "SELECT " +
                            "v.\"VehicleID\", " +
                            "v.\"VehicleNumber\", " +
                            "v.\"VehicleType\", " +
                            "rh.\"VehicleDirection\" AS \"LastDirection\", " +
                            "rh.\"Status\" AS \"LastStatus\", " +
                            "rh.\"Address\" AS \"LastAddress\", " +
                            "rh.\"Latitude\", " +
                            "rh.\"Longitude\" " +
                        "FROM " +
                            "\"Vehicles\" v " +
                        "JOIN " +
                            "\"RouteHistory\" rh ON v.\"VehicleID\" = rh.\"VehicleID\" " +
                        "JOIN ( " +
                            "SELECT " +
                                "\"VehicleID\", MAX(\"Epoch\") AS \"LastEpoch\" " +
                            "FROM " +
                                "\"RouteHistory\" " +
                            "GROUP BY " +
                                "\"VehicleID\" " +
                            ") latest ON rh.\"VehicleID\" = latest.\"VehicleID\" AND rh.\"Epoch\" = latest.\"LastEpoch\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();
                            if (reader != null)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                            }
                            else gvarData.DicOfDic["Tags"].TryAdd("STS", "0");

                            while (reader.Read())
                            {
                                var rowDictionary = new ConcurrentDictionary<string, string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    rowDictionary[reader.GetName(i)] = reader[i].ToString();
                                }
                                result.Add(rowDictionary);
                            }
                            gvarData.DicOfDT["VehiclesInformations"] = result.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void GetVehicleInformation(ref GVAR gvarData)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();

                // Set the search path to public
                using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                {
                    cmd.ExecuteNonQuery();
                }

                if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                {
                    if (tagsData.TryGetValue("VehicleID", out var VehicleIDStr))
                    {

                        if (long.TryParse(VehicleIDStr, out long VehicleID))
                        {
                            string query =
                                "SELECT " +
                                    "v.\"VehicleNumber\", " +
                                    "v.\"VehicleType\", " +
                                    "d.\"DriverName\", " +
                                    "d.\"PhoneNumber\", " +
                                    "CONCAT(rh.\"Latitude\", ', ', rh.\"Longitude\") AS \"LastPosition\", " +
                                    "vi.\"VehicleMake\", " +
                                    "vi.\"VehicleModel\", " +
                                    "rh.\"Epoch\" AS \"LastGPSTime\", " +
                                    "rh.\"VehicleSpeed\" AS \"LastGPSSpeed\", " +
                                    "rh.\"Address\" AS \"LastAddress\" " +
                                "FROM " +
                                    "public.\"Vehicles\" v " +
                                "JOIN " +
                                    "public.\"VehiclesInformations\" vi ON v.\"VehicleID\" = vi.\"VehicleID\" " +
                                "JOIN " +
                                    "public.\"Driver\" d ON vi.\"DriverID\" = d.\"DriverID\" " +
                                "JOIN " +
                                    "public.\"RouteHistory\" rh ON v.\"VehicleID\" = rh.\"VehicleID\" " +
                                "JOIN ( " +
                                    "SELECT " +
                                        "\"VehicleID\", " +
                                        "MAX(\"Epoch\") AS \"LastEpoch\" " +
                                    "FROM " +
                                        "public.\"RouteHistory\" " +
                                    "GROUP BY " +
                                        "\"VehicleID\" " +
                                ") latest ON rh.\"VehicleID\" = latest.\"VehicleID\" AND rh.\"Epoch\" = latest.\"LastEpoch\" " +
                                "WHERE " +
                                    "v.\"VehicleID\" = @VehicleID;";
                            var cmd = new NpgsqlCommand(query, connection);

                            cmd.Parameters.AddWithValue("@VehicleID", VehicleID);

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                    var resultDict = new ConcurrentDictionary<string, string>
                                    {
                                        ["VehicleNumber"] = reader["VehicleNumber"].ToString(),
                                        ["VehicleType"] = reader["VehicleType"].ToString(),
                                        ["DriverName"] = reader["DriverName"].ToString(),
                                        ["PhoneNumber"] = reader["PhoneNumber"].ToString(),
                                        ["LastPosition"] = reader["LastPosition"].ToString(),
                                        ["VehicleMake"] = reader["VehicleMake"].ToString(),
                                        ["VehicleModel"] = reader["VehicleModel"].ToString(),
                                        ["LastGPSTime"] = reader["LastGPSTime"].ToString(),
                                        ["LastGPSSpeed"] = reader["LastGPSSpeed"].ToString(),
                                        ["LastAddress"] = reader["LastAddress"].ToString()
                                    };

                                    gvarData.DicOfDT["VehiclesInformations"] = new[] { resultDict };
                                }
                                else gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }

                        }
                    }
                }
            }
        }

        public void GetVehicleRouteHistory(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // Set the search path to public
                    using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                    {
                        if (tagsData.TryGetValue("VehicleID", out var VehicleIDStr) &&
                            tagsData.TryGetValue("StartEpoch", out var StartEpochStr) &&
                            tagsData.TryGetValue("EndEpoch", out var EndEpochStr))
                        {

                            if (long.TryParse(VehicleIDStr, out long VehicleID) &&
                                long.TryParse(StartEpochStr, out long StartEpoch) &&
                                long.TryParse(EndEpochStr, out long EndEpoch))
                            {
                                string query =
                                    "SELECT " +
                                        "v.\"VehicleID\", " +
                                        "v.\"VehicleNumber\", " +
                                        "rh.\"Address\", " +
                                        "rh.\"Status\", " +
                                        "rh.\"Latitude\", " +
                                        "rh.\"Longitude\", " +
                                        "rh.\"VehicleDirection\", " +
                                        "rh.\"VehicleSpeed\" AS \"GPSSpeed\", " +
                                        "rh.\"Epoch\" AS \"GPSTime\" " +
                                    "FROM " +
                                        "\"Vehicles\" v " +
                                    "JOIN " +
                                        "\"RouteHistory\" rh ON v.\"VehicleID\" = rh.\"VehicleID\" " +
                                    "WHERE " +
                                        "v.\"VehicleID\" = @VehicleID " +
                                        "AND rh.\"Epoch\" BETWEEN @StartEpoch AND @EndEpoch " +
                                    "ORDER BY " +
                                        "rh.\"Epoch\";";

                                var cmd = new NpgsqlCommand(query, connection);

                                cmd.Parameters.AddWithValue("@VehicleID", VehicleID);
                                cmd.Parameters.AddWithValue("@StartEpoch", StartEpoch);
                                cmd.Parameters.AddWithValue("@EndEpoch", EndEpoch);

                                using (var reader = cmd.ExecuteReader())
                                {
                                    var routeHistory = new List<ConcurrentDictionary<string, string>>();

                                    while (reader.Read())
                                    {
                                        gvarData.DicOfDic["Tags"].TryAdd("STS", "1");

                                        var resultDict = new ConcurrentDictionary<string, string>
                                        {
                                            ["VehicleNumber"] = reader["VehicleNumber"].ToString(),
                                            ["Address"] = reader["Address"].ToString(),
                                            ["Status"] = reader["Status"].ToString(),
                                            ["Latitude"] = reader["Latitude"].ToString(),
                                            ["Longitude"] = reader["Longitude"].ToString(),
                                            ["VehicleDirection"] = reader["VehicleDirection"].ToString(),
                                            ["GPSSpeed"] = reader["GPSSpeed"].ToString(),
                                            ["GPSTime"] = reader["GPSTime"].ToString(),
                                        };

                                        routeHistory.Add(resultDict);
                                    }
                                    if (routeHistory.Count > 0)
                                    {
                                        gvarData.DicOfDT["RouteHistory"] = routeHistory.ToArray();
                                    }
                                    else
                                    {
                                        gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                    }
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void GetGeofenceInformation(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // Set the search path to public
                    using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }


                    string query = "SELECT * FROM \"Geofences\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var geofenceList = new List<ConcurrentDictionary<string, string>>();

                            while (reader.Read())
                            {
                                var resultDict = new ConcurrentDictionary<string, string>
                                {
                                    ["GeofenceID"] = reader["GeofenceID"].ToString(),
                                    ["GeofenceType"] = reader["GeofenceType"].ToString(),
                                    ["AddedDate"] = reader["AddedDate"].ToString(),
                                    ["StrockColor"] = reader["StrockColor"].ToString(),
                                    ["StrockOpacity"] = reader["StrockOpacity"].ToString(),
                                    ["StrockWeight"] = reader["StrockWeight"].ToString(),
                                    ["FillColor"] = reader["FillColor"].ToString(),
                                    ["FillOpacity"] = reader["FillOpacity"].ToString(),
                                };

                                geofenceList.Add(resultDict);
                            }

                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();

                            if (geofenceList.Count > 0)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                gvarData.DicOfDT["Geofences"] = geofenceList.ToArray();
                            }
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetCircleGeofence(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // Set the search path to public
                    using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }


                    string query = "SELECT * FROM \"CircleGeofence\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var geofenceList = new List<ConcurrentDictionary<string, string>>();

                            while (reader.Read())
                            {
                                var resultDict = new ConcurrentDictionary<string, string>
                                {
                                    ["GeofenceID"] = reader["GeofenceID"].ToString(),
                                    ["Radius"] = reader["Radius"].ToString(),
                                    ["Latitude"] = reader["Latitude"].ToString(),
                                    ["Longitude"] = reader["Longitude"].ToString(),
                                };

                                geofenceList.Add(resultDict);
                            }

                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();

                            if (geofenceList.Count > 0)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                gvarData.DicOfDT["CircleGeofence"] = geofenceList.ToArray();
                            }
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetPolygonGeofence(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // Set the search path to public
                    using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }


                    string query = "SELECT * FROM \"PolygonGeofence\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var geofenceList = new List<ConcurrentDictionary<string, string>>();

                            while (reader.Read())
                            {
                                var resultDict = new ConcurrentDictionary<string, string>
                                {
                                    ["GeofenceID"] = reader["GeofenceID"].ToString(),
                                    ["Latitude"] = reader["Latitude"].ToString(),
                                    ["Longitude"] = reader["Longitude"].ToString(),
                                };

                                geofenceList.Add(resultDict);
                            }

                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();

                            if (geofenceList.Count > 0)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                gvarData.DicOfDT["PolygonGeofence"] = geofenceList.ToArray();
                            }
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void GetRectangleGeofence(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    // Set the search path to public
                    using (var cmd = new NpgsqlCommand("SET search_path TO public;", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }


                    string query = "SELECT * FROM \"RectangleGeofence\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            var geofenceList = new List<ConcurrentDictionary<string, string>>();

                            while (reader.Read())
                            {
                                var resultDict = new ConcurrentDictionary<string, string>
                                {
                                    ["GeofenceID"] = reader["GeofenceID"].ToString(),
                                    ["North"] = reader["North"].ToString(),
                                    ["East"] = reader["East"].ToString(),
                                    ["West"] = reader["West"].ToString(),
                                    ["South"] = reader["South"].ToString(),
                                };

                                geofenceList.Add(resultDict);
                            }

                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();

                            if (geofenceList.Count > 0)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                gvarData.DicOfDT["RectangleGeofence"] = geofenceList.ToArray();
                            }
                            else
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}