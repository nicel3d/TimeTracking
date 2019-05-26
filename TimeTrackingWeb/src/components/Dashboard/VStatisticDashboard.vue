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
              <v-flex class="text-xs-right">0</v-flex>
            </v-layout>
          </div>
          <div class="mt-3">
            <v-layout justify-space-between>
              <v-flex>Использование запрещенных приложений</v-flex>
              <v-flex class="text-xs-right">1</v-flex>
            </v-layout>
          </div>
          <div class="mt-3">
            <v-layout justify-space-between>
              <v-flex>Общее кол-во записанной информации</v-flex>
              <v-flex class="text-xs-right">1</v-flex>
            </v-layout>
          </div>
        </v-layout>
      </v-container>
    </v-card-text>
  </v-card>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component
export default class VStatisticDashboard extends Vue {
  state: number = 0
  states = [
    {
      state: 0,
      text: 'yes'
    }
  ]
}
</script>

<style lang="scss">
</style>
