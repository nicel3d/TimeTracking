<template>
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Активность пользователей</h3>
      <v-spacer></v-spacer>
    </v-card-title>
    <v-card-text>
      <v-chart-timeline/>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VChartTimeline from '%/components/CharJs/VChartTimeline.vue'

@Component({
  components: { VChartTimeline }
})
export default class VChartDashboard extends Vue {
}
</script>

<style lang="scss">
</style>
