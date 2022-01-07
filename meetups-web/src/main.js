import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './router'
import moment from 'moment'

Vue.config.productionTip = false

Vue.filter('formatDate', function (value) {
  if (value) {
    return moment(String(value)).format('DD/MM/YYYY HH:mm')
  }
});

Vue.filter('formatHour', function (value) {
  if (value) {
    return moment(String(value)).format('HH:mm')
  }
});

new Vue({
  vuetify,
  router,
  render: h => h(App)
}).$mount('#app')
