using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using VoorbeeldToetsApiOntwikkeling.Models;
using VoorbeeldToetsApiOntwikkeling.Services;

namespace VoorbeeldToetsApiOntwikkeling.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RealEstateController : ControllerBase
    {
        private readonly IRealEstateData _realEstateData;
        private readonly ILogger<RealEstateController> _logger;

        public RealEstateController(IRealEstateData realEstateData, ILogger<RealEstateController> logger)
        {
            _realEstateData = realEstateData;
            _logger = logger;
        }

        [HttpGet("ListForSale")]
        [EnableRateLimiting(policyName: "fixed")]
        public ActionResult<IEnumerable<Property>> GetForSale()
        {
            _logger.LogInformation("Fetching properties listed for sale.");
            var properties = _realEstateData.GetForSale();
            _logger.LogInformation("Found {Count} properties for sale.", properties.Count());
            return Ok(properties);
        }

        [HttpGet("ListSold")]
        [EnableRateLimiting(policyName: "fixed")]
        public ActionResult<IEnumerable<Property>> GetSold()
        {
            var properties = _realEstateData.GetSold();
            _logger.LogInformation("Fetching sold properties. Found {Count} sold properties.", properties.Count());
            return Ok(properties);
        }

        [HttpGet("Details/{id:int}")]
        [EnableRateLimiting(policyName: "fixed")]
        public ActionResult<Property> Get(int id)
        {
            var property = _realEstateData.Get(id);
            if (property is null)
            {
                _logger.LogWarning("Property with ID {PropertyId} not found.", id);
                return NotFound();
            }
            _logger.LogInformation("Fetched details for property with ID {PropertyId}.", id);
            return Ok(property);
        }

        [HttpPost("Create")]
        [EnableRateLimiting(policyName: "fixed")]
        public ActionResult Add(Property property)
        {
            _logger.LogInformation("Creating a new property with ID {PropertyId}.", property.Id);
            _realEstateData.Add(property);
            _logger.LogInformation("Property with ID {PropertyId} created successfully.", property.Id);
            return CreatedAtAction(nameof(Get), new { id = property.Id }, property);
        }

        [HttpPut("Update")]
        [EnableRateLimiting(policyName: "fixed")]
        public ActionResult Update(Property property)
        {
            var existingProperty = _realEstateData.Get(property.Id);
            if (existingProperty is null)
            {
                _logger.LogWarning("Attempted to update a non-existent property with ID {PropertyId}.", property.Id);
                return NotFound();
            }

            _logger.LogInformation("Updating property with ID {PropertyId}.", property.Id);
            _realEstateData.Update(property);
            _logger.LogInformation("Property with ID {PropertyId} updated successfully.", property.Id);
            return Ok(property);
        }
    }
}
