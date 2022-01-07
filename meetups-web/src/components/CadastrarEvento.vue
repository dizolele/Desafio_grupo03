<template>
  <v-container>
    <v-form ref="form" lazy-validation>
      <v-text-field
        label="Nome"
        :rules="[rules.required]"
        hide-details="auto"
        hint="Obs: Máximo 250 caracteres"
        v-model="novoEvento.nome"
      ></v-text-field>
      <v-text-field
        label="Local"
        prepend-inner-icon="mdi-map-marker"
        :rules="[rules.required]"
        hide-details="auto"
        hint="Obs: Máximo 250 caracteres"
        v-model="novoEvento.local"
      ></v-text-field>
      <v-text-field
        label="Descricao"
        :rules="[rules.required, rules.min]"
        hide-details="auto"
        hint="Obs: Máximo 1000 caracteres"
        v-model="novoEvento.descricao"
      ></v-text-field>
      <v-text-field
        label="Quantidade de Vagas"
        :rules="[rules.required]"
        hide-details="auto"
        v-model="novoEvento.limiteVagas"
      ></v-text-field>
      <v-select

        label="Escolha uma categoria"

        v-model="novoEvento.idCategoriaEvento"

        :items="categorias"

        item-text="nome"

        item-value="id"

      >
      </v-select>
      <v-text-field
        label="DataHoraInicio"
        :rules="[rules.required]"
        hide-details="auto"
        type="datetime-local"
        hint="Obs: Eventos não podem ser criados para a data atual"
        v-model="novoEvento.dataHoraInicio"
      ></v-text-field>
      <v-text-field
        label="DataHoraFim"
        :rules="[rules.required]"
        hide-details="auto"
        type="datetime-local"
        hint="Obs: Eventos devem terminar na mesma data do inicio"
        v-model="novoEvento.dataHoraFim"
      ></v-text-field>
      <div class= "botaocadastro">
      <v-btn 
      dark
      color="#1565C0"
      class="mr-4" 
      @click="criarNovo()">
        Criar Novo Evento
      </v-btn>
      <v-btn 
      dark
      color="#1565C0"
      class="mr-4" 
      >
      <router-link to ="/admin" class="linkeventos">
        Visualizar Eventos
      </router-link>
      </v-btn>
      </div>
      
    </v-form>
  </v-container>
</template>

<script>
import {criarEvento} from "../services/apiService";

export default {
  name: "CadastroEvento",
  components: {
   
  },
  data: () => {
    return {
      novoEvento: {
        nome: "",
        local: "",
        descricao: "",
        dataHoraInicio: new Date().toISOString,
        dataHoraFim: "",
        limiteVagas: 0,
        idCategoriaEvento: 0

      },
      eventos: [],
      categorias: [

        { id: 1, nome: "Backend" },
        { id: 2, nome: "Frontend" },
        { id: 3, nome: "Mobile" },
        { id: 4, nome: "Cloud & DevOps" },
        { id: 5, nome: "No Code & Low Code" },
        { id: 6, nome: "UI/UX" },
        { id: 7, nome: "Data & Analytics" },
        { id: 8, nome: "Gestão Ágil" },
        { id: 9, nome: "Qualidade" },

      ],
      rules: {
          required: value => !!value || 'Required.',
      }

    };
  },
  methods: {
    criarNovo() {
      //console.log(this.novoEvento.DataHoraInicio);
      criarEvento(this.novoEvento).then((evento) => {
        this.eventos.push(evento);
        this.novoEvento = {
          nome: "",
          local: "",
          descricao: "",
          dataHoraInicio: "",
          dataHoraFim: "",
          idCategoriaEvento: "",
          limiteVagas: "",
          nomeCategoriaEvento: "",
          nomeStatusEvento: "",
        };
        alert("Evento Cadastrado com sucesso")
      });
    },
  },
};
</script>

<style>
.botaocadastro {
  margin-top: 30px;
}
.v-application .linkeventos {
  text-decoration: none;
  color: white;
}
</style>