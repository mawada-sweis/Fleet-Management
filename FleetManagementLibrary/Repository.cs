using FleetManagementShared;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Concurrent;

namespace FleetManagementLibrary
{

    public class GVAR
    {
        /// <summary>
        /// GVAR class is used to send and receive data to and either from the backend or frontend.
        /// It contains two dictionaries:
        /// 1. DicOfDic: A dictionary of dictionaries to store request data as string, 
        /// the data stored in "Tags" key, example of GVAR Json Request:
        /// {
        ///   "DicOfDic" : {
        ///             "Tags":{
        ///                 "VehicleID": "26",
        ///                 "VehicleDirection": "100",
        ///             }
        ///   }
        /// }
        /// 2. DicOfDT: A dictionary that stores arrays of dictionaries with table's data as a response, 
        /// and "STS" for successful status, example of GVAR Json Response:
        /// {
        ///     "DicOfDic": {
        ///             "Tags": {
        ///                 "STS": "1", // 1 means successfull, 0 failed
        ///                 "Address": "Main",
        ///                 "VehicleSpeed": "90 km/h"
        ///             }
        ///     },
        ///     "DicOfDT": {
        ///         "RouteHistory": // table name
        ///         [
        ///             {
        ///               "Address": "Main",
        ///               "VehicleID": "26",
        ///             },
        ///             {
        ///               "Address": "Main",
        ///               "VehicleID": "25,
        ///             },
        ///         ]
        ///     }
        /// }
        /// </summary>
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

