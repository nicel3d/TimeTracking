<template>
  <v-breadcrumbs divider="/">
    <v-breadcrumbs-item
      v-for="(route, key) in $route.matched"
      v-if="route.meta && route.meta.breadcrumb"
      :to="route.patch"
      :key="key">
      {{ route.meta.breadcrumb }}
    </v-breadcrumbs-item>
  </v-breadcrumbs>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component
export default class VBreadcrumbsRouter extends Vue {
}
</script>

<style scoped>

</style>
