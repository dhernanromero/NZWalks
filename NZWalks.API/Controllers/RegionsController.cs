using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Text.Json;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        private readonly ILogger<RegionsController> logger;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger )
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
            this.logger = logger;
        }
        // GET ALL REGION
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            #region coment

            //var regions = new List<Region>
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Aukland Region",
            //        Code = "AKL",
            //        ReguionImageUrl =""
            //    },
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellington Region",
            //        Code = "WLG",
            //        ReguionImageUrl =""
            //    }
            //};
            #endregion

            try
            {
                //throw new Exception("this is a custom exception");

                //GEt data from Database - Domain models
                var regionsDomain = await regionRepository.GetAllAsync();

                //Map Domain models to DTOs
                //var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);
                //Return Dto
                logger.LogInformation($"Fionished GetAllRegions request with data: {JsonSerializer.Serialize(regionsDomain)}");

                return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }

          
        }

        //GET SINGLE REGION (Get region by ID)
        //GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute]Guid id) 
        {
            //var region = dbContext.Regions.Find(id);
            //Get region domain model from database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if(regionDomain == null)
            {
                return NotFound();
            }

            //Map/Convert Region domain model to Region DTO
            //
            
            //Return DTO back to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        //POST To create a new Region
        //POST: https://localhost:portnumber/api/regions
        [HttpPost]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto) 
        {
            //Map or Convert DTO to DOmain Model
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            //use DOmain model to create REgion
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //MAp Domain model bacj to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }


        //PUT To update a new Region
        //PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        //[Authorize(Roles = "Writer")]
        public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody]UpdateRegionRequestDto updateRegionRequestDto )
        {
       
            //Map DTO to DOmain
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //Revisar si existe la region
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            ////Map DTP tp domain model
            // Convert Domain model to dto
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
         
        }

        //DELETE REGION
        //DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if(regionDomainModel == null) 
            { 
                return NotFound();  
            }

            //delete
            //dbContext.Regions.Remove(regionDomainModel);
            //await dbContext.SaveChangesAsync();

            //retun deleted region back
            //Map Doman model to DTO
            
            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }


    }
}
