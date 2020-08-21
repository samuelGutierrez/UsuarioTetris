using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UserTetris.Api.Responses;
using UserTetris.Core.DTOs;
using UserTetris.Core.Interfaces;
using UsuarioTetris.Core.Entities;

namespace UsuarioTetris.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userServices;
        private readonly IMapper _mapper;
        public UserController(IMapper mapper, IUserService userServices)
        {
            _userServices = userServices;
            _mapper = mapper;
        }

        /// <summary>
        /// Traer todos los usuarios.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetUsers))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse<IEnumerable<UserDto>>))]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userServices.GetUsers();
            var usersDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            var response = new ApiResponse<IEnumerable<UserDto>>(usersDtos);
            return Ok(response);
        }

        /// <summary>
        /// Traer un usuario por su número de identificación
        /// </summary>
        /// <param name="identification">Número de identificación del usuario</param>
        /// <returns></returns>
        [HttpGet("{identification}",Name = nameof(GetUser))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse<UserDto>))]
        public async Task<IActionResult> GetUser(int identification)
        {
            var user = await _userServices.GetUser(identification);
            var userDto = _mapper.Map<UserDto>(user);
            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        /// <summary>
        /// Crear un nuevo usuario
        /// </summary>
        /// <param name="userDto">Información a llenar para poder crear un nuevo usuario</param>
        /// <returns></returns>
        [HttpPost(Name = nameof(Post))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<UserDto>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse<UserDto>))]
        public async Task<IActionResult> Post(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            await _userServices.PostUser(user);

            userDto = _mapper.Map<UserDto>(user);
            var response = new ApiResponse<UserDto>(userDto);
            return Ok(response);
        }

        /// <summary>
        /// Actualizar la información de un usuario ya existente
        /// </summary>
        /// <param name="identification">Número de identificación del usuario</param>
        /// <param name="userDto">Información del usuario</param>
        /// <returns></returns>
        [HttpPut(Name = nameof(Put))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> Put(int identification, UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.NumIdentification = identification;

            var result = await _userServices.UpdateUser(user);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }

        /// <summary>
        /// Eliminar un usuario
        /// </summary>
        /// <param name="identification">Número de identificación del usuario</param>
        /// <returns></returns>
        [HttpDelete("{identification}", Name = nameof(Delete))]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.NotFound, Type = typeof(ApiResponse<bool>))]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> Delete(int identification)
        {
            var result = await _userServices.DeleteUser(identification);
            var response = new ApiResponse<bool>(result);
            return Ok(response);
        }
    }
}