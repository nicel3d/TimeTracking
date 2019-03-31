import Vue from 'vue'
import Vuetify from 'vuetify/lib'
import 'vuetify/src/stylus/app.styl'
import ru from 'vuetify/src/locale/ru'

Vue.use(Vuetify, {
  iconfont: 'fa',
  lang: {
    locales: { ru },
    current: 'ru'
  }
})
