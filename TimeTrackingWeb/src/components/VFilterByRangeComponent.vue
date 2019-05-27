<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-card>
    <v-card-title primary-title>
      <h3 class="mb-0">Фильтр</h3>
    </v-card-title>

    <v-container>
      <v-layout row wrap>
        <v-flex xs12>
          <date-picker
            :clearable="false"
            width="100%"
            :value="[value.begDate, value.endDate]"
            range
            lang="ru"
            @change="onChangeDates"
            :input-attr="{required: true}"
            format="YYYY-MM-DD"
            confirm
          />
        </v-flex>
        <v-flex xs12>
          <v-range-slider
            :value="[value.begHour, value.endHour]"
            :tick-labels="hours"
            always-dirty
            min="0"
            :max="hours.length"
            thumb-label
            thumb-size="64"
            ticks="always"
            @change="onChangeHours"
          >
            <template v-slot:thumb-label="props">
          <span>
            {{ props.value.toString() }}
          </span>
            </template>
          </v-range-slider>
        </v-flex>
      </v-layout>
    </v-container>
  </v-card>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'
import DatePicker from 'vue2-datepicker'
import { FilterRequest } from '%/stores/api/SwaggerDocumentationTypescript'

@Component({
  components: { DatePicker }
})
export default class VFilterByRangeComponent extends Vue {
  @Prop() readonly value!: FilterRequest

  hours: number[] = []

  created () {
    this.hours = Array.from(new Array(24), (x, i) => i)
  }

  onChangeDates (val) {
    this.$emit('input', new FilterRequest({
      ...this.value,
      begDate: val[0],
      endDate: val[1]
    }))
  }

  onChangeHours (val) {
    this.$emit('input', new FilterRequest({
      ...this.value,
      begHour: val[0],
      endHour: val[1]
    }))
  }
}
</script>

<style>
</style>
