using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Features.Improvements.Commands.DeleteImprovementsById;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.CreateTypeOfProperties;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.DeleteTypeOfPropertiesById;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.UpdateTypeOfProperties;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetAllTypeOfProperties;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetTypeOfPropertiesById;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperties;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Mantenimiento de tipo de propiedades")]
    public class TypeOfPropertiesController : BaseApiController
    {
        [Authorize(Policy = "RequireOnlyAdminAndDeveloper")]
        [HttpGet]
        [SwaggerOperation(
           Summary = "Listado de tipo de propiedades",
           Description = "Obtiene todos los tipos de ventas registrados."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeOfPropertiesViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List()
        {
            try
            {
                var typeOfProperties = await Mediator.Send(new GetAllTypeOfPropertiesQuery());
                return Ok(typeOfProperties);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Policy = "RequireOnlyAdminAndDeveloper")]
        [HttpGet("{id}")]
        [SwaggerOperation(
           Summary = "Tipo de propiedad por Id",
           Description = "Obtiene un tipo de propiedad utilizando el id como filtro."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveTypeOfPropertiesViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var typeOfProperty = await Mediator.Send(new GetTypeOfPropertiesByIdQuery { Id = id});
                return Ok(typeOfProperty);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(
          Summary = "Creacion de tipo de propiedad",
          Description = "Recibe los parametros necesarios para un nuevo tipo de propiedad."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateTypeOfPropertiesCommand command)
        {
            try
            {
                await Mediator.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(
               Summary = "Actualizacion de tipo de propiedad",
               Description = "Recibe los parametros necesarios para modificar un tipo de propiedad existente."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveTypeOfPropertiesViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, UpdateTypeOfPropertiesCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (id != command.Id)
                {
                    return BadRequest();
                }
                return Ok(await Mediator.Send(command));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar un tipo de propiedad",
            Description = "Recibe los parametros necesarios para eliminar una tipo de propiedad existente."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteTypeOfPropertiesByIdCommand { Id = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

