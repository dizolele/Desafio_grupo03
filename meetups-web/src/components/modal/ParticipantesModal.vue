<template>
  <div class="text-center">
    <v-dialog v-model="dialog" width="500">
      <template v-slot:activator="{ on, attrs }">
        <v-btn
          color="#00426C"
          dark
          v-bind="attrs"
          v-on="on"
          @click="gerarListaParticipantes()"
        >
          Ver participantes
        </v-btn>
      </template>

      <v-card>
        <v-card-title class="text-h5 blue darken-2 white--text">
          Lista de participantes
        </v-card-title>

        <v-card-text class="mt-4">
          <ul>
            <li :key="index" v-for="(participante, index) in participantes" class="mt-2 d-flex justify-space-around"> 
              <p>{{ participante.loginParticipante }}</p>
              <v-btn class="ml-2" :class="[participante.flagPresente == true ? 'success' : 'error']" x-small @click="alterarPresenca(participante.idParticipacao, participante.flagPresente, index)"
                >Presente: {{participante.flagPresente}}</v-btn
              >
            </li>
          </ul>
        </v-card-text>

        <v-divider></v-divider>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" text @click="dialog = false"> Fechar </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { consultaParticipantesId, alterarParticipacao } from "../../services/apiService";
export default {
  data() {
    return {
      dialog: false,
      rating: 5,
      comentario: "",
      rules: [
        (value) => !!value || "Required.",
        (value) => (value && value.length <= 1000) || "Max 1000 characters",
      ],

      participantes: [],
      flagParticipacao: [],
    };
  },
  props: {
    idEvento: Number,
  },
  mounted() {},
  methods: {
    gerarListaParticipantes() {
      consultaParticipantesId(this.idEvento).then((participantes) => {
        this.participantes = participantes;
      });
    },
    alterarPresenca(idParticipante, flag, index) {
      var flagObject = { flagPresente: !flag }
      this.flagParticipacao = flag;
      alterarParticipacao(idParticipante, flagObject);
      this.participantes[index].flagPresente = !this.participantes[index].flagPresente; // forçando atualizacao lista
    }
  },
};
</script>

<style>
</style>