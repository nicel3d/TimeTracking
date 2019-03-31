import Vue from 'vue'
import './plugins/vuetify'
import './plugins/vee-validate'
import router from './routes/Router'
import store from './store'
import App from './App.vue'
import './registerServiceWorker'

const moment: any = require('moment-timezone')
const VueMoment: any = require('vue-moment')

Vue.use(VueMoment, {
  moment
})

moment.locale('ru')
Vue.prototype.$appName = 'Time tracking web'
Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
