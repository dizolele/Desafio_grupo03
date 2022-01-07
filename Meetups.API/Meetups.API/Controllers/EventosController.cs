using Meetups.API.Domain.DTO;
using Meetups.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meetups.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly EventoService eventosService;

        public EventosController(EventoService eventosService)
        {
            this.eventosService = eventosService;
        }

        [HttpGet]
        public IEnumerable<EventoResponse> Get()
        {
            return eventosService.ListarEventos();
        }

        [HttpGet("nome/{idCategoria}")]
        public IActionResult GetById(int idCategoria)
        {
            var retorno = eventosService.PesquisarPorIdCategoria(idCategoria);

            if(retorno.Sucesso)
            {
                return Ok(retorno);
            } else
            {
                return NotFound(retorno.Mensagem);
            }

        }

        [HttpGet("date/{date}")]
        public IActionResult GetByDate(DateTime date)
        {
            var retorno = eventosService.BuscarPorData(date);

            if (retorno.Sucesso)
            {
                return Ok(retorno.ObjetoRetorno);
            } else
            {
                return NotFound(retorno.Mensagem);
            }
        }

        [HttpPost]
        public IActionResult Post ([FromBody] EventoCreateRequest postModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = eventosService.CadastrarNovo(postModel);
                if (!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
                else
                    return Ok(retorno);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put (int id, [FromBody] StatusEventoUpdateRequest putModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = eventosService.EditarStatus(id, putModel);

                if(!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
                return Ok(retorno.ObjetoRetorno);
            } else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var retorno = eventosService.Deletar(id);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok();
        }

    }
}
