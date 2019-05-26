<template>
  <v-breadcrumbs class="pb-0 px-3" divider="/" :items="items"/>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { oc } from 'ts-optchain'

@Component
export default class VBreadcrumbsRouter extends Vue {
  get items () {
    return this.$route.matched
      .filter(item => oc(item.meta).breadcrumb(null))
      .map((item, index, array) => ({
        text: item.meta.breadcrumb,
        disabled: index + 1 <= array.length,
        href: item.path
      }))
  }
}
</script>

<style scoped>

</style>
