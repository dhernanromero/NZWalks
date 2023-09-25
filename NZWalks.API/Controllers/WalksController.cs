using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // CREATE walk
        //POST: /api/wals
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
      
            // Map dto to Domain Model
            var walkDomainModel = mapper.Map<Walk>(addWalkRequestDto);
            await walkRepository.CreateAsybc(walkDomainModel);

            // Map Domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }

        // GET walks
        // GET: /api/walks?filterOnName&filterQuery=Track&sortyByName&isAscending=true
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {
            var walsDomainModel = await walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending ?? true,pageNumber,pageSize);
            //Create an exception
            throw new Exception("this is a new exception");

            // Map Domain Model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walsDomainModel));
        }

        // GET Walk by Id
        // GET; /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomainModel = await walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }

            //Map domain to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));

        }

        //Update walks
        //PUT: /api/walks/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateWalkRequestDto updateWalkRequestDto )
        {

            // Map dto to Domain model
            var walkDomainModel = mapper.Map<Walk>(updateWalkRequestDto);
            walkDomainModel = await walkRepository.UpdateAsync(id, walkDomainModel);

            if (walkDomainModel == null)
            {
                return NotFound();
            }

            // map domain model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomainModel));
           
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
            if(deletedWalkDomainModel == null)
            {
                return NotFound();
            }

            // Map Domain model to dto
            return Ok(mapper.Map<WalkDto>(deletedWalkDomainModel));
        }
    }
}
