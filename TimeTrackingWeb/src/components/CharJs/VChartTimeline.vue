<template>
  <div>
    <canvas ref="chartCanvas" width="600" height="80"></canvas>
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
            return Color('#41B883')
          } else if (text === 'Off') {
            return Color('#E46651')
          } else {
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
      'data': {
        'labels': ['user 1', 'nikel'],
        'datasets': [
          {
            'data': [
              [
                '2018-05-23T07:00:00',
                '2018-05-23T10:40:44',
                'Unknown'
              ]
            ]
          },
          {
            'data': [
              [
                '2018-05-23T00:00:00',
                '2018-05-23T24:00:00',
                'Unknown'
              ],
              [
                '2018-05-23T07:00:00',
                '2018-05-23T10:00:00',
                'On'
              ],
              [
                '2018-05-23T10:00:00',
                '2018-05-23T10:40:44',
                'Unknown'
              ],
              [
                '2018-05-23T10:40:00',
                '2018-05-23T13:00:00',
                'Off'
              ],
              [
                '2018-05-23T13:00:00',
                '2018-05-23T14:00:00',
                'On'
              ],
              [
                '2018-05-23T14:00:00',
                '2018-05-23T15:30:00',
                'Off'
              ],
              [
                '2018-05-23T15:31:00',
                '2018-05-23T17:00:00',
                'On'
              ]
            ]
          }
        ]
      }
    }
  }),

  mounted () {
    const chart = new Chart(this.$refs.chartCanvas, this.data)
  }
}
</script>

<style scoped>

</style>
