import axios from 'axios'
//import { part } from 'core-js/core/function';

const apiUrlBase = "https://localhost:7201/api";

  export function consultaEventos() {
    return axios.get(`${apiUrlBase}/Eventos`).then((response) => {
      const eventos = response.data;
      return eventos?.map((evento) => {
        return {
          idEvento: evento.idEvento,
          nome: evento.nome,
          dataHoraInicio: evento.dataHoraInicio,
          dataHoraFim: evento.dataHoraFim,
          local: evento.local,
          descricao: evento.descricao,
          limiteVagas: evento.limiteVagas,
          categoriaEvento: evento.categoriaEvento.nomeCategoria,
          statusEvento: evento.statusEvento.nomeStatus
        };
      });
    });
  }

  export function criarEvento(novoEvento) {
    return axios.post(`${apiUrlBase}/Eventos`, novoEvento).then((response) => {
      const evento = response.data.objetoRetorno;
      return {
        nome: evento.nome,
        dataHoraInicio: evento.dataHoraInicio,
        dataHoraFim: evento.dataHoraFim,
        local: evento.local,
        descricao: evento.descricao,
        limiteVagas: evento.limiteVagas,
        idCategoriaEvento: evento.idCategoriaEvento
      };
    });
  }

  export function consultaCategoriaId(id) {
    return axios.get(`${apiUrlBase}/Eventos/nome/${id}`).then(response => {
        const eventos = response.data.objetoRetorno
        return eventos?.map(evento => {
            return {
                idEvento: evento.idEvento,
                nome: evento.nome,
                dataHoraInicio: evento.dataHoraInicio,
                dataHoraFim: evento.dataHoraFim,
                local: evento.local,
                descricao: evento.descricao,
                limiteVagas: evento.limiteVagas,
                categoriaEvento: evento.categoriaEvento.nomeCategoria,
                statusEvento: evento.statusEvento.nomeStatus,
            }
        })
    })
}

export function consultaData(data) {
  return axios.get(`${apiUrlBase}/Eventos/date/${data}`).then(response => {
      const eventos = response.data;
      return eventos?.map(evento => {
          return {
              idEvento: evento.idEvento,
              nome: evento.nome,
              dataHoraInicio: evento.dataHoraInicio,
              dataHoraFim: evento.dataHoraFim,
              local: evento.local,
              descricao: evento.descricao,
              limiteVagas: evento.limiteVagas,
              categoriaEvento: evento.categoriaEvento.nomeCategoria,
              statusEvento: evento.statusEvento.nomeStatus
          }
      })
  })
}

export function alterarEventoStatus(id, status) {
  return axios.put(`${apiUrlBase}/Eventos/${id}`, status).then((response) => {
          const evento = response.data.objetoRetorno;
          return {
              idEventoStatus: evento.idEventoStatus
          };
      })
}


//Participantes

export function consultaParticipantesId(id) {
  return axios.get(`${apiUrlBase}/Participacao/${id}`).then((response) => {
    const participante = response.data;
    return participante?.map((participante) => {
      return {
        idParticipacao : participante.idParticipacao,
        idEvento : participante.idEvento,
        loginParticipante : participante.loginParticipante,
        flagPresente : participante.flagPresente,
        nota : participante.nota,
        comentario : participante.comentario
      }
    })
  })
}

export function inscreverParticipante(novoParticipante) {
  return axios.post(`${apiUrlBase}/Participacao/Participacao`, novoParticipante).then((response) => {
      const participante = response.data.objetoRetorno;
      return {
          idEvento: participante.idEvento,
          loginParticipante: participante.loginParticipante
      };
  })
}


export function alterarParticipacao(id, flag) {
  return axios.put(`${apiUrlBase}/Participacao/${id}`, flag)
      .then((response) => {
          const participacao = response.data.objetoRetorno;
          return {
              flagPresente: participacao.flagPresente,
          };
      })
}


export function participantes(nome){
  return axios.get(`${apiUrlBase}/Participacao/Participantes/${nome}`)
  .then((response) => {
    const participacao = response.data.objetoRetorno;
    return {
      idParticipacao: participacao.idParticipacao,
      idEvento: participacao.idEvento,
      loginParticipante: participacao.loginParticipante,
      flagPresente: participacao.flagPresente,
      nota: participacao.nota,
      comentario: participacao.comentario,
      eventos: participacao.eventos,
      categoriaEvento: participacao.categoriaEvento,
      statusEvento: participacao.statusEvento
    }
  })
}

export function enviarAvaliacao(id, avaliacao) {
  return axios.put(`${apiUrlBase}/Participacao/Avaliar/${id}`, avaliacao)
  .then((response) => {
      const avaliacao = response.data.objetoRetorno;
      return {
          nota: avaliacao.nota,
          comentario: avaliacao.comentario
      };
  })
}

export function consultarParticipantes(nome) {
  return axios.get(`${apiUrlBase}/Participacao/Participantes/${nome}`).then((response) => {
    const participante = response.data;
    return participante?.map((participante) => {
      return {
        idParticipacao : participante.idParticipacao,
        idEvento : participante.idEvento,
        loginParticipante : participante.loginParticipante,
        flagPresente : participante.flagPresente,
        nota : participante.nota,
        comentario : participante.comentario,
        eventos: participante.eventos,
        categoriaEvento: participante.eventos.categoriaEvento.nomeCategoria,
        statusEvento: participante.eventos.statusEvento.nomeStatus,
      }
    })
  })
}












