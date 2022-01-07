<template>
  <v-app>
      <AppBar/>
      <div class="cadastro">
      <h1 class="titulocadastro">Cadastrar novo evento:</h1>
      <CadastroEvento/>
      </div>
  </v-app>
</template>

<script>
import AppBar from "../components/AppBar"
import CadastroEvento from "../components/CadastrarEvento.vue";
export default {
    name: "CriarNovo",
    components: {
        CadastroEvento,
        AppBar,
    }
}
</script>

<style>
.titulocadastro {
    text-align: center;
    font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
    color: #1565C0;
    text-decoration: underline;
}

.cadastro {
    margin-top: 20px;
}
</style>