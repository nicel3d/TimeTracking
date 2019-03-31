import Vue from 'vue'
import Vuex from 'vuex'
import { WSApi } from '%/stores/api/SwaggerDocumentationTypescript'
import './api/InterceptConfigFetch'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    token: null,
    api: new WSApi('http://localhost:5000')
  },
  mutations: {},
  actions: {}
})
