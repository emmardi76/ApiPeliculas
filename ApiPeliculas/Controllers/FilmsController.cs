using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiPeliculas.Controllers
{
    [Authorize]
    [Route("api/Films")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiPeliculas")]
    public class FilmsController : Controller
    {
        private readonly IFilmRepository _fRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public FilmsController(IFilmRepository fRepository, IMapper mapper, IWebHostEnvironment hostingEnvironment)
        {
            _fRepository = fRepository;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// Get all Films
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<FilmDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetFilms()
        {
            var ListFilms = _fRepository.GetFilm();

            var ListFilmsDto = new List<FilmDto>();

            foreach(var List in ListFilms)
            {
                ListFilmsDto.Add(_mapper.Map<FilmDto>(List));
            }
            return Ok(ListFilmsDto);
        }
        /// <summary>
        /// Get an individual film
        /// </summary>
        /// <param name="FilmId">This is the Id of film</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{FilmId:int}", Name = "GetFilm")]
        [ProducesResponseType(200, Type = typeof(FilmDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetFilm(int FilmId)
        {
            var itemFilm = _fRepository.GetFilm(FilmId);

            if (itemFilm == null)
            {
                return NotFound();
            }

            var itemFilmDto = _mapper.Map<FilmDto>(itemFilm);
            return Ok(itemFilmDto);
        }
        /// <summary>
        /// Get film in category
        /// </summary>
        /// <param name="categoryId">This is the categoryId</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("GetFilmInCategory/{categoryId:int}")]
        [ProducesResponseType(200, Type = typeof(FilmDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetFilmInCategory(int categoryId)
        {
            var FilmList = _fRepository.GetFilmInCategory(categoryId);

            if(FilmList == null)
            {
                return NotFound();
            }

            var itemFilm = new List<FilmDto>();

            foreach(var item in FilmList)
            {
                itemFilm.Add(_mapper.Map<FilmDto>(item));
            }

            return Ok(itemFilm);
        }
        /// <summary>
        /// Search a film
        /// </summary>
        /// <param name="Name">This is the Name of the film</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("SearchFilm")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult SearchFilm(string Name)
        {
            try
            {
                var result = _fRepository.SearchFilm(Name);
                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving application data");
            }
        }
        /// <summary>
        /// Create new film
        /// </summary>
        /// <param name="FilmDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(FilmDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateFilm([FromForm] FilmCreateDto FilmDto)
        {
            if (FilmDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_fRepository.ExistFilm(FilmDto.Name))
            {
                ModelState.AddModelError("", "The film exist yet");
                return StatusCode(404,ModelState);
            }

            /* Push files */
            var file = FilmDto.Photo;
            string mainRoute = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            if (file.Length > 0)
            {
                //New image
                var PhotoName = Guid.NewGuid().ToString();
                var pushes = Path.Combine(mainRoute, @"photos");
                var extension = Path.GetExtension(files[0].FileName);

                using (var fileStreams = new FileStream(Path.Combine(pushes, PhotoName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStreams);
                }
                FilmDto.RouteImaage = @"\photos\" + PhotoName + extension;

            }
            /*****************************************************/

            var film = _mapper.Map<Film>(FilmDto);

            if (!_fRepository.CreateFilm(film))
            {
                ModelState.AddModelError("", $"Anything is brong saving the record{film.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetFilm", new { FilmId = film.Id}, film);
        }
        /// <summary>
        /// Update an existing film 
        /// </summary>
        /// <param name="FilmId"></param>
        /// <param name="filmDto"></param>
        /// <returns></returns>
        [HttpPatch("{FilmId:int}", Name = "UpdateFilm")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateFilm(int FilmId, [FromBody]FilmDto filmDto)
        {
            if (filmDto == null || FilmId != filmDto.Id)
            {
                return BadRequest(ModelState);
            }

            var film = _mapper.Map<Film>(filmDto);

            if (!_fRepository.UpdateFilm(film))
            {
                ModelState.AddModelError("", $"Anything is brong updating the record{film.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        /// <summary>
        /// Delete an existing film
        /// </summary>
        /// <param name="FilmId"></param>
        /// <returns></returns>
        [HttpDelete("{FilmId:int}", Name = "DeleteFilm")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteFilm(int FilmId)
        {
            if (!_fRepository.ExistFilm(FilmId))
            {
                return NotFound();
            }

            var film = _fRepository.GetFilm(FilmId);

            if (!_fRepository.DeleteFilm(film))
            {
                ModelState.AddModelError("", $"Anything is brong deleting the record{film.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
