<template>
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Статистика</h3>
      <v-spacer></v-spacer>
    </v-card-title>
    <v-card-text>
      <v-container>
        <v-layout justify-end column fill-height>
          <div class="mt-3">
            <v-layout justify-space-between>
              <v-flex>Использование разрешенных приложений</v-flex>
              <v-flex class="text-xs-right">{{ timeAllowedApplication }}</v-flex>
            </v-layout>
          </div>
          <div class="mt-3">
            <v-layout justify-space-between>
              <v-flex>Использование запрещенных приложений</v-flex>
              <v-flex class="text-xs-right">{{ timeForbiddenApplication }}</v-flex>
            </v-layout>
          </div>
          <div class="mt-3">
            <v-layout justify-space-between>
              <v-flex>Общее кол-во записанной информации</v-flex>
              <v-flex class="text-xs-right">{{ timeAllApplication }}</v-flex>
            </v-layout>
          </div>
        </v-layout>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator'

@Component
export default class VStatisticDashboard extends Vue {
  @Prop() filter!: Date

  timeAllowedApplication: string = '00:00:00'
  timeForbiddenApplication: string = '00:00:00'
  timeAllApplication: string = '00:00:00'

  @Watch('filter')
  loadStatistic () {
    this.$store.state.api.dashboard_GetStatisticByDate(this.filter)
      .then(res => {
        this.timeAllApplication = res.timeAllApplication
        this.timeForbiddenApplication = res.timeForbiddenApplication
        this.timeAllowedApplication = res.timeAllowedApplication
      })
      .catch(res => this.$root.$emit('snackbar', res))
  }

  mounted () {
    this.loadStatistic()
  }
}
</script>

<style lang="scss">
</style>
