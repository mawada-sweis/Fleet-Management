using FleetManagementLibrary;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [HttpPost("AddVehicle")]
        public async Task<IActionResult> AddVehicle()
        {
            try {
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body);
                var json = await reader.ReadToEndAsync();

                Request.Body.Seek(0, SeekOrigin.Begin);

                var data = JsonConvert.DeserializeObject<GVAR>(json);

                if (data == null)
                {
                    return BadRequest("Invalid data");
                }
                _endpointsRepository.AddVehicle(ref data);
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
                _endpointsRepository.UpdateVehicle(ref data);
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
                _endpointsRepository.DeleteVehicle(ref data);
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
                _endpointsRepository.AddDriver(ref data);
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
                _endpointsRepository.UpdateDriver(ref data);
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
                _endpointsRepository.DeleteDriver(ref data);
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
                _endpointsRepository.AddVehiclesInformations(ref data);
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
                _endpointsRepository.UpdateVehiclesInformation(ref data);
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
                _endpointsRepository.DeleteVehiclesInformation(ref data);
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
                var result = await _endpointsRepository.AddRouteHistoryAsync(data);
                return Ok(JsonConvert.SerializeObject(result, Formatting.Indented));

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
                _endpointsRepository.GetVehicleInformation(ref data);
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
                _endpointsRepository.GetVehicleRouteHistory(ref data);
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