        /// <summary>
        /// Retrieves data from the specified table in the database and returns it as an array of dictionaries.
        /// Each dictionary represents a row with column names as keys and column values as values.
        /// The table name is enclosed in double quotes to prevent SQL injection.
        /// </summary>
        /// <param name="tableName">The name of the table to retrieve data from, enclosed in double quotes. 
        ///     Should be one of the following:
        ///     Vehicles, VehiclesInformations, RouteHistory, Driver, Geofences, CircleGeofence, PolygonGeofence, or RectangleGeofence.
        /// </param>
        /// <returns>An array of dictionaries representing the rows in the table.</returns>
        private ConcurrentDictionary<string, string>[] RetrieveData(string tableName)
        {
            var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();
                    string query = $"SELECT * FROM {tableName};";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var rowDictionary = new ConcurrentDictionary<string, string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        rowDictionary[reader.GetName(i)] = reader[i]?.ToString() ?? string.Empty;
                                    }
                                    result.Add(rowDictionary);
                                }
                                Console.WriteLine($"{tableName} Retrieved.");
                            }
                            else
                            {
                                Console.WriteLine($"{tableName} has no data.");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when retrieve data from {tableName}: {ex.Message}");
            }

            return result.ToArray();
        }


        /// <summary>
        /// Converts a given date string to epoch time (seconds since Unix epoch).
        /// </summary>
        /// <param name="date">The date string in the format "yyyy-MM-dd HH:mm:ss".</param>
        /// <returns>The epoch time as a long value.</returns>
        private long GetEpochTime(string date)
        {
            long epochTime = 0;
            try
            {
                DateTime dateTime = DateTime.ParseExact(date, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime, TimeSpan.Zero);

                epochTime = dateTimeOffset.ToUnixTimeSeconds();

                Console.WriteLine($"Converted {date} to epoch time {epochTime}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occures when parsing date to epoch time {ex.Message}");
            }
            return epochTime;
        }

        /// <summary>
        /// Retrieves data from the "Vehicles" table in the database and stores it in the provided GVAR object.
        /// The method sets a status tag in the GVAR object indicating whether data retrieval was successful.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetVehicles(ref GVAR gvarData)
        {
            var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM \"Vehicles\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();
                            if (reader != null && reader.HasRows)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");

                                while (reader.Read())
                                {
                                    var rowDictionary = new ConcurrentDictionary<string, string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        rowDictionary[reader.GetName(i)] = reader[i]?.ToString() ?? string.Empty;
                                    }
                                    result.Add(rowDictionary);
                                }
                                gvarData.DicOfDT["Vehicles"] = result.ToArray();
                                Console.WriteLine("Fetch data from Vehicles table successfully.");
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
                Console.WriteLine($"An error occurred when fetching data from Vehicles table: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new vehicle to the "Vehicles" table in the database.
        /// The vehicle details are taken from the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the latest vehicles data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing vehicle details and where the status and updated data will be stored.</param>
        public void AddVehicle(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                    {
                        if (tagsData.TryGetValue("VehicleNumber", out var vehicleNumberStr) &&
                            tagsData.TryGetValue("VehicleType", out var vehicleType))
                        {
                            if (long.TryParse(vehicleNumberStr, out long vehicleNumber))
                            {
                                var query = "INSERT INTO \"Vehicles\" (\"VehicleNumber\", \"VehicleType\") " +
                                    "VALUES (@VehicleNumber, @VehicleType)" +
                                    "ON CONFLICT (\"VehicleNumber\", \"VehicleType\") DO NOTHING " +
                                    "RETURNING \"VehicleNumber\";";
                                var cmd = new NpgsqlCommand(query, connection);
                                cmd.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                                cmd.Parameters.AddWithValue("@VehicleType", vehicleType);

                                if (cmd.ExecuteScalar() != null)
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                    var vehiclesData = RetrieveData("\"Vehicles\"");
                                    gvarData.DicOfDT["Vehicles"] = vehiclesData;
                                    Console.WriteLine("Add data to Vehicles table successfully.");
                                }
                                else
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Vehicle Number must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include VehicleNumber and VehicleType values.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when adding data to Vehicles table: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing vehicle in the "Vehicles" table in the database.
        /// The vehicle details are taken from the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the latest vehicles data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing vehicle details and where the status and updated data will be stored.</param>
        public void UpdateVehicle(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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
                                var query = "UPDATE \"Vehicles\" " +
                                    "SET \"VehicleNumber\" = @VehicleNumber, \"VehicleType\" = @VehicleType " +
                                    "WHERE \"VehicleID\" = @VehicleID " +
                                    "AND (\"VehicleNumber\" IS DISTINCT FROM @VehicleNumber " +
                                         "OR \"VehicleType\" IS DISTINCT FROM @VehicleType) " +
                                    "RETURNING \"VehicleNumber\";";

                                var cmd = new NpgsqlCommand(query, connection);

                                cmd.Parameters.AddWithValue("@VehicleNumber", vehicleNumber);
                                cmd.Parameters.AddWithValue("@VehicleType", vehicleType);
                                cmd.Parameters.AddWithValue("@VehicleID", vehicleID);

                                if (cmd.ExecuteScalar() != null)
                                {
                                    gvarData.DicOfDic["Tags"]["STS"] = "1";
                                    var vehiclesData = RetrieveData("\"Vehicles\"");
                                    gvarData.DicOfDT["Vehicles"] = vehiclesData;
                                    Console.WriteLine("Update data in Vehicles table successfully.");
                                }

                                else
                                {
                                    gvarData.DicOfDic["Tags"]["STS"] = "0";
                                }
                            }
                            else
                            {
                                Console.WriteLine("Vehicle Number and vehicle ID must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include VehicleNumber, VehicleID and VehicleType values.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when updating data in Vehicles table: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a vehicle from the "Vehicles" table in the database.
        /// The vehicle ID is taken from the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the latest vehicles data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing the vehicle ID and where the status and updated data will be stored.</param>
        public void DeleteVehicle(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                    {
                        if (tagsData.TryGetValue("VehicleID", out var vehicleIDStr))
                        {
                            if (long.TryParse(vehicleIDStr, out long vehicleID))
                            {
                                var query = "DELETE FROM \"Vehicles\" " +
                                            "WHERE \"VehicleID\" = @VehicleID " +
                                            "RETURNING \"VehicleID\";";
                                var cmd = new NpgsqlCommand(query, connection);
                                cmd.Parameters.AddWithValue("@VehicleID", vehicleID);

                                if (cmd.ExecuteScalar() != null)
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                    Console.WriteLine("Delete data from Vehicles table successfully.");
                                }
                                else
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                }

                                var vehiclesData = RetrieveData("\"Vehicles\"");
                                gvarData.DicOfDT["Vehicles"] = vehiclesData;
                            }
                            else
                            {
                                Console.WriteLine("Vehicle ID must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include VehicleID value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when deleting data from Vehicles table: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves data from the "Driver" and "VehiclesInformations" tables in the database.
        /// Joins the data based on DriverID and stores the results in the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the retrieved data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetDrivers(ref GVAR gvarData)
        {
            try
            {
                var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
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
                            if (reader != null && reader.HasRows)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");

                                while (reader.Read())
                                {
                                    var rowDictionary = new ConcurrentDictionary<string, string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        rowDictionary[reader.GetName(i)] = reader[i]?.ToString() ?? string.Empty;
                                    }
                                    result.Add(rowDictionary);
                                }
                                gvarData.DicOfDT["Driver"] = result.ToArray();
                                Console.WriteLine("Fetch data from Driver and VehicleInformation tables successfully.");
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
                Console.WriteLine($"An error occurred when fetching data from Driver and VehicleInformation tables: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves data from the "Driver" table in the database and stores it in the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the retrieved data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetDriver(ref GVAR gvarData)
        {
            try
            {
                var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM \"Driver\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();
                            if (reader != null && reader.HasRows)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                while (reader.Read())
                                {
                                    var rowDictionary = new ConcurrentDictionary<string, string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        rowDictionary[reader.GetName(i)] = reader[i]?.ToString() ?? string.Empty;
                                    }
                                    result.Add(rowDictionary);
                                }
                                gvarData.DicOfDT["Driver"] = result.ToArray();
                                Console.WriteLine("Fetch data from Driver table successfully.");
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
                Console.WriteLine($"An error occurred when fetching data from Driver table: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new driver to the "Driver" table in the database.
        /// The driver details are taken from the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the latest drivers data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing driver details and where the status and updated data will be stored.</param>
        public void AddDriver(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                    {
                        if (tagsData.TryGetValue("DriverName", out var driverName) &&
                            tagsData.TryGetValue("PhoneNumber", out var driverPhoneNumberSTR))
                        {
                            if (long.TryParse(driverPhoneNumberSTR, out long driverPhoneNumber))
                            {
                                var query = "INSERT INTO \"Driver\" " +
                                            "(\"DriverName\", \"PhoneNumber\") " +
                                            "VALUES (@DriverName, @PhoneNumber)" +
                                            "ON CONFLICT (\"PhoneNumber\") DO NOTHING " +
                                            "RETURNING \"DriverID\";";
                                var cmd = new NpgsqlCommand(query, connection);
                                cmd.Parameters.AddWithValue("@DriverName", driverName);
                                cmd.Parameters.AddWithValue("@PhoneNumber", driverPhoneNumber);

                                if (cmd.ExecuteScalar() != null)
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                    var vehiclesData = RetrieveData("\"Driver\"");
                                    gvarData.DicOfDT["Driver"] = vehiclesData;
                                    Console.WriteLine("Added data to Driver table successfully.");
                                }
                                else
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                }

                            }
                            else
                            {
                                Console.WriteLine("Driver phone number must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include DriverName and PhoneNumber values.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when adding data to Driver table: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing driver in the "Driver" table in the database.
        /// The driver details are taken from the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the latest drivers data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing driver details and where the status and updated data will be stored.</param>
        public void UpdateDriver(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                    {
                        if (tagsData.TryGetValue("PhoneNumber", out var phoneNumberStr) &&
                            tagsData.TryGetValue("DriverName", out var driverName) &&
                            tagsData.TryGetValue("DriverID", out var driverIDStr))
                        {
                            if (long.TryParse(phoneNumberStr, out long phoneNumber) &&
                                long.TryParse(driverIDStr, out long driverID))
                            {
                                var query = "UPDATE \"Driver\" " +
                                            "SET \"DriverName\" = @DriverName, " +
                                            "\"PhoneNumber\" = @PhoneNumber " +
                                            "WHERE \"DriverID\" = @DriverID " +
                                            "AND (\"DriverName\" IS DISTINCT FROM @DriverName " +
                                            "OR \"PhoneNumber\" IS DISTINCT FROM @PhoneNumber) " +
                                            "RETURNING \"DriverID\";";
                                var cmd = new NpgsqlCommand(query, connection);

                                cmd.Parameters.AddWithValue("@DriverName", driverName);
                                cmd.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                                cmd.Parameters.AddWithValue("@DriverID", driverID);

                                if (cmd.ExecuteScalar() != null)
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                    var vehiclesData = RetrieveData("\"Driver\"");
                                    gvarData.DicOfDT["Driver"] = vehiclesData;
                                    Console.WriteLine("Update data in Driver table successfully.");
                                }
                                else
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Driver phone number and driver ID must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include PhoneNumber, DriverName and DriverID values.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when updating data in Driver table: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a driver from the "Driver" table in the database.
        /// The driver ID is taken from the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the latest drivers data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing the driver ID and where the status and updated data will be stored.</param>
        public void DeleteDriver(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                    {
                        if (tagsData.TryGetValue("DriverID", out var driverIDStr))
                        {

                            if (long.TryParse(driverIDStr, out long driverID))
                            {
                                var query = "DELETE FROM \"Driver\" " +
                                            "WHERE \"DriverID\" = @DriverID " +
                                            "RETURNING \"DriverID\";";
                                var cmd = new NpgsqlCommand(query, connection);
                                cmd.Parameters.AddWithValue("@DriverID", driverID);

                                if (cmd.ExecuteScalar() != null)
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                    var vehiclesData = RetrieveData("\"Driver\"");
                                    gvarData.DicOfDT["Driver"] = vehiclesData;
                                    Console.WriteLine("Delete data from Driver table successfully.");
                                }
                                else
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Driver ID must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include DriverID value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when deleting data from Driver table: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves data from the "VehiclesInformations" table in the database and stores it in the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the retrieved data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetVehicleInfo(ref GVAR gvarData)
        {
            try
            {
                var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM \"VehiclesInformations\";";

                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();
                            if (reader != null && reader.HasRows)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                while (reader.Read())
                                {
                                    var rowDictionary = new ConcurrentDictionary<string, string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        rowDictionary[reader.GetName(i)] = reader[i]?.ToString() ?? string.Empty;
                                    }
                                    result.Add(rowDictionary);
                                }
                                gvarData.DicOfDT["VehiclesInformations"] = result.ToArray();
                                Console.WriteLine("Fetch data from VehicleInformation table successfully.");
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
                Console.WriteLine($"An error occurred when fetching data from VehicleInformation table: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new vehicle information entry to the "VehiclesInformations" table in the database.
        /// The vehicle details are taken from the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the latest vehicle information data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing vehicle details and where the status and updated data will be stored.</param>
        public void AddVehiclesInformations(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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
                                var query = "INSERT INTO \"VehiclesInformations\" " +
                                            "(\"DriverID\", \"VehicleMake\", \"VehicleModel\", \"PurchaseDate\", \"VehicleID\") " +
                                            "VALUES (@DriverID, @VehicleMake, @VehicleModel, @PurchaseDate, @VehicleID)" +
                                            "ON CONFLICT (\"DriverID\", \"VehicleID\") DO NOTHING " +
                                            "RETURNING \"ID\";";
                                var cmd = new NpgsqlCommand(query, connection);

                                cmd.Parameters.AddWithValue("@DriverID", driverID);
                                cmd.Parameters.AddWithValue("@VehicleMake", vehicleMake);
                                cmd.Parameters.AddWithValue("@VehicleModel", vehicleModel);
                                cmd.Parameters.AddWithValue("@PurchaseDate", purchaseDate);
                                cmd.Parameters.AddWithValue("@VehicleID", vehicleID);

                                if (cmd.ExecuteScalar() != null)
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                    var vehiclesData = RetrieveData("\"VehiclesInformations\"");
                                    gvarData.DicOfDT["VehiclesInformations"] = vehiclesData;
                                    Console.WriteLine("Add data to Vehicle Information table successfully.");
                                }
                                else
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                }

                            }
                            else
                            {
                                Console.WriteLine("Driver ID and vehicle ID must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include DriverID, VehicleMake, VehicleModel, PurchaseDate and VehicleID values.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when adding data to Vehicle Information table: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing vehicle information entry in the "VehiclesInformations" table in the database.
        /// The vehicle details are taken from the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the latest vehicle information data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing vehicle details and where the status and updated data will be stored.</param>
        public void UpdateVehiclesInformation(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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

                                var query = "UPDATE \"VehiclesInformations\" " +
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
                                            "RETURNING \"ID\";";
                                var cmd = new NpgsqlCommand(query, connection);

                                cmd.Parameters.AddWithValue("@DriverID", driverID);
                                cmd.Parameters.AddWithValue("@VehicleMake", vehicleMake);
                                cmd.Parameters.AddWithValue("@VehicleModel", vehicleModel);
                                cmd.Parameters.AddWithValue("@PurchaseDate", purchaseDate);
                                cmd.Parameters.AddWithValue("@VehicleID", vehicleID);
                                cmd.Parameters.AddWithValue("@ID", ID);

                                if (cmd.ExecuteScalar() != null)
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                    var vehiclesData = RetrieveData("\"VehiclesInformations\"");
                                    gvarData.DicOfDT["VehiclesInformations"] = vehiclesData;
                                    Console.WriteLine("Update data in Vehicle Information table successfully.");
                                }
                                else
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Driver ID, vehicle ID and ID must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include DriverID, VehicleMake, VehicleModel, PurchaseDate, VehicleID, and ID values.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when updating data in Vehicle Information table: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a vehicle information entry from the "VehiclesInformations" table in the database.
        /// The entry ID is taken from the provided GVAR object.
        /// Updates the GVAR object with the status of the operation and the latest vehicle information data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing the entry ID and where the status and updated data will be stored.</param>
        public void DeleteVehiclesInformation(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

                    if (gvarData.DicOfDic.TryGetValue("Tags", out var tagsData))
                    {
                        if (tagsData.TryGetValue("ID", out var IDStr))
                        {
                            if (long.TryParse(IDStr, out long ID))
                            {
                                var query = "DELETE FROM \"VehiclesInformations\" " +
                                            "WHERE \"ID\" = @ID " +
                                            "RETURNING \"ID\";";
                                var cmd = new NpgsqlCommand(query, connection);

                                cmd.Parameters.AddWithValue("@ID", ID);

                                if (cmd.ExecuteScalar() != null)
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                    var vehiclesData = RetrieveData("\"VehiclesInformations\"");
                                    gvarData.DicOfDT["VehiclesInformations"] = vehiclesData;
                                    Console.WriteLine("Delete data from Vehicle Information table successfully.");
                                }
                                else
                                {
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                }
                            }
                            else
                            {
                                Console.WriteLine("ID must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include ID value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when deleting data from Vehicle Information table: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new route history entry to the "RouteHistory" table in the database asynchronously.
        /// The route details are taken from the provided GVAR object.
        /// Validates the input data and updates the GVAR object with the status of the operation and the latest route history data.
        /// Broadcasts the updated GVAR data to all connected clients.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object containing route details and where the status and updated data will be stored.</param>
        /// <returns>A task that represents the asynchronous operation, containing the updated GVAR object.</returns>
        public async Task<GVAR> AddRouteHistoryAsync(GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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
                                if (!Enum.TryParse(status, true, out VehicleStatus statusCode))
                                {
                                    Console.WriteLine("Status value must be either Stopped, Moving, " +
                                        "Idle, OutOfService, or Maintenance.");
                                    gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                    gvarData.DicOfDT["RouteHistory"] = vehiclesPastData;
                                    return gvarData;
                                }
                                else
                                {
                                    var statusCodeInt = (int)statusCode;
                                    long epoch = GetEpochTime(epochStr);
                                    var query = "INSERT INTO \"RouteHistory\" " +
                                                "(\"VehicleDirection\", \"Status\", \"VehicleSpeed\", \"Address\", " +
                                                "\"Latitude\", \"Longitude\", \"Epoch\", \"VehicleID\") " +
                                                "VALUES (@VehicleDirection, @Status, @VehicleSpeed, @Address, @Latitude, " +
                                                "@Longitude, @Epoch, @VehicleID) " +
                                                "RETURNING \"RouteHistoryID\";";
                                    var cmd = new NpgsqlCommand(query, connection);

                                    cmd.Parameters.AddWithValue("@VehicleDirection", vehicleDirection);
                                    cmd.Parameters.AddWithValue("@Status", statusCodeInt.ToString());
                                    cmd.Parameters.AddWithValue("@VehicleSpeed", vehicleSpeed);
                                    cmd.Parameters.AddWithValue("@Address", address);
                                    cmd.Parameters.AddWithValue("@Latitude", latitude);
                                    cmd.Parameters.AddWithValue("@Longitude", longitude);
                                    cmd.Parameters.AddWithValue("@Epoch", epoch);
                                    cmd.Parameters.AddWithValue("@VehicleID", vehicleID);

                                    if (cmd.ExecuteScalar() != null)
                                    {
                                        gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                        var vehiclesData = RetrieveData("\"RouteHistory\"");
                                        gvarData.DicOfDT["RouteHistory"] = vehiclesData;

                                        // Broadcast the GVAR data to all connected clients
                                        var jsonData = JsonConvert.SerializeObject(gvarData);
                                        await _webSocketManager.BroadcastMessageAsync(jsonData);
                                        Console.WriteLine("Add data to Route History table successfully.");
                                    }
                                    else
                                    {
                                        gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("vehicle Direction, vehicle ID, latitude, and longitude must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include VehicleDirection, Status, VehicleSpeed, Address, Latitude, Longitude, Epoch, and VehicleID value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when adding data to Route History table: {ex.Message}");
            }
            return gvarData;
        }

        /// <summary>
        /// Retrieves comprehensive vehicle information from the database, including details from the "Vehicles" and "RouteHistory" tables.
        /// Updates the GVAR object with the status of the operation and the retrieved data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetVehicleInformations(ref GVAR gvarData)
        {
            try
            {
                var result = new ConcurrentBag<ConcurrentDictionary<string, string>>();
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
                                while (reader.Read())
                                {
                                    var rowDictionary = new ConcurrentDictionary<string, string>();
                                    for (int i = 0; i < reader.FieldCount; i++)
                                    {
                                        rowDictionary[reader.GetName(i)] = reader[i]?.ToString() ?? string.Empty;
                                    }
                                    result.Add(rowDictionary);
                                }
                                gvarData.DicOfDT["VehiclesInformations"] = result.ToArray();
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                Console.WriteLine("Fetch data from Vehicle Information table and other tables successfully.");
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
                Console.WriteLine($"An error occurred when fetching data from Vehicle Information table and other tables: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves specific vehicle information from the database based on the provided VehicleID.
        /// Updates the GVAR object with the status of the operation and the retrieved data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetVehicleInformation(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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
                                        var resultDict = new ConcurrentDictionary<string, string>
                                        {
                                            ["VehicleNumber"] = reader["VehicleNumber"]?.ToString() ?? string.Empty,
                                            ["VehicleType"] = reader["VehicleType"]?.ToString() ?? string.Empty,
                                            ["DriverName"] = reader["DriverName"]?.ToString() ?? string.Empty,
                                            ["PhoneNumber"] = reader["PhoneNumber"]?.ToString() ?? string.Empty,
                                            ["LastPosition"] = reader["LastPosition"]?.ToString() ?? string.Empty,
                                            ["VehicleMake"] = reader["VehicleMake"]?.ToString() ?? string.Empty,
                                            ["VehicleModel"] = reader["VehicleModel"]?.ToString() ?? string.Empty,
                                            ["LastGPSTime"] = reader["LastGPSTime"]?.ToString() ?? string.Empty,
                                            ["LastGPSSpeed"] = reader["LastGPSSpeed"]?.ToString() ?? string.Empty,
                                            ["LastAddress"] = reader["LastAddress"]?.ToString() ?? string.Empty
                                        };

                                        gvarData.DicOfDT["VehiclesInformations"] = new[] { resultDict };
                                        gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                        Console.WriteLine("Fetch Specific data from Vehicle Information table and other tables successfully.");
                                    }
                                    else
                                    {
                                        gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                    }
                                }

                            }
                            else
                            {
                                Console.WriteLine("Vehicle ID must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include VehicleID value.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when fetching specific data from Vehicle Information table and other tables: {ex.Message}");
            }

        }

        /// <summary>
        /// Retrieves the route history of a specific vehicle from the database within the specified time range.
        /// Updates the GVAR object with the status of the operation and the retrieved route history data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetVehicleRouteHistory(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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

                                        var resultDict = new ConcurrentDictionary<string, string>
                                        {
                                            ["VehicleNumber"] = reader["VehicleNumber"]?.ToString() ?? string.Empty,
                                            ["Address"] = reader["Address"]?.ToString() ?? string.Empty,
                                            ["Status"] = reader["Status"]?.ToString() ?? string.Empty,
                                            ["Latitude"] = reader["Latitude"]?.ToString() ?? string.Empty,
                                            ["Longitude"] = reader["Longitude"]?.ToString() ?? string.Empty,
                                            ["VehicleDirection"] = reader["VehicleDirection"]?.ToString() ?? string.Empty,
                                            ["GPSSpeed"] = reader["GPSSpeed"]?.ToString() ?? string.Empty,
                                            ["GPSTime"] = reader["GPSTime"]?.ToString() ?? string.Empty,
                                        };

                                        routeHistory.Add(resultDict);
                                    }
                                    if (routeHistory.Count > 0)
                                    {
                                        gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                        gvarData.DicOfDT["RouteHistory"] = routeHistory.ToArray();
                                        Console.WriteLine("Fetch data from Route History table tables successfully.");
                                    }
                                    else
                                    {
                                        gvarData.DicOfDic["Tags"].TryAdd("STS", "0");
                                    }
                                }

                            }
                            else
                            {
                                Console.WriteLine("Vehicle ID, Start and End Epoch must be a numeric string, not a string containing any characters.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("The request must include VehicleID, StartEpoch, and EndEpoch values.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The request must include a Tags field that holds the data.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred when fetching data from Route History table: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves geofence information from the "Geofences" table in the database.
        /// Updates the GVAR object with the status of the operation and the retrieved geofence data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetGeofenceInformation(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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
                                    ["GeofenceID"] = reader["GeofenceID"]?.ToString() ?? string.Empty,
                                    ["GeofenceType"] = reader["GeofenceType"]?.ToString() ?? string.Empty,
                                    ["AddedDate"] = reader["AddedDate"]?.ToString() ?? string.Empty,
                                    ["StrockColor"] = reader["StrockColor"]?.ToString() ?? string.Empty,
                                    ["StrockOpacity"] = reader["StrockOpacity"]?.ToString() ?? string.Empty,
                                    ["StrockWeight"] = reader["StrockWeight"]?.ToString() ?? string.Empty,
                                    ["FillColor"] = reader["FillColor"]?.ToString() ?? string.Empty,
                                    ["FillOpacity"] = reader["FillOpacity"]?.ToString() ?? string.Empty,
                                };

                                geofenceList.Add(resultDict);
                            }

                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();

                            if (geofenceList.Count > 0)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                gvarData.DicOfDT["Geofences"] = geofenceList.ToArray();
                                Console.WriteLine("Fetch data from Geofence Information table tables successfully.");
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
                Console.WriteLine($"An error occurred when fetching data from Geofence Information table: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves circle geofence information from the "CircleGeofence" table in the database.
        /// Updates the GVAR object with the status of the operation and the retrieved circle geofence data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetCircleGeofence(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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
                                    ["GeofenceID"] = reader["GeofenceID"]?.ToString() ?? string.Empty,
                                    ["Radius"] = reader["Radius"]?.ToString() ?? string.Empty,
                                    ["Latitude"] = reader["Latitude"]?.ToString() ?? string.Empty,
                                    ["Longitude"] = reader["Longitude"]?.ToString() ?? string.Empty,
                                };

                                geofenceList.Add(resultDict);
                            }

                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();

                            if (geofenceList.Count > 0)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                gvarData.DicOfDT["CircleGeofence"] = geofenceList.ToArray();
                                Console.WriteLine("Fetch data from Circle Geofence table tables successfully.");
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
                Console.WriteLine($"An error occurred when fetching data from Circle Geofence table: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves polygon geofence information from the "PolygonGeofence" table in the database.
        /// Updates the GVAR object with the status of the operation and the retrieved polygon geofence data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetPolygonGeofence(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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
                                    ["GeofenceID"] = reader["GeofenceID"]?.ToString() ?? string.Empty,
                                    ["Latitude"] = reader["Latitude"]?.ToString() ?? string.Empty,
                                    ["Longitude"] = reader["Longitude"]?.ToString() ?? string.Empty,
                                };

                                geofenceList.Add(resultDict);
                            }

                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();

                            if (geofenceList.Count > 0)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                gvarData.DicOfDT["PolygonGeofence"] = geofenceList.ToArray();
                                Console.WriteLine("Fetch data from Polygon Geofence table tables successfully.");
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
                Console.WriteLine($"An error occurred when fetching data from Polygon Geofence table: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves rectangle geofence information from the "RectangleGeofence" table in the database.
        /// Updates the GVAR object with the status of the operation and the retrieved rectangle geofence data.
        /// </summary>
        /// <param name="gvarData">Reference to the GVAR object where the retrieved data will be stored.</param>
        public void GetRectangleGeofence(ref GVAR gvarData)
        {
            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    connection.Open();

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
                                    ["GeofenceID"] = reader["GeofenceID"]?.ToString() ?? string.Empty,
                                    ["North"] = reader["North"]?.ToString() ?? string.Empty,
                                    ["East"] = reader["East"]?.ToString() ?? string.Empty,
                                    ["West"] = reader["West"]?.ToString() ?? string.Empty,
                                    ["South"] = reader["South"]?.ToString() ?? string.Empty,
                                };

                                geofenceList.Add(resultDict);
                            }

                            gvarData.DicOfDic["Tags"] = new ConcurrentDictionary<string, string>();

                            if (geofenceList.Count > 0)
                            {
                                gvarData.DicOfDic["Tags"].TryAdd("STS", "1");
                                gvarData.DicOfDT["RectangleGeofence"] = geofenceList.ToArray();
                                Console.WriteLine("Fetch data from Rectang Geofence table tables successfully.");
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
                Console.WriteLine($"An error occurred when fetching data from Rectang Geofence table: {ex.Message}");
            }
        }
    }
}