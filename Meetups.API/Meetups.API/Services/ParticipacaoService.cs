using Meetups.API.DAL;
using Meetups.API.Domain.DTO;
using Meetups.API.Domain.Entity;
using Meetups.API.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Meetups.API.Services
{
    public class ParticipacaoService
    {
        private readonly MeetupsContext _context;

        public ParticipacaoService (MeetupsContext context)
        {
            _context = context;
        }

        public ParticipacaoResponse MapeiaParticipacaoResponse(Participacao participacao)
        {
            var retorno = new ParticipacaoResponse()
            {
                IdParticipacao = participacao.IdParticipacao,
                IdEvento = participacao.IdEvento,
                LoginParticipante = participacao.LoginParticipante,
                FlagPresente = participacao.FlagPresente,
                Nota = participacao.Nota,
                Comentario = participacao.Comentario,
            };

            return retorno;

        }

        public List<ParticipacaoResponse> PesquisarPorIdEvento (int id)
        {
            var lista = _context.Participacoes.Where(x => x.IdEvento == id).ToList();
            
            var retorno = new List<ParticipacaoResponse>();
            foreach (var participacao in lista)
            {
                retorno.Add(MapeiaParticipacaoResponse(participacao));
            }

            return retorno;
        }

        public ServiceResponse<ParticipacaoResponse> CadastrarNovo(ParticipacaoCreateRequest model)
        {
            var evento = _context.Eventos.Find(model.IdEvento);
            int quantidade = PesquisarPorIdEvento(model.IdEvento).Count;
            if (quantidade >= evento.LimiteVagas)
                return new ServiceResponse<ParticipacaoResponse>("O evento não possui vagas disponíveis");
            if (evento.IdEventoStatus != 1)
                return new ServiceResponse<ParticipacaoResponse>("Ó evento não está aberto para inscrições");

            var participacao = new Participacao()
            {
                IdEvento = model.IdEvento,
                LoginParticipante = model.LoginParticipante,
                FlagPresente = false,
                Nota = null,
                Comentario = "",
            };

            _context.Add(participacao);
            _context.SaveChanges();

            return new ServiceResponse<ParticipacaoResponse>(MapeiaParticipacaoResponse(participacao));
        }

        public ServiceResponse<ParticipacaoResponse> EditarPresenca(int id, ParticipacaoUpdateRequest model)
        {
            var resultado = _context.Participacoes.Find(id);
            var idEvento = resultado.IdEvento;
            var evento = _context.Eventos.Find(idEvento);

            if (evento.IdEventoStatus == 1 || evento.IdEventoStatus == 4)
                return new ServiceResponse<ParticipacaoResponse>("Somente é possível indicar presença após o evento ter iniciado");
            if (model.FlagPresente != null)
                resultado.FlagPresente = model.FlagPresente;

            _context.Entry(resultado).State = EntityState.Modified;
            _context.SaveChanges();

            return new ServiceResponse<ParticipacaoResponse>(MapeiaParticipacaoResponse(resultado));
        }

        public ServiceResponse<ParticipacaoResponse> Avaliacao(int id, AvaliacaoUpdateRequest model)
        {
            var resultado = _context.Participacoes.Find(id);
            if (resultado == null)
                return new ServiceResponse<ParticipacaoResponse>("Participação inexixtente");

            if (resultado.FlagPresente == false)
                return new ServiceResponse<ParticipacaoResponse>("Somente é permitido avaliar após participar do evento");
            
            if (model.Nota != null)
                resultado.Nota = model.Nota;
            if (model.Comentario != null)
                resultado.Comentario = model.Comentario;

            _context.Entry(resultado).State = EntityState.Modified;
            _context.SaveChanges();

            return new ServiceResponse<ParticipacaoResponse>(MapeiaParticipacaoResponse(resultado));

        }

        public List<ParticipanteUnicoResponse> PesquisarPorLogin(string nome)
        {

            var lista = _context.Participacoes.Where(x => x.LoginParticipante == nome).ToList();
            var retorno = new List<ParticipanteUnicoResponse>();

            foreach (var item in lista)
            {
                retorno.Add(MapeiaParticipanteResponse(item));
            }

            return retorno;


        }
        private ParticipanteUnicoResponse MapeiaParticipanteResponse(Participacao participacao)
        {

            Evento evento = _context.Eventos.Find(participacao.IdEvento);


            var retorno = new ParticipanteUnicoResponse()
            {
                IdParticipacao = participacao.IdParticipacao,
                IdEvento = participacao.IdEvento,
                LoginParticipante = participacao.LoginParticipante,
                FlagPresente = participacao.FlagPresente,
                Nota = participacao.Nota,
                Comentario = participacao.Comentario,

            };

            retorno.Eventos = new EventoResponse2()
            {
                IdEvento = evento.IdEvento,
                IdEventoStatus = evento.IdEventoStatus,
                IdCategoriaEvento = evento.IdCategoriaEvento,
                Nome = evento.Nome,
                DataHoraInicio = evento.DataHoraInicio,
                DataHoraFim = evento.DataHoraFim,
                Local = evento.Local,
                Descricao = evento.Descricao,
                LimiteVagas = evento.LimiteVagas,
            };

            StatusEvento statusEvento = _context.StatusEventos.Find(evento.IdEventoStatus);

            CategoriaEvento categoriaEvento = _context.CategoriaEventos.Find(evento.IdCategoriaEvento);

            retorno.Eventos.StatusEvento = new StatusEventoResponse()
            {
                IdEventoStatus = statusEvento.IdEventoStatus,
                NomeStatus = statusEvento.NomeStatus
            };


            retorno.Eventos.CategoriaEvento = new CategoriaEventoResponse()
            {
                IdCategoriaEvento = categoriaEvento.IdCategoriaEvento,
                NomeCategoria = categoriaEvento.NomeCategoria
            };


            return retorno;
        }
    }
}
