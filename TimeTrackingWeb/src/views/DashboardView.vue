<template>
  <div>
    <v-layout row wrap>
      <v-flex xs12>
        <v-filter-by-day-dashboard v-model="filter"/>
      </v-flex>
    </v-layout>
    <v-layout row wrap>
      <v-flex xs12>
        <v-chart-dashboard :filter="filter"/>
      </v-flex>
    </v-layout>
    <v-layout row wrap>
      <v-flex xs12 md6 d-flex>
        <v-active-user-dashboard/>
      </v-flex>
      <v-flex xs12 md6 d-flex>
        <v-statistic-dashboard :filter="filter"/>
      </v-flex>
    </v-layout>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VChartDashboard from '%/components/Dashboard/VChartDashboard.vue'
import VActiveUserDashboard from '%/components/Dashboard/VActiveUserDashboard.vue'
import VStatisticDashboard from '%/components/Dashboard/VStatisticDashboard.vue'
import VFilterByDayDashboard from '%/components/Dashboard/VFilterByDayDashboard.vue'

@Component({
  components: { VFilterByDayDashboard, VStatisticDashboard, VActiveUserDashboard, VChartDashboard }
})
export default class DashboardView extends Vue {
  filter: Date = this.$moment().subtract(3, 'days').toDate()
}
</script>
