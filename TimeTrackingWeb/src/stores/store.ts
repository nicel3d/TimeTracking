import Vue from 'vue'
import Vuex from 'vuex'
import { WSApi } from '%/stores/api/SwaggerDocumentationTypescript'
import './api/InterceptConfigFetch'
import { RouterNameEnum } from '%/constants/RouterConstant'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    timeLineHandle: null,
    token: null,
    api: new WSApi('http://localhost:5000')
  },
  mutations: {
    setTimeLineHandle: (state, data) => (state.timeLineHandle = data)
  },
  actions: {
    logout ({ state }) {
      state.token = null
      Vue.router.replace({ name: RouterNameEnum.Authorization })
    },
    setToken ({ state }, token) {
      state.token = token
      Vue.router.push({ name: RouterNameEnum.Dashboard })
    }
  }
})
