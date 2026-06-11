using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;
using AutoMapper;
namespace NZWalks.API.Controllers
{
    //GET : https://localhost:7191/api/regions
    [Route("api/regions")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        // GET ALL REGIONS 
        // GET: https://localhost:7191/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get Data from Database - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync();
            //Converting Domain Models to DTOs
            var regionsDTO = mapper.Map<List<RegionDTO>> (regionsDomain);

            return Ok(regionsDTO);
        }

        //GET REGION BY ID
        //GET: https://localhost:7191/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {

            //var region= dbContext.Regions.Find(id);
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound(); // This path returns a value
            }
            //Map/Convert Region Domain Model to Region DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomain);
            return Ok(regionDTO);
        }

        //Post to create a new region
        //POST: https://localhost:7191/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //CONVERT DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);

            //USE Domain Model to create a new region in the database
            regionDomainModel= await regionRepository.CreateAsync(regionDomainModel);

            //Map domain model back to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new {id=regionDTO.Id}, regionDTO);
        }
        //Update Region
        //PUT: https://localhost:7191/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {
            //Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDTO);

            //Check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if(regionDomainModel == null)
            {
                return NotFound();
            }



            //Mapping Domain Model back to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);
            return Ok(regionDTO);

        }

        //Delete Region
        //DELETE: https://localhost:7191/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if (regionDomainModel == null)
            {
                return NotFound(); 
            }

            //map domain model to DTO
            var regionDTO = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDTO);
        }
    }
}
