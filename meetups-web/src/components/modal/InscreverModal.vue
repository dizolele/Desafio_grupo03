<template>
  <div class="text-center">
    <v-dialog v-model="dialog" width="500">
      <template v-slot:activator="{ on, attrs }">
        <v-btn color="#00426C" dark v-bind="attrs" v-on="on">
          Participar
        </v-btn>
      </template>

      <v-card>
        <v-card-title class="text-h5 blue darken-2 white--text">
          Confirme sua participação
        </v-card-title>

        <v-card-text class="mt-4"> Nome: {{ loginParticipante }} </v-card-text>

        <v-divider></v-divider>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" text @click="inscreverParticipante">
            Inscrever
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- SNACKBAR -->
    <v-snackbar v-model="snackbar" :timeout="timeout" tile :color="wasSubscribed ? 'success' : 'red'">
      {{ text }}

      <template v-slot:action="{ attrs }">
        <v-btn color="white" text v-bind="attrs" @click="snackbar = false">
          Ok
        </v-btn>
      </template>
    </v-snackbar>

  </div>
</template>

<script>
import { inscreverParticipante } from "../../services/apiService";
export default {
  data() {
    return {
      dialog: false,

      snackbar: false,
      text: 'Inscrição realizada com sucesso!',
      timeout: 5000,
      wasSubscribed: false,
    };
  },
  props: {
    idEvento: Number,
    loginParticipante: String,
  },
  methods: {
    inscreverParticipante() {
      this.wasSubscribed = false; // reseta
      inscreverParticipante({
        idEvento: this.idEvento,
        loginParticipante: this.loginParticipante,
      }).then(participante => {console.log(participante); this.sucessoInscricao()});
      this.dialog = false;
      if(!this.wasSubscribed) {
        this.text = 'Não foi possível se inscrever nesse evento.';
      }
      this.snackbar = true;
    },
    sucessoInscricao() {
      this.text = 'Inscrição realizada com sucesso!';
      this.wasSubscribed = true;
    }
  },
};
</script>

<style>
</style>