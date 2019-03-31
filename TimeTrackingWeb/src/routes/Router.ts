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
      name: RouterNameEnum.Authorization,
      component: () => import('../layout/UnregisteredLayout.vue'),
    },
    {
      path: '/main',
      component: () => import('../layout/WrapperLayout.vue'),
      children: [
        {
          path: 'home',
          name: RouterNameEnum.Home,
          component: () => import('../views/Home.vue'),
          meta: {
            breadcrumb: 'Home'
          }
        }
      ]
    }
  ]
})
