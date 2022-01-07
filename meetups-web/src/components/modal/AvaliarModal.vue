<template>
  <div class="text-center">
    <v-dialog v-model="dialog" width="500">
      <template v-slot:activator="{ on, attrs }">
        <v-btn color="#00426C" dark v-bind="attrs" v-on="on">
          Avaliar
        </v-btn>
      </template>

      <v-card>
        <v-card-title class="text-h5 grey lighten-2">
          Avalie esse evento
        </v-card-title>

        <v-card-text class="mt-2">
          Dê uma nota de 1 a 5
          <v-rating
            v-model="rating"
            background-color="orange lighten-3"
            color="orange"
            large
          ></v-rating>
        </v-card-text>

        <v-card-text class="">
          Deixe um comentário:
          <v-text-field
            label="Comentário"
            :rules="rules"
            hide-details="auto"
            v-model="comentario"
          ></v-text-field>
        </v-card-text>

        <v-divider></v-divider>

        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="primary" text @click="avaliar">
            Avaliar
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
// import { enviarAvaliacao, participantes } from '../../services/apiService';
import {consultarParticipantes, enviarAvaliacao} from '../../services/apiService';
export default {
  data() {
    return {
      dialog: false,
      rating: 5,
      comentario: "",
      rules: [
        value => !!value || 'Required.',
        value => (value && value.length <= 1000) || 'Max 1000 characters',
      ],
      eventosDoParticipante: [],
      idEventoParticipante: 0,
      nomeParticipante: localStorage.getItem("nomeParticipante"),
    };
  },
  props: {
    idEvento: Number
  },
  methods: {
    avaliar() {
      this.dialog = false;
      console.log(this.comentario + "   " + typeof this.rating);
      enviarAvaliacao( {nota:this.rating, comentario:this.comentario})
      

      consultarParticipantes(this.nomeParticipante).then((eventos) => {
        this.eventosDoParticipante = eventos;
        //console.log(this.eventosDoParticipante);
        for(let x = 0; x < this.eventosDoParticipante.length; x++) {
          if(this.eventosDoParticipante[x].idEvento === this.idEvento) {
            this.idEventoParticipante = this.eventosDoParticipante[x].idParticipacao;
          }
        }
        //console.log(this.idEventoParticipante);
      }).then((participacao) => {
        console.log(participacao);
        enviarAvaliacao(this.idEventoParticipante, {nota: this.rating, comentario: this.comentario});
      })
    }
  }
}     
</script>

<style>

</style>