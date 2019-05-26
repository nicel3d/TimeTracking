<template>
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Текущая активность</h3>
      <v-spacer></v-spacer>
    </v-card-title>
    <v-card-text>
      <v-container>
        <v-layout row wrap>
          <v-flex d-flex xs12 md6>
            <v-chart-pie-container/>
          </v-flex>
          <v-flex d-flex xs12 md6>
            <v-layout column>
              <div class="mt-3">
                <v-layout row wrap justify-space-between>
                  <v-flex>Пользователей онлайн</v-flex>
                  <v-flex class="text-xs-right">0</v-flex>
                </v-layout>
              </div>
              <div class="mt-3">
                <v-layout justify-space-between>
                  <v-flex>Пользователей офлайн</v-flex>
                  <v-flex class="text-xs-right">1</v-flex>
                </v-layout>
              </div>
              <div class="mt-3">
                <v-layout justify-space-between>
                  <v-flex>Общее кол-во пользователей</v-flex>
                  <v-flex class="text-xs-right">1</v-flex>
                </v-layout>
              </div>
            </v-layout>
          </v-flex>
        </v-layout>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VChartPieContainer from '%/components/CharJs/VChartPieContainer.vue'

@Component({
  components: { VChartPieContainer }
})
export default class VActiveUserDashboard extends Vue {
}
</script>

<style lang="scss">
</style>
