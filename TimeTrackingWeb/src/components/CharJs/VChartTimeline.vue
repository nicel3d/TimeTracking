<template>
  <div>
    <canvas ref="chartCanvas" width="600" height="80"></canvas>
    <div style="opacity:0;" class="chartTooltip center"/>
  </div>
</template>

<script>
import { StateEnum } from '../../stores/api/SwaggerDocumentationTypescript'
import { ActivityStaffResponse } from '%/stores/api/SwaggerDocumentationTypescript'

export default {
  name: 'VChartTimeline',
  props: {
    dataset: {
      type: Array,
      default: () => ([])
    }
  },
  data: () => ({
    data: {
      type: 'timeline',
      options: {
        'colorFunction': function (text, data, dataset, index) {
          switch (text) {
            case StateEnum.Allowed:
              return Color('#41B883')
            case StateEnum.Forbidden:
              return Color('#E46651')
            default:
              return Color('transparent')
          }
        },
        'elements': {
          // 'colorFunction': function (text, data, dataset, index) {
          //   return Color('Red')
          // }, // не работает
          'showText': false,
          'textPadding': 4
        }
      },
      data: {}
    }
  }),

  watch: {
    /**
     * @param {ActivityStaffResponse[]} dataset
     */
    dataset (dataset = []) {
      const names = [...new Set(dataset.map(x => x.staffAlias))]
      const newDataset = {
        labels: names,
        datasets: []
      }
      names.forEach(name => {
        newDataset.datasets.push(
          {
            data: dataset
              .filter(x => x.staffAlias === name)
              .map(x => ([
                x.begDate.toISOString(),
                x.endDate.toISOString(),
                x.statusApplication
              ]))
          }
        )
      })
      this.data.data = newDataset
      const chart = new Chart(this.$refs.chartCanvas, this.data)
    }
  },

  mounted () {
    // const chart = new Chart(this.$refs.chartCanvas, this.data)
  }
}
</script>

<style scoped>

</style>
