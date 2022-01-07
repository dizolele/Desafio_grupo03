<template>
  <v-card
    class="mx-auto"
    max-width="400"
    height="420"
    :style="
      evento.statusEvento == 'Aberto para inscrições'
        ? 'borderBottom: 5px solid #2196F3' : evento.statusEvento == 'Em andamento'
        ? 'borderBottom: 5px solid #FFB300' : evento.statusEvento == 'Concluído'
        ? 'borderBottom: 5px solid #4CAF50' : 'borderBottom: 5px solid #F44336'
    "
  >
    <v-img
      class="white--text align-end"
      height="200px"
      :src="gerarImagem()"
    >
      <v-card-title style="background: linear-gradient(180deg, rgba(2,0,36,0) 0%, rgba(21,101,192,0.4458158263305322) 35%, rgba(0,212,255,1) 100%)"><span class="mt-5">{{ evento.nome }}</span></v-card-title>
    </v-img>
    <v-card-subtitle class="pb-0 d-flex justify-space-between">
      <v-chip
        class="mb-2 white--text"
        :class="[
          evento.categoriaEvento == 'Backend'
            ? backend : evento.categoriaEvento == 'Frontend'
            ? frontend : evento.categoriaEvento == 'Mobile'
            ? mobile : evento.categoriaEvento == 'Cloud & DevOps'
            ? cloudDev : evento.categoriaEvento == 'No Code & Low Code'
            ? noCode : evento.categoriaEvento == 'UI/UX'
            ? uiUx : evento.categoriaEvento == 'Data & Analytics'
            ? dataAnalytics : evento.categoriaEvento == 'Gestão Ágil'
            ? gestaoAgil : qualidade,
        ]"
      >
        {{ evento.categoriaEvento }}
      </v-chip>
      <p class="mt-2">
        <v-icon class="mr-1" :color="gerarCorIcon()">
          {{ gerarIcon() }} </v-icon
        >{{ evento.statusEvento }}
      </p>
    </v-card-subtitle>

    <v-card-text class="text--primary">
      <div class="d-flex justify-space-between flex-wrap">
        <v-tooltip top color="primary">
        <template v-slot:activator="{ on, attrs }">
        <div class="local"
        v-bind="attrs"
        v-on="on">
          <v-icon class="mr-1" color="#005183">mdi-map-marker</v-icon
          >{{ evento.local }}
        </div>
        </template>
        <span>{{evento.local}}</span>
        </v-tooltip>
        <div>
          <v-icon class="mr-1" color="#005183">mdi-clock-outline</v-icon
          >{{ evento.dataHoraInicio | formatDate }} -
          {{ evento.dataHoraFim | formatHour }}
        </div>
      </div>
      <v-tooltip top color="primary">
      <template v-slot:activator="{ on, attrs }">
      <div class="descricao mt-2"
      v-bind="attrs"
      v-on="on">
        <v-icon class="mr-1">mdi-card-text-outline</v-icon
        >{{ evento.descricao }}
      </div>
      </template>
      <span>{{evento.descricao}}</span>
    </v-tooltip>
    </v-card-text>

    <v-card-actions v-if="tipoUsuario == 'user'" class="justify-center">
      <inscrever-modal
        v-if="evento.statusEvento == 'Aberto para inscrições' && meusEventos === false"
        :loginParticipante="this.nomeParticipante" 
        :idEvento="evento.idEvento"
      /> 
      <avaliar-modal v-if="evento.statusEvento == 'Concluído' && flagPresenteParticipante" :idEvento="evento.idEvento"/>
    </v-card-actions>

    <v-card-actions v-if="tipoUsuario == 'admin'" class="justify-center">
      <participantes-modal :idEvento="evento.idEvento" v-if="evento.statusEvento === 'Concluído' || evento.statusEvento === 'Em andamento'" />
      <v-btn v-if="evento.statusEvento === 'Aberto para inscrições' && dataHoje == formatedDate" @click="alterarStatusEvento(2)">Iniciar evento</v-btn>
      <v-btn v-if="evento.statusEvento === 'Aberto para inscrições' && dataHoje2.getTime() < formatedDate2 && dataHoje2.toDateString() != formatedDateString" @click="alterarStatusEvento(4)">Cancelar evento</v-btn>
      <v-btn v-if="evento.statusEvento === 'Em andamento'" @click="alterarStatusEvento(3)">Concluir evento</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script>
import {alterarEventoStatus, consultarParticipantes} from "./../services/apiService";
import AvaliarModal from "./modal/AvaliarModal.vue";
import InscreverModal from "./modal/InscreverModal.vue";
import ParticipantesModal from "./modal/ParticipantesModal.vue";
//import moment from 'moment'

