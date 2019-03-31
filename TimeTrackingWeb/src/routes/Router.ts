import Vue from 'vue'
import Router from 'vue-router'
import { RouterNameEnum } from '%/constants/RouterConstant'

Vue.use(Router)

export default new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      component: () => import('../templates/UnregisteredTemplate.vue'),
      redirect: '/authorization',
      children: [
        {
          path: 'authorization',
          name: RouterNameEnum.Authorization,
          component: () => import('../views/AuthView.vue')
        }
      ]
    },
    {
      path: '/main',
      component: () => import('../templates/AuthorizedTemplate.vue'),
      children: [
        {
          path: 'home',
          name: RouterNameEnum.Dashboard,
          component: () => import('../views/DashboardView.vue'),
          meta: {
            breadcrumb: 'Dashboard'
          }
        }
      ]
    }
  ]
})
