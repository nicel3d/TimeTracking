import Vue from 'vue'
import store from '../store'
import fetchIntercept from 'fetch-intercept'

fetchIntercept.register({
  request (url: string, config: any) {
    config.headers['Access-Control-Allow-Origin'] = '*'

    const token = store.state.token
    if (token !== null) {
      config.headers['Authorization'] = `Bearer ${token}`
    }

    return [url, config]
  },

  requestError (error: any) {
    return new Promise<any>((resolve, reject) => reject(error))
  },

  response (response: Response) {
    if (response.status !== 200) {
      switch (response.status) {
        case 403:
          // Vue.prototype.$notify.danger('you do not have permission to view this section')
          break
        case 500:
          // errors from server or incorrect parameters of dispatch in methods
          break
        case 204:
          // not content
          break
        default:
          break
      }
    }

    return response
  },

  responseError (error: any) {
    return new Promise<any>((resolve, reject) => reject(error))
  }
})
