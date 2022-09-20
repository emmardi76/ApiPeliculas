using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiPeliculas.Controllers
{
    [Authorize]
    [Route("api/Categories")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiPeliculasCategorias")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _ctRepository;
        private readonly IMapper _mapper;        

        public CategoriesController(ICategoryRepository ctRepository, IMapper mapper)
        {
            _ctRepository = ctRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategories()
        {
            var ListCategories = await _ctRepository.GetCategoryAsync();

            var ListCategoriesDto = new List<CategoryDto>();

            foreach(var List in ListCategories)
            {
                ListCategoriesDto.Add(_mapper.Map<CategoryDto>(List));
            }
            return Ok(ListCategoriesDto);
        }
        /// <summary>
        /// Get an individual category
        /// </summary>
        /// <param name="categoryId">This is the Id of category</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{categoryId:int}", Name = "GetCategory")]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategory(int categoryId)
        {
            var itemCategory = _ctRepository.GetCategory(categoryId);

            if (itemCategory == null)
            {
                return NotFound();
            }

            var itemCategoryDto = _mapper.Map<CategoryDto>(itemCategory);
            return Ok(itemCategoryDto);
        }
        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_ctRepository.ExistCategory(categoryDto.Name))
            {
                ModelState.AddModelError("", "The category exist yet");
                return StatusCode(404,ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_ctRepository.CreateCategory(category))
            {
                ModelState.AddModelError("", $"Anything is brong saving the record{category.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategory", new { categoryId = category.Id}, category);
        }
        /// <summary>
        /// Update an existing category 
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="categoryDto"></param>
        /// <returns></returns>
        [HttpPatch("{categoryId:int}", Name = "UpdateCategory")]
        [ProducesResponseType(204)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(int categoryId, [FromBody]CategoryDto categoryDto)
        {
            if (categoryDto == null || categoryId != categoryDto.Id)
            {
                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(categoryDto);

            if (!_ctRepository.UpdateCategory(category))
            {
                ModelState.AddModelError("", $"Anything is brong updating the record{category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        /// <summary>
        /// Delete an existing category
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpDelete("{categoryId:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_ctRepository.ExistCategory(categoryId))
            {
                return NotFound();
            }

            var category = _ctRepository.GetCategory(categoryId);

            if (!_ctRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("", $"Anything is brong deleting the record{category.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
