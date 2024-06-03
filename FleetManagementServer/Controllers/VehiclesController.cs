using FleetManagementLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FleetManagementServer.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly EndpointsRepository _endpointsRepository;
        public VehiclesController(EndpointsRepository endpointsRepository)
        {
            _endpointsRepository = endpointsRepository;
        }

        /// <summary>
        /// Retrieves vehicle data and returns it as a GVAR JSON response.
        /// </summary>
        /// <returns>An IActionResult containing the vehicle data in GVAR JSON format or an error message.</returns>
        [HttpGet("GetVehicles")]
        public IActionResult GetVehicles()
        {
            try
            {
                GVAR data = new GVAR();

                _endpointsRepository.GetVehicles(ref data);
                return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));

            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new vehicle using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the updated vehicle data in GVAR JSON format or an error message.</returns>
        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.AddVehicle(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        /// <summary>
        /// Updates an existing vehicle using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the updated vehicle data in GVAR JSON format or an error message.</returns>
        [HttpPut("UpdateVehicle")]
        public async Task<IActionResult> UpdateVehicle()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.UpdateVehicle(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        /// <summary>
        /// Deletes a vehicle using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the result of the delete operation in GVAR JSON format or an error message.</returns>
        [HttpDelete("DeleteVehicle")]
        public async Task<IActionResult> DeleteVehicle()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.DeleteVehicle(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }

            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves driver data and returns it as a GVAR JSON response.
        /// </summary>
        /// <returns>An IActionResult containing the driver data in GVAR JSON format or an error message.</returns>
        [HttpGet("GetDrivers")]
        public IActionResult GetDrivers()
        {
            try
            {
                GVAR data = new GVAR();
                _endpointsRepository.GetDrivers(ref data);
                return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));

            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new driver using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the updated driver data in GVAR JSON format or an error message.</returns>
        [HttpPost("AddDriver")]
        public async Task<IActionResult> AddDriver()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.AddDriver(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        /// <summary>
        /// Updates an existing driver using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the updated driver data in GVAR JSON format or an error message.</returns>
        [HttpPut("UpdateDriver")]
        public async Task<IActionResult> UpdateDriver()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.UpdateDriver(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a driver using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the result of the delete operation in GVAR JSON format or an error message.</returns>
        [HttpDelete("DeleteDriver")]
        public async Task<IActionResult> DeleteDriver()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.DeleteDriver(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds new vehicle information using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the updated vehicle information data in GVAR JSON format or an error message.</returns>
        [HttpPost("AddVehiclesInformation")]
        public async Task<IActionResult> AddVehiclesInformation()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.AddVehiclesInformations(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates existing vehicle information using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the updated vehicle information data in GVAR JSON format or an error message.</returns>
        [HttpPut("UpdateVehiclesInformation")]
        public async Task<IActionResult> UpdateVehiclesInformation()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.UpdateVehiclesInformation(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes vehicle information using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the result of the delete operation in GVAR JSON format or an error message.</returns>
        [HttpDelete("DeleteVehiclesInformation")]
        public async Task<IActionResult> DeleteVehiclesInformation()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.DeleteVehiclesInformation(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new route history entry using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the updated route history data in GVAR JSON format or an error message.</returns>
        [HttpPost("AddRouteHistory")]
        public async Task<IActionResult> AddRouteHistory()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    var result = await _endpointsRepository.AddRouteHistoryAsync(data);
                    return Ok(JsonConvert.SerializeObject(result, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all vehicle information and returns it as a JSON response.
        /// </summary>
        /// <returns>An IActionResult containing the vehicle information data in GVAR JSON format or an error message.</returns>
        [HttpGet("AllVehicleInformations")]
        public IActionResult GetVehicleInfo()
        {
            try
            {
                GVAR data = new GVAR();

                _endpointsRepository.GetVehicleInfo(ref data);
                return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));

            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }

        /// <summary>
        /// Retrieves specific driver information and returns it as a GVAR JSON response.
        /// </summary>
        /// <returns>An IActionResult containing the driver information data in GVAR JSON format or an error message.</returns>
        [HttpGet("GetDriverInformation")]
        public IActionResult GetDriverInformation()
        {
            try
            {
                GVAR data = new GVAR();

                _endpointsRepository.GetDriver(ref data);
                return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves comprehensive vehicle information and returns it as a GVAR JSON response.
        /// </summary>
        /// <returns>An IActionResult containing the vehicle information data in GVAR JSON format or an error message.</returns>
        [HttpGet("GetVehicleInformations")]
        public IActionResult GetVehicleInformations()
        {
            try
            {
                GVAR data = new GVAR();

                _endpointsRepository.GetVehicleInformations(ref data);
                return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves specific vehicle information using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the vehicle information data in GVAR JSON format or an error message.</returns>
        [HttpPost("GetVehicleInformation")]
        public async Task<IActionResult> GetVehicleInformation()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.GetVehicleInformation(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the route history of a specific vehicle using the data provided in the request body.
        /// </summary>
        /// <returns>An IActionResult containing the route history data in GVAR JSON format or an error message.</returns>
        [HttpPost("GetVehicleRouteHistory")]
        public async Task<IActionResult> GetVehicleRouteHistory()
        {
            try
            {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                else
                {
                    _endpointsRepository.GetVehicleRouteHistory(ref data);
                    return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
                }
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves geofence information and returns it as a GVAR JSON response.
        /// </summary>
        /// <returns>An IActionResult containing the geofence information data in GVAR JSON format or an error message.</returns>
        [HttpGet("GetGeofenceInformation")]
        public IActionResult GetGeofenceInformation()
        {
            try
            {
                GVAR data = new GVAR();

                _endpointsRepository.GetGeofenceInformation(ref data);
                return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves circle geofence information and returns it as a GVAR JSON response.
        /// </summary>
        /// <returns>An IActionResult containing the circle geofence information data in GVAR JSON format or an error message.</returns>
        [HttpGet("GetCircleGeofence")]
        public IActionResult GetCircleGeofence()
        {
            try
            {
                GVAR data = new GVAR();

                _endpointsRepository.GetCircleGeofence(ref data);
                return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves polygon geofence information and returns it as a GVAR JSON response.
        /// </summary>
        /// <returns>An IActionResult containing the polygon geofence information data in GVAR JSON format or an error message.</returns>
        [HttpGet("GetPolygonGeofence")]
        public IActionResult GetPolygonGeofence()
        {
            try
            {
                GVAR data = new GVAR();

                _endpointsRepository.GetPolygonGeofence(ref data);
                return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves rectangle geofence information and returns it as a GVAR JSON response.
        /// </summary>
        /// <returns>An IActionResult containing the rectangle geofence information data in GVAR JSON format or an error message.</returns>
        [HttpGet("GetRectangleGeofence")]
        public IActionResult GetRectangleGeofence()
        {
            try
            {
                GVAR data = new GVAR();

                _endpointsRepository.GetRectangleGeofence(ref data);
                return Ok(JsonConvert.SerializeObject(data, Formatting.Indented));
            }
            catch (JsonSerializationException ex)
            {
                return BadRequest($"JSON deserialization error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}