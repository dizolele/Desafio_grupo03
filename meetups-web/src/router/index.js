import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import CriarNovo from "../views/CriarNovo.vue"
import Admin from "../views/Admin.vue"
import User from "../views/Usuario.vue"
import MeusEventos from "../views/MeusEventos.vue"


Vue.use(VueRouter)

const routes = [
  {
    path: '/',
    name: 'Home',
    component: Home
  },

  {
    path: '/admin',
    name: 'Admin',
    component: Admin
  },

  {
    path: '/user',
    name: 'User',
    component: User
  },

  {
    path: '/criar-novo',
    name: 'CriarNovo',
    component: CriarNovo
  },
  {
    path: '/meus-eventos',
    name: 'MeusEventos',
    component: MeusEventos
  },
]


const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

export default router
