<template>
  <v-container fluid>
    <AppBar />
    <v-data-iterator
      :items="items"
      :items-per-page.sync="itemsPerPage"
      :page.sync="page"
      :search="search"
      :sort-by="sortBy.toLowerCase()"
      :sort-desc="sortDesc"
      hide-default-footer
    >
      <template v-slot:header>
        <v-toolbar dark color="blue darken-3" class="mb-1">
          <v-text-field
            v-model="search"
            clearable
            flat
            solo-inverted
            hide-details
            prepend-inner-icon="mdi-magnify"
            label="Search"
          ></v-text-field>
          <template v-if="$vuetify.breakpoint.mdAndUp">
            <v-spacer></v-spacer>
            <!-- Filtro por Categoria -->
            <v-combobox
              label="Escolha uma categoria"
              v-model="selectedCategory"
              :items="categorias"
              item-text="nome"
              item-value="id"
              @change="filtrarCategoria"
              class="mt-5"
            >
            </v-combobox>
            <v-spacer></v-spacer>

            <!-- Filtro por DATA -->
            <v-col cols="12" sm="6" md="4" class="mt-5">
              <v-menu
                ref="menu"
                v-model="menu"
                :close-on-content-click="false"
                :return-value.sync="date"
                transition="scale-transition"
                offset-y
                min-width="auto"
              >
                <template v-slot:activator="{ on, attrs }">
                  <v-text-field
                    v-model="date"
                    label="Escolha uma data"
                    prepend-icon="mdi-calendar"
                    readonly
                    v-bind="attrs"
                    v-on="on"
                  ></v-text-field>
                </template>
                <v-date-picker v-model="date" no-title scrollable>
                  <v-spacer></v-spacer>
                  <v-btn text color="primary" @click="menu = false">
                    Cancel
                  </v-btn>
                  <v-btn text color="primary" @click="filtrarData"> OK </v-btn>
                </v-date-picker>
              </v-menu>
            </v-col>
          </template>
        </v-toolbar>
      </template>

      <template v-slot:default="props">
        <v-row>
          <v-col
            v-for="item in props.items"
            :key="item.name"
            cols="12"
            sm="6"
            md="4"
            lg="3"
          >
            <evento-card-padrao
              class="mt-5"
              :tipoUsuario="tipoUser"
              :evento="item"
              :meusEventos="true"
            />
          </v-col>
        </v-row>
      </template>

      <template v-slot:footer>
        <v-row class="mt-2" align="center" justify="center">
          <span class="grey--text">Items per page</span>
          <v-menu offset-y>
            <template v-slot:activator="{ on, attrs }">
              <v-btn
                dark
                text
                color="primary"
                class="ml-2"
                v-bind="attrs"
                v-on="on"
              >
                {{ itemsPerPage }}
                <v-icon>mdi-chevron-down</v-icon>
              </v-btn>
            </template>
            <v-list>
              <v-list-item
                v-for="(number, index) in itemsPerPageArray"
                :key="index"
                @click="updateItemsPerPage(number)"
              >
                <v-list-item-title>{{ number }}</v-list-item-title>
              </v-list-item>
            </v-list>
          </v-menu>

          <v-spacer></v-spacer>

          <span class="mr-4 grey--text">
            Page {{ page }} of {{ numberOfPages }}
          </span>
          <v-btn
            fab
            dark
            color="blue darken-3"
            class="mr-1"
            @click="formerPage"
          >
            <v-icon>mdi-chevron-left</v-icon>
          </v-btn>
          <v-btn fab dark color="blue darken-3" class="ml-1" @click="nextPage">
            <v-icon>mdi-chevron-right</v-icon>
          </v-btn>
        </v-row>
        <div class="botaohome">
        <v-btn v-if="tipoUser === 'user'" dark color="#1565C0" class="mr-4 white--text">
            <router-link to="/user" class="linkinicio">
              Voltar para Página Inicial
            </router-link>
          </v-btn>
        </div>
      </template>
    </v-data-iterator>
  </v-container>
