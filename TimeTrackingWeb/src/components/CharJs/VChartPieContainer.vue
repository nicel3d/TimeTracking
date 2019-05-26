<template>
  <div class="container px-0 py-0">
    <v-chart-pie
      :data="chartData"
      :options="{legend: false}"
    />
  </div>
</template>

<script>
import VChartPie from './VChartPie'

export default {
  name: 'VChartPieContainer',
  components: { VChartPie },
  data: () => ({
    chartData: {
      labels: ['Онлайн', 'Офлайн'],
      datasets: [
        {
          label: 'Data One',
          backgroundColor: ['#41B883', '#E46651'],
          data: [0, 1]
        }
      ]
    }
  })
}
</script>

<style lang="scss" scoped>
  .container {
    max-width: 300px;
  }
</style>
