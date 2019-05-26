<template>
  <div>
    <canvas ref="chartCanvas" width="600" height="160"></canvas>
    <div style="opacity:0;" class="chartTooltip center"/>
  </div>
</template>

<script>
export default {
  name: 'VChartTimeline',
  data: () => ({
    data: {
      'type': 'timeline',
      'options': {
        'colorFunction': function (text, data, dataset, index) {
          if (text === 'On') {
            return Color('green')
          } else if (text === 'Off') {
            return Color('red')
          } else {
            return Color('transparent')
          }
        },
        'elements': {
          'colorFunction': function (text, data, dataset, index) {
            return Color('Red')
          },
          'showText': false,
          'textPadding': 4
        }
      },
      'data': {
        'labels': [
          'Cool Graph',
          'heater1'
        ],
        'datasets': [
          {
            'data': [
              [
                '2018-01-22T16:00:00.000Z',
                '2018-01-23T05:40:44.626Z',
                'Unknown'
              ]
            ],
          },
          {
            'data': [
              [
                '2018-01-22T16:00:00.000Z',
                '2018-01-22T20:00:00.000Z',
                'On'
              ],
              [
                '2018-01-22T20:00:00.000Z',
                '2018-01-23T04:57:55.437Z',
                'Off'
              ],
              [
                '2018-01-23T04:57:55.437Z',
                '2018-01-23T05:40:44.626Z',
                'On'
              ]
            ]
          }
        ]
      },
    }
  }),

  mounted () {
    const chart = new Chart(this.$refs.chartCanvas, this.data)
  }
}
</script>

<style scoped>

</style>
