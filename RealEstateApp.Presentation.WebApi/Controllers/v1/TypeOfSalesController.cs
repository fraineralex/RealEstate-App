using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.DeleteTypeOfPropertiesById;
using RealEstateApp.Core.Application.Features.TypeOfSales.Commands.CreateTypeOfSales;
using RealEstateApp.Core.Application.Features.TypeOfSales.Commands.DeleteTypeOfSales;
using RealEstateApp.Core.Application.Features.TypeOfSales.Commands.UpdateTypeOfSales;
using RealEstateApp.Core.Application.Features.TypeOfSales.Queries.GetAllTypeOfSales;
using RealEstateApp.Core.Application.Features.TypeOfSales.Queries.GetTypeOfSalesById;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.TypeOfSales;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.Presentation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Mantenimiento de tipo de ventas")]
    public class TypeOfSalesController : BaseApiController
    {
        [Authorize(Policy = "RequireOnlyAdminAndDeveloper")]
        [HttpGet]
        [SwaggerOperation(
           Summary = "Listado de tipo de ventas",
           Description = "Obtiene todos los tipos de ventas registrados."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeOfSalesViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> List()
        {
            try
            {
                var typeOfProperties = await Mediator.Send(new GetAllTypeOfSalesQuery());

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
           Summary = "Tipo de venta por Id",
           Description = "Obtiene un tipo de venta utilizando el id como filtro."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveTypeOfSalesViewModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var typeOfSale = await Mediator.Send(new GetTypeOfSalesByIdQuery { Id = id });
                return Ok(typeOfSale);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(
          Summary = "Creacion de tipo de venta",
          Description = "Recibe los parametros necesarios para un nuevo tipo de venta."
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(CreateTypeOfSalesCommand command)
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
               Summary = "Actualizacion de tipo de venta",
               Description = "Recibe los parametros necesarios para modificar un tipo de venta existente."
        )]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaveTypeOfSalesViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, TypeOfSalesUpdateCommand command)
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
            Summary = "Eliminar un tipo de venta",
            Description = "Recibe los parametros necesarios para eliminar una tipo de venta existente."
        )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteTypeOfSalesByIdCommand { Id = id });
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}

