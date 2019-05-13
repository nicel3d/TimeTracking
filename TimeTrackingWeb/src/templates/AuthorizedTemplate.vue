<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-app>
    <v-navigation-drawer
      :mini-variant="miniVariant"
      :clipped="clipped"
      v-model="drawer"
      persistent
      enable-resize-watcher
      fixed
      app
    >
      <v-list
        class="pt-0"
        dense>
        <v-divider/>
        <template v-for="(item, key) in items">
          <v-list-tile
            v-if="!item.items"
            :key="key"
            :to="{name: item.name}"
            value="true"
          >
            <v-list-tile-action>
              <v-icon>{{ item.icon }}</v-icon>
            </v-list-tile-action>
            <v-list-tile-content>
              <v-list-tile-title>{{ item.title }}</v-list-tile-title>
            </v-list-tile-content>
          </v-list-tile>
          <v-list-group
            v-else
            :key="key"
            value="true"
            :prepend-icon="item.icon"
          >
            <template v-slot:activator>
              <v-list-tile>
                <v-list-tile-title>{{ item.title }}</v-list-tile-title>
              </v-list-tile>
            </template>
            <v-list-tile
              :class="{'pl-4': !miniVariant}"
              v-for="(child, index) in item.items"
              :key="index"
              :to="{name: child.name}"
            >
              <v-list-tile-action>
                <v-icon>{{ child.icon }}</v-icon>
              </v-list-tile-action>
              <v-list-tile-title>{{ child.title }}</v-list-tile-title>
            </v-list-tile>
          </v-list-group>
        </template>
        <v-list-tile @click.prevent="dialog = true">
          <v-list-tile-action>
            <v-icon>fas fa-sign-out-alt</v-icon>
          </v-list-tile-action>
          <v-list-tile-content>
            <v-list-tile-title>Выход</v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
      </v-list>
    </v-navigation-drawer>
    <v-toolbar
      :clipped-left="clipped"
      app
    >
      <v-toolbar-side-icon @click.stop="drawer = !drawer"/>
      <v-btn
        icon
        @click.stop="miniVariant = !miniVariant">
        <v-icon v-html="miniVariant ? 'fas fa-chevron-right' : 'fas fa-chevron-left'"/>
      </v-btn>
      <v-btn
        icon
        @click.stop="clipped = !clipped">
        <v-icon>fas fa-globe</v-icon>
      </v-btn>
      <v-toolbar-title v-text="$appName"/>
      <v-spacer/>
      <!-- <v-btn
        icon
        @click.stop="rightDrawer = !rightDrawer">
        <v-icon>fas fa-bars</v-icon>
      </v-btn> -->
    </v-toolbar>
    <v-content>

      <v-breadcrumbs-router/>

      <v-container
        fluid
        class="wrapper">
        <v-fade-transition mode="out-in">
          <router-view/>
        </v-fade-transition>
      </v-container>
    </v-content>
    <v-navigation-drawer
      :right="right"
      v-model="rightDrawer"
      temporary
      fixed
      app
    >
      <v-list>
        <v-list-tile @click="right = !right">
          <v-list-tile-action>
            <v-icon>fas fa-exchange-alt</v-icon>
          </v-list-tile-action>
          <v-list-tile-title>Switch drawer (click me)</v-list-tile-title>
        </v-list-tile>
      </v-list>
    </v-navigation-drawer>

    <v-footer
      :fixed="fixed"
      app>
      <v-flex
        grey
        darken-3
        py-3
        text-xs-center
        white--text
        xs12
      >
        &copy; {{ $moment().format('YYYY') }} - {{ $appName }}
      </v-flex>
    </v-footer>

    <v-logout-confirm-dialog :dialog.sync="dialog"/>
  </v-app>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VBreadcrumbsRouter from '../components/VBreadcrumbsRouter.vue'
import VLogoutConfirmDialog from '../components/VLogoutConfirmDialog.vue'
import { RouterNameEnum } from '%/constants/RouterConstant'

@Component({
  components: {
    VBreadcrumbsRouter,
    VLogoutConfirmDialog
  }
})
export default class AuthorizedTemplate extends Vue {
  clipped = false
  drawer = true
  fixed = false
  dialog = false
  items = [
    {
      icon: 'fas fa-home',
      title: 'Dashboard',
      name: RouterNameEnum.Dashboard
    },
    {
      icon: 'fas fa-users',
      title: 'Пользователи',
      name: RouterNameEnum.Staff
    },
    {
      icon: 'fas fa-user-friends',
      title: 'Группы',
      name: RouterNameEnum.Groups
    },
    {
      icon: 'fas fa-users-cog',
      title: 'Активность пользователей',
      name: RouterNameEnum.ActivityStaff
    },
    {
      icon: 'fas fa-project-diagram',
      title: 'Приложения',
      items: [
        {
          icon: 'fas fa-project-diagram',
          title: 'Приложения, список',
          name: RouterNameEnum.ApplicationsList
        },
        {
          icon: 'fas fa-project-diagram',
          title: 'Приложения за период',
          name: RouterNameEnum.ApplicationsRange
        }
      ]
    }
  ]
  miniVariant = false
  right = true
  rightDrawer = false
}
</script>

<style lang="scss">
.v-list__tile__title {
  color: #000000;
}

@media (min-width: 1390px) {
  .container.wrapper {
    margin: 0;
  }
}
</style>
