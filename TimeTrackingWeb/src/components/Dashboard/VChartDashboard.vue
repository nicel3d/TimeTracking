<template>
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Активность пользователей</h3>
      <v-spacer></v-spacer>
    </v-card-title>
    <div class="mx-2">
      <template v-if="dataset.length">
        <v-btn color="primary" dark class="mb-2" @click.prevent>Экспорт в xlsx</v-btn>
        <v-btn color="primary" dark class="mb-2" @click.prevent>Экспорт в csv</v-btn>
      </template>
    </div>
    <v-card-text>
      <v-chart-timeline :dataset="dataset"/>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'
import VChartTimeline from '%/components/CharJs/VChartTimeline.vue'
import { ActivityStaffResponse } from '%/stores/api/SwaggerDocumentationTypescript'

@Component({
  components: { VChartTimeline }
})
export default class VChartDashboard extends Vue {
  @Prop() filter!: Date

  dataset: ActivityStaffResponse[] = []

  @Watch('filter')
  loadActivity () {
    this.$store.state.api.dashboard_GetActivityStaffByDate(this.filter)
      .then(res => (this.dataset = res))
      .catch(res => this.$root.$emit('snackbar', res))
  }

  mounted () {
    this.loadActivity()
  }
}
</script>

<style lang="scss">
</style>
