<template>
  <div>
    
    <div class="header">
    <v-container> 
     <h1 class="titulo"><span id="meetups">MEETUPS</span> <span id="iteris">ITERIS</span></h1>
        <p class="paragrafo" >
          Participe dos nossos MEETUPS. Meetups são eventos técnicos, para
          compartilhar conhecimento e aumentar a interação entre os
          colaboradores da empresa.
        </p>
    </v-container>
    </div>
    
    <div class="main">
        <v-app>
        <v-layout align-center justify-center>
          <v-flex xs12 sm8 md4>
            <v-card class="elevation-12">
              
              <v-toolbar id="toobar" dark color="primary">
                <v-toolbar-title>Login</v-toolbar-title>
                <v-spacer></v-spacer>
              </v-toolbar>

              <v-card-text>
                <v-form>
                  <v-text-field v-model="Nome" label="Nome completo" type="text"></v-text-field>
                
                <v-chip-group
                  v-model="selection"
                  column
                  color="blue"
                >
                  <v-chip filter outlined>Participante</v-chip>

                  <v-chip filter outlined>Administrador</v-chip>
                </v-chip-group>
                
                </v-form>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <router-link to="/user">
                    <v-btn @click="guardarNome" style="color: white" color="primary" v-if="selection == 0" href="/user">Acessar</v-btn>
                </router-link>
                <router-link to="/admin">
                    <v-btn @click="guardarNome" style="color: white" color="primary" v-if="selection == 1" href="/admin">Acessar</v-btn>
                </router-link>                       
              </v-card-actions>

            </v-card>
          </v-flex>
        </v-layout>
  </v-app>
     
  </div>
  </div>
</template>


<script>
export default {
  name: 'Home',
  components: {},
  data: () => ({
    selection: 0,
    Nome: "",
  }),

  methods: {
    guardarNome() {
      localStorage.removeItem("nomeParticipante");
      this.nomeParticipante = this.Nome;
      localStorage.setItem("nomeParticipante", this.nomeParticipante);
    }
  }
}
</script>

<style>
.header {
  width: 100%;
  text-align: center;
  font-family: Ubuntu, Cantarell, "Open Sans", "Helvetica Neue", sans-serif;
  background-color: #1565C0;
}

a {
  text-decoration: none;
}

#meetups{
  color:white;
}

#iteris{
  color:#f1a261;
}

.titulo {
    margin-top: 5%;
    width: 100%;
    font-size: 48px;
}

.link {
  text-decoration: none;
}
.paragrafo {
    text-align: center;
    font-size: 18px;
    color: #ffffff;
    padding: 12px;
}

#toobar {
  margin-top:-50%;
}
</style>