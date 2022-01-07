using Meetups.API.Domain.DTO;
using Meetups.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Meetups.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipacaoController : ControllerBase
    {
        private readonly ParticipacaoService participacaoService;

        public ParticipacaoController(ParticipacaoService participacaoService)
        {
            this.participacaoService = participacaoService;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var retorno = participacaoService.PesquisarPorIdEvento(id);
            if(retorno == null)
                return NotFound();
            return Ok(retorno);
        }

        [HttpPost("Participacao")]
        public IActionResult PostParticipacao([FromBody] ParticipacaoCreateRequest postModel)
        {
            if(ModelState.IsValid)
            {
                var retorno = participacaoService.CadastrarNovo(postModel);

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

        public IActionResult PutParticipante(int id, [FromBody] ParticipacaoUpdateRequest putModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = participacaoService.EditarPresenca(id, putModel);
                if (!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut ("Avaliar/{id}")]

        public IActionResult PutParticipante(int id, [FromBody] AvaliacaoUpdateRequest putModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = participacaoService.Avaliacao(id, putModel);
                if (!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("Participantes/{nome}")]
        public IActionResult GetByLogin(string nome)
        {
            var retorno = participacaoService.PesquisarPorLogin(nome);

            if (retorno == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(retorno);
            }
        }
    }

    




}
