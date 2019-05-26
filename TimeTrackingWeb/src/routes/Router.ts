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
            breadcrumb: 'Главный экран'
          }
        },
        {
          path: 'staff',
          name: RouterNameEnum.Staff,
          component: () => import('../views/StaffView.vue'),
          meta: {
            breadcrumb: 'Пользователи'
          }
        },
        {
          path: 'groups',
          name: RouterNameEnum.Groups,
          component: () => import('../views/GroupsView.vue'),
          meta: {
            breadcrumb: 'Группы'
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
          path: 'settings',
          name: RouterNameEnum.Settings,
          component: () => import('../views/SettingsView.vue'),
          meta: {
            breadcrumb: 'Настройки для клиентских модулей'
          }
        },
        {
          path: 'applications',
          name: RouterNameEnum.ApplicationsList,
          component: () => import('../views/ApplicationsView.vue'),
          meta: {
            breadcrumb: 'Приложения'
          }
        },
        {
          path: 'applications-range',
          name: RouterNameEnum.ApplicationsRange,
          component: () => import('../views/ApplicationsRangeView.vue'),
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
