import Vue from 'vue'
import Router from 'vue-router'
import { RouterNameEnum } from '%/constants/RouterConstant'
import store from '%/stores/store'

Vue.use(Router)

const router = new Router({
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
        },
        {
          path: 'activity-staff',
          name: RouterNameEnum.ActivityStaff,
          component: () => import('../views/ActivityStaffView.vue'),
          meta: {
            breadcrumb: 'Активность пользователя'
          }
        },
        {
          path: 'applications',
          name: RouterNameEnum.Applications,
          component: () => import('../views/ApplicationsView.vue'),
          meta: {
            breadcrumb: 'Приложения'
          }
        }
      ],
      beforeEnter: (to, from, next) => {
        const token = store.state.token
        if (!token) {
          const query = { next: to.path.toString() }
          if (to.query.next) {
            query.next = to.query.next.toString()
          }

          next({ name: RouterNameEnum.Authorization, query: query })
        } else {
          next()
        }
      }
    }
  ]
})

export default Vue.router = router