</template>

<script>
import {
  consultarParticipantes,
 // consultaCategoriaId,
  consultaData,
} from "../services/apiService.js";
import EventoCardPadrao from "../components/EventoCardPadrao.vue";
import AppBar from "../components/AppBar";
export default {
  name: "MeusEventos",
  props: {},
  data() {
    return {
      itemsPerPageArray: [4, 8, 12],
      search: "",
      filter: {},
      sortDesc: false,
      page: 1,
      itemsPerPage: 4,
      sortBy: "name",

      tipoUser: "user",

      keys: [],
      items: [],

      categorias: [
        { id: 0, nome: "Todos" },
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
      selectedCategory: "Todos",

      date: new Date(Date.now() - new Date().getTimezoneOffset() * 60000)
        .toISOString()
        .substr(0, 10),
      menu: false,
      modal: false,
      menu2: false,

      nomeParticipante: localStorage.getItem("nomeParticipante"),
    };
  },
  components: {
    EventoCardPadrao,
    AppBar,
  },
  computed: {
    numberOfPages() {
      return Math.ceil(this.items.length / this.itemsPerPage);
    },
    filteredKeys() {
      return this.keys.filter((key) => key !== "Name");
    },
  },
  methods: {
    nextPage() {
      if (this.page + 1 <= this.numberOfPages) this.page += 1;
    },
    formerPage() {
      if (this.page - 1 >= 1) this.page -= 1;
    },
    updateItemsPerPage(number) {
      this.itemsPerPage = number;
    },
    filtrarCategoria() {
      if (this.selectedCategory.id === 0) {
        this.items = [];
        consultarParticipantes(this.nomeParticipante).then((eventos) => {
          for (let x = 0; x < eventos.length; x++) {
            this.items.push(eventos[x].eventos);
          }
          for (let y = 0; y < this.items.length; y++) {
            this.items[y].statusEvento = this.items[y].statusEvento.nomeStatus;
            this.items[y].categoriaEvento =
              this.items[y].categoriaEvento.nomeCategoria;
          }
        });
      } else {
        this.items = [];
        consultarParticipantes(this.nomeParticipante).then((eventos) => {
          for (let x = 0; x < eventos.length; x++) {
            if(eventos[x].eventos.idCategoriaEvento == this.selectedCategory.id) {
              this.items.push(eventos[x].eventos);
            }
          }
          for (let y = 0; y < this.items.length; y++) {
            this.items[y].statusEvento = this.items[y].statusEvento.nomeStatus;
            this.items[y].categoriaEvento =
              this.items[y].categoriaEvento.nomeCategoria;
          }
        });
      }
      this.date = new Date(Date.now() - new Date().getTimezoneOffset() * 60000)
        .toISOString()
        .substr(0, 10);
    },
    filtrarData() {
      this.$refs.menu.save(this.date);
      consultaData(this.date).then((eventos) => {
        this.items = eventos;
        console.log(this.items);
      });
      this.selectedCategory = "Todos";
    },
  },
  mounted() {
    consultarParticipantes(this.nomeParticipante).then((eventos) => {
      //this.items = eventos; // for e mapear no items
      for (let x = 0; x < eventos.length; x++) {
        this.items.push(eventos[x].eventos);
      }
      console.log(this.items);
      for (let y = 0; y < this.items.length; y++) {
        // Esse segundo for é para fazer o statusEvento pegar o nome dele ao invés do objeto id e nome inteiro
        this.items[y].statusEvento = this.items[y].statusEvento.nomeStatus;
        this.items[y].categoriaEvento =
          this.items[y].categoriaEvento.nomeCategoria;
      }
    });
  },
};
</script>

<style>
.linkcadastro {
  text-decoration: none;
  color: white !important;
}
.botaotelacadastro {
  margin-top: 20px;
  margin-left: 45%;
}
.v-application .linkinicio {
  text-decoration: none;
  color: white;
}
.botaohome {
  margin-top: 20px;
  margin-left: 43%;
}
</style>