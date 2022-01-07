using Meetups.API.DAL;
using Meetups.API.Domain.DTO;
using Meetups.API.Domain.Entity;
using Meetups.API.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Meetups.API.Services
{
    public class EventoService
    {
        private readonly MeetupsContext _context;

        public EventoService(MeetupsContext context)
        {
            _context = context;
        }

        public List<EventoResponse> ListarEventos()
        {
            var lista = _context.Eventos.Include(x => x.IdEventoStatusNavigation).
                Include(x => x.IdCategoriaEventoNavigation)
                .Include(x => x.Participacoes).ToList();

            var retorno = new List<EventoResponse>();

            foreach (var item in lista)
            {
                if ((item.DataHoraInicio.Date < DateTime.Today) && (item.IdEventoStatus < 4))
                {
                    item.IdEventoStatus = 4;
                }
                retorno.Add(MapeiaResponse(item));
            }

            return retorno;
        }

        public ServiceResponse<List<EventoResponse>> PesquisarPorIdCategoria(int id)
        {
            var lista = _context.Eventos
                .Include(x => x.IdCategoriaEventoNavigation)
                .Include(x => x.IdEventoStatusNavigation)
                .Where(x => x.IdCategoriaEvento == id)
                .ToList();

            var retorno = new List<EventoResponse>();

            if (lista == null)
            {
                return new ServiceResponse<List<EventoResponse>>("Não existe eventos nessa categoria!");
            }
            else
            {
                foreach (var item in lista)
                {
                    retorno.Add(MapeiaResponse(item));
                }

                return new ServiceResponse<List<EventoResponse>>(retorno);

            }

        }

        public ServiceResponse<List<EventoResponse>> BuscarPorData(DateTime data)
        {
            var lista = _context.Eventos
                .Include(evento => evento.Participacoes)
                .Include(evento => evento.IdCategoriaEventoNavigation)
                .Include(evento => evento.IdEventoStatusNavigation)
                .Where(x => x.DataHoraInicio.Date == data).ToList();

            var resultado = new List<EventoResponse>();

            foreach (var item in lista)
            {
                resultado.Add(MapeiaResponse(item));
            }

            if (resultado == null)
            {
                return new ServiceResponse<List<EventoResponse>>("Não existe evento nesta data");
            }
            else
            {
                return new ServiceResponse<List<EventoResponse>>(resultado);
            }
        }

        public EventoResponse MapeiaResponse(Evento evento)
        {
            var retorno = new EventoResponse()
            {

                Nome = evento.Nome,
                DataHoraInicio = evento.DataHoraInicio,
                DataHoraFim = evento.DataHoraFim,
                Local = evento.Local,
                Descricao = evento.Descricao,
                LimiteVagas = evento.LimiteVagas,
                IdEventoStatus = evento.IdEventoStatus,
                IdEvento = evento.IdEvento,

            };

            if (evento.Participacoes != null)
            {
                retorno.Participacoes = new List<ParticipacaoResponse>();
                foreach (var item in evento.Participacoes)
                {
                    retorno.Participacoes.Add(new ParticipacaoResponse()
                    {
                        IdParticipacao = item.IdParticipacao,
                        LoginParticipante = item.LoginParticipante,
                        FlagPresente = item.FlagPresente,
                        Nota = item.Nota,
                        Comentario = item.Comentario,
                    });
                }
            }

            if (evento.IdCategoriaEventoNavigation != null)
                retorno.CategoriaEvento = new CategoriaEventoResponse()
                {
                    IdCategoriaEvento = evento.IdCategoriaEventoNavigation.IdCategoriaEvento,
                    NomeCategoria = evento.IdCategoriaEventoNavigation.NomeCategoria
                };

            if (evento.IdEventoStatusNavigation != null)
                retorno.StatusEvento = new StatusEventoResponse()
                {
                    IdEventoStatus = evento.IdEventoStatusNavigation.IdEventoStatus,
                    NomeStatus = evento.IdEventoStatusNavigation.NomeStatus
                };


            return retorno;
        }

        public ServiceResponse<EventoResponse> CadastrarNovo(EventoCreateRequest model)
        {
            var novoEvento = new Evento()
            {
                Nome = model.Nome,
                DataHoraInicio = model.DataHoraInicio,
                DataHoraFim = model.DataHoraFim,
                Local = model.Local,
                Descricao = model.Descricao,
                LimiteVagas = model.LimiteVagas,
                IdCategoriaEvento = model.IdCategoriaEvento,
                IdEventoStatus = 1,
            };


            if (model.DataHoraInicio >= model.DataHoraFim)
                return new ServiceResponse<EventoResponse>("A hora inicial tem que ser menor do que a hora final");

            if (model.DataHoraInicio.Date != model.DataHoraFim.Date)
                return new ServiceResponse<EventoResponse>("O evento deve ser realizado em um único dia");

            if (model.DataHoraInicio.ToString("dd/MM/yyyy") == DateTime.Now.ToString("dd/MM/yyyy"))
                return new ServiceResponse<EventoResponse>("O evento não pode ser realizado no dia corrente");

            if (model.DataHoraInicio < DateTime.Now)
                return new ServiceResponse<EventoResponse>("Data de inicial menor do que a data atual");

            _context.Add(novoEvento);
            _context.SaveChanges();

            return new ServiceResponse<EventoResponse>(MapeiaResponse(novoEvento));
        }

        public ServiceResponse<EventoResponse> EditarStatus(int id, StatusEventoUpdateRequest model)
        {
            var resultado = _context.Eventos.Find(id);
            if (resultado == null)
                return new ServiceResponse<EventoResponse>("Evento não encontrado");
            if (model.IdEventoStatus == 1 && resultado.IdEventoStatus != 2)
                return new ServiceResponse<EventoResponse>("Evento já foi iniciado, concluido ou cancelado");
            if (model.IdEventoStatus == 2 && resultado.DataHoraInicio.Date != DateTime.Today.Date)
                return new ServiceResponse<EventoResponse>("Evento só pode iniciar na data agendada");
            if (model.IdEventoStatus == 3 && resultado.IdEventoStatus != 2)
                return new ServiceResponse<EventoResponse>("O evento só pode ser  concluido se tiver em andamento");
            if (model.IdEventoStatus == 4 && resultado.DataHoraInicio.Date == DateTime.Today.Date)
                return new ServiceResponse<EventoResponse>("O evento não pode ser cancelado no mesmo dia");
            if (model.IdEventoStatus != null)
                resultado.IdEventoStatus = model.IdEventoStatus;

            _context.Entry(resultado).State = EntityState.Modified;
            _context.SaveChanges();

            return new ServiceResponse<EventoResponse>(MapeiaResponse(resultado));


        }

        public ServiceResponse<bool> Deletar(int id)
        {
            var resultado = _context.Eventos.Find(id);
            if(resultado == null)
                return new ServiceResponse<bool>("Evento não encontrado no banco de dados");

            _context.Remove(resultado);
            _context.SaveChanges();

            return new ServiceResponse<bool>(true);

        }

    }
}
