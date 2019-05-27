<template>
  <div>
    <canvas ref="chartCanvas" width="600" height="80"></canvas>
    <div style="opacity:0;" class="chartTooltip center"/>
  </div>
</template>

<script>
import { StateEnum } from '../../stores/api/SwaggerDocumentationTypescript'
import { ActivityStaffResponse } from '%/stores/api/SwaggerDocumentationTypescript'
import { RouterNameEnum } from '../../constants/RouterConstant'

/**
 * @param chart
 * @param {event} evt
 * @returns {null|{label: *, value: *}}
 */
function clickHandler (chart, evt) {
  const firstPoint = chart.getElementAtEvent(evt)[0]

  if (firstPoint) {
    const label = chart.data.labels[firstPoint._index]
    const value = chart.data.datasets[firstPoint._datasetIndex].data[firstPoint._index]
    return { label, value }
  }

  return null
}

export default {
  name: 'VChartTimeline',
  props: {
    dataset: {
      type: Array,
      default: () => ([])
    }
  },
  data: () => ({
    chart: null,
    canvas: null,
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
      this.chart.data = newDataset
      this.chart.update()
    }
  },

  mounted () {
    this.canvas = this.$refs.chartCanvas
    const chart = this.chart = new Chart(this.canvas, this.data)
    const vm = this
    this.canvas.onclick = function (evt) {
      const handle = clickHandler(chart, evt)
      if (handle) {
        vm.$store.commit('setTimeLineHandle', handle)
        vm.$router.push({ name: RouterNameEnum.ActivityStaff })
      }
    }
  }
}
</script>

<style scoped>

</style>
