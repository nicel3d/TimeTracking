<template>
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Фильтр по дню</h3>
    </v-card-title>

    <v-container>
      <v-layout row wrap>
        <v-flex xs12>
          <date-picker
            :clearable="false"
            width="100%"
            :value="value"
            lang="ru"
            @change="onChange"
            :input-attr="{required: true}"
            format="YYYY-MM-DD"
            confirm
          />
        </v-flex>
      </v-layout>
    </v-container>
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import DatePicker from 'vue2-datepicker'

@Component({
  components: { DatePicker }
})
export default class VFilterByDayDashboard extends Vue {
  @Prop() readonly value!: Date | null

  onChange (val) {
    this.$emit('input', val)
  }
}
</script>

<style>
</style>