export default {
  components: { InscreverModal, AvaliarModal, ParticipantesModal },
  data: () => ({
    //tipoUsuario: "user",
    evento2: {
      nome: "Frontend",
      dataHoraInicio: "04/01/2022 16:35",
      dataHoraFim: "04/01/2022 17:35",
      descricao:
        "Lorem ipsum lorem ipsum Lorem ipsum lorem ipsumLorem ipsum lorem ipsumLorem ipsum lorem ipsumLorem ipsum lorem ipsumLorem ipsum lorem ipsumLorem ipsum lorem ipsum",
      categoriaEvento: 2 /**/,
      local: "Teams",
      statusEvento: 1,
    },

    backend: "teal darken-3",
    frontend: "blue darken-4",
    mobile: "indigo darken-2",
    cloudDev: "blue lighten-1",
    noCode: "cyan lighten-2",
    uiUx: "teal lighten-1",
    dataAnalytics: "green darken-1",
    gestaoAgil: "cyan darken-3",
    qualidade: "blue-grey darken-2",

    dataHoje: '',//moment().format("YYYY/MM/DD"),
    formatedDate: '',

    dataHoje2: new Date(),
    formatedDate2: '',
    formatedDateString: '',
    
    nomeParticipante: localStorage.getItem("nomeParticipante"),
    flagPresenteParticipante: false,
  }),
  props: {
    evento: {
      idEvento: Number,
      nome: String,
      dataHoraInicio: Date,
      dataHoraFim: Date,
      local: String,
      descricao: String,
      limiteVagas: Number,
      categoriaEvento: Object,
      statusEvento: Object,
    },
    tipoUsuario: String,
    meusEventos: Boolean,
  },
  updated() {
    this.dataHoje = new Date().toDateString();
    this.formatedDate = new Date(this.evento.dataHoraInicio).toDateString();
    //console.log(this.evento.nome + ' ' + this.formatedDate)
    //console.log(this.dataHoje < this.formatedDate)

    this.formatedDate2 = new Date(this.evento.dataHoraInicio).getTime();
    this.formatedDateString = new Date(this.evento.dataHoraInicio).toDateString();

    if(this.evento.statusEvento === 'Concluído') this.verificarPresente();
  },
  mounted() {
    this.dataHoje = new Date().toDateString();
    this.formatedDate = new Date(this.evento.dataHoraInicio).toDateString();
    if(this.evento.statusEvento === 'Concluído') this.verificarPresente();
  },
  methods: {
    gerarIcon() {
      if (this.evento.statusEvento == "Aberto para inscrições") {
        return "mdi-account-multiple-plus";
      }
      if (this.evento.statusEvento == "Em andamento") {
        return "mdi-clock-fast";
      }
      if (this.evento.statusEvento == "Concluído") {
        return "mdi-check-circle";
      }
      if (this.evento.statusEvento == "Cancelado") {
        return "mdi-close-circle";
      }
    },
    gerarCorIcon() {
      if (this.evento.statusEvento == "Aberto para inscrições") {
        return "blue";
      }
      if (this.evento.statusEvento == "Em andamento") {
        return "amber darken-1";
      }
      if (this.evento.statusEvento == "Concluído") {
        return "green";
      }
      if (this.evento.statusEvento == "Cancelado") {
        return "red";
      }
    },
    alterarStatusEvento(idStatus) {
        alterarEventoStatus(this.evento.idEvento, {idEventoStatus: idStatus});
        if(idStatus === 2) {
            this.evento.statusEvento = "Em andamento"; // forçando atualizacao lista
        } else if (idStatus === 3) {
            this.evento.statusEvento = "Concluído";
        } else if (idStatus === 4) {
            this.evento.statusEvento = "Cancelado";
        }
  },

  gerarImagem() {
      switch(this.evento.categoriaEvento) {
        case 'Backend':
          return 'https://static.vecteezy.com/ti/vetor-gratis/p3/3416059-flat-design-backend-of-developer-concept-vetor.jpg';
        case 'Frontend':
          return 'https://thumbs.dreamstime.com/b/ilustra%C3%A7%C3%A3o-do-vetor-conceito-abstrato-de-desenvolvimento-front-end-interface-site-codifica%C3%A7%C3%A3o-desenvolvedores-e-programa%C3%A7%C3%A3o-195723253.jpg';
        case 'Mobile':
          return 'https://img.myloview.com.br/adesivos/app-development-vector-illustration-concept-suitable-for-web-landing-page-ui-mobile-app-editorial-design-flyer-banner-and-other-related-occasion-700-223853564.jpg';  
        case 'Cloud & DevOps':  
          return 'https://img.freepik.com/vetores-gratis/engenheiros-devops-criacao-do-produto-do-programa-trabalho-em-equipe-bem-coordenado-colaboracao-de-desenvolvedores-de-software-interacao-produtiva-dos-profissionais-programadores-codificando-e-desenvolvendo-ui-conceito-de-vetor_533410-116.jpg?size=626&ext=jpg';
        case 'No Code & Low Code':
          return 'https://www.seguetech.com/wp-content/uploads/2019/04/segue-blog-low-code-no-code-MAIN.png';
        case 'UI/UX':
          return 'https://image.freepik.com/vetores-gratis/conceito-de-desenvolvimento-de-aplicativo-ui-e-ux_52683-48848.jpg';
        case 'Data & Analytics':
          return 'https://img.freepik.com/free-vector/big-isolated-employee-working-office-workplace-flat-illustration_1150-41780.jpg?size=626&ext=jpg';
        case 'Gestão Ágil':
          return 'https://img.freepik.com/vetores-gratis/quadro-kanban-gerenciamento-de-projetos-agil-colaboracao-da-equipe-do-escritorio-e-ilustracao-em-vetor-coerencia-do-processo-de-projetos_102902-1494.jpg?size=626&ext=jpg';      
        case 'Qualidade':
          return 'https://media.istockphoto.com/vectors/questions-and-answers-q_a-with-people-persons-with-smartphones-ask-vector-id1195021297?b=1&k=20&m=1195021297&s=612x612&w=0&h=I-EPbOTTKPhpfp21eOz6gM4jVUQx_lUgOdxtOTLc92w=';
      }
    },
    verificarPresente() {
      consultarParticipantes(this.nomeParticipante).then(eventos => {
        for(let x = 0; x < eventos.length; x++) {
          if(eventos[x].idEvento === this.evento.idEvento && eventos[x].flagPresente === true) {
              this.flagPresenteParticipante = true;
          }
        }
      })
    }
  
  }
}
</script>

<style>
.local {
  white-space: nowrap; 
  width: 40%; 
  overflow: hidden;
  text-overflow: ellipsis; 
}
.descricao {
  white-space: nowrap; 
  width: 90%; 
  overflow: hidden;
  text-overflow: ellipsis; 
}
</style>