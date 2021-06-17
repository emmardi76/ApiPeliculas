using ApiPeliculas.Models;
using ApiPeliculas.Models.Dtos;
using ApiPeliculas.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
//using AutoMapper.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace ApiPeliculas.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "ApiPeliculasUsuarios")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public UsersController(IUserRepository userRepository, IMapper mapper, IConfiguration config)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _config = config;
        }
        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<UserDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsers()
        {
            var ListUsers = _userRepository.GetUser();

            var ListtUsersDto = new List<UserDto>();

            foreach(var List in ListUsers)
            {
                ListtUsersDto.Add(_mapper.Map<UserDto>(List));
            }
            return Ok(ListtUsersDto);
        }
        /// <summary>
        /// Get an user
        /// </summary>
        /// <param name="userId">this is the userId</param>
        /// <returns></returns>
        [HttpGet("{userId:int}", Name = "GetUser")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetUser(int userId)
        {
            var itemUser = _userRepository.GetUser(userId);

            if (itemUser == null)
            {
                return NotFound();
            }

            var itemUserDto = _mapper.Map<UserDto>(itemUser);
            return Ok(itemUserDto);
        }
        /// <summary>
        /// Register an user
        /// </summary>
        /// <param name="userAuthDto">This is userAuthDto</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("RegisterUser")]
        [ProducesResponseType(201)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult RegisterUser( UserAuthDto userAuthDto)
        {
            userAuthDto.User = userAuthDto.User.ToLower();

            if (_userRepository.ExistUser(userAuthDto.User))
            {
                return BadRequest("The user exist yet");
            }

            var userACreate = new User
            {
                UserA = userAuthDto.User
            };

            var userCreate = _userRepository.RegisterUser(userACreate, userAuthDto.Password);
            return Ok(userCreate);
        }
        /// <summary>
        /// Login an user
        /// </summary>
        /// <param name="userAuthLoginDto">This is userAuthLoginDto</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("LoginUser")]
        [ProducesResponseType(200, Type = typeof(UserAuthLoginDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult LoginUser(UserAuthLoginDto userAuthLoginDto)
        {
           
                var userfromRepo = _userRepository.LoginUser(userAuthLoginDto.User, userAuthLoginDto.Password);

                if (userfromRepo == null)
                {
                    return Unauthorized();
                }

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier, userfromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userfromRepo.UserA.ToString())

                };

                //Generation token
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = credentials
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
                    
        }
    }
}
