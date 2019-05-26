<template>
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Активность пользователей</h3>
      <v-spacer></v-spacer>
    </v-card-title>
    <v-card-text>
      <v-chart-timeline :dataset="dataset"/>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VChartTimeline from '%/components/CharJs/VChartTimeline.vue'
import { ActivityStaffResponse } from '%/stores/api/SwaggerDocumentationTypescript'

@Component({
  components: { VChartTimeline }
})
export default class VChartDashboard extends Vue {
  dataset: ActivityStaffResponse[] = []

  mounted () {
    this.$store.state.api.dashboard_GetActivityStaffByDate(this.$moment().subtract(2, 'days').toDate())
      .then(res => (this.dataset = res))
      .catch(res => this.$root.$emit('snackbar', res))
  }
}
</script>

<style lang="scss">
</style>
