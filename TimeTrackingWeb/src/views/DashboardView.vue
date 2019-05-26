<template>
  <div>
    <v-chart-dashboard/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VChartTimeline from '%/components/VChartTimeline.vue'
import VChartDashboard from '%/components/Dashboard/VChartDashboard.vue'

@Component({
  components: { VChartDashboard, VChartTimeline }
})
export default class DashboardView extends Vue {
}
</script>
