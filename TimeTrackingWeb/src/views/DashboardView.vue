<template>
  <div>
    <v-layout row>
      <v-flex xs12>
        <v-chart-dashboard/>
      </v-flex>
    </v-layout>
    <br>
    <v-layout row>
      <v-flex xs12 md6>
        <v-active-user-dashboard/>
      </v-flex>
    </v-layout>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VChartDashboard from '%/components/Dashboard/VChartDashboard.vue'
import VActiveUserDashboard from '%/components/Dashboard/VActiveUserDashboard.vue'

@Component({
  components: { VActiveUserDashboard, VChartDashboard }
})
export default class DashboardView extends Vue {
}
</script>
