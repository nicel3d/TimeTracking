import Vue from 'vue'
import './plugins/vuetify'
import './plugins/vee-validate'
import router from './routes/Router'
import store from './stores/store'
import App from './App.vue'
import './registerServiceWorker'
import VuePictureSwipe from 'vue-picture-swipe'

const moment: any = require('moment-timezone')
const VueMoment: any = require('vue-moment')

Vue.component('vue-picture-swipe', VuePictureSwipe)
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
