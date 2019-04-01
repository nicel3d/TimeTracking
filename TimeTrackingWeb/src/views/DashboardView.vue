<template>
  <div>
    Dashboard
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component
export default class DashboardView extends Vue {
  mounted () {
    this.$store.state.api.activityStaff_GetAll()
      .then(res => {
        console.error(res)
      })
  }
}
</script>
