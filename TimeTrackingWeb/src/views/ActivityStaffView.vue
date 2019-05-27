<template>
  <div>
    <v-layout row wrap>
      <v-flex xs12>
        <v-filter-by-range-component v-model="filter"/>
      </v-flex>
      <v-flex xs12>
        <v-table-activity-staff
          ref="table"
          :filter="filter"
          @on-edit="$refs.dialog.onView($event)"
        />
      </v-flex>
    </v-layout>
    <v-window-edit-activity-staff ref="dialog" @on-success="$refs.table.onPagination()"/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { FilterRequest } from '%/stores/api/SwaggerDocumentationTypescript'
import { filterDefault } from '%/constants/FilterDefault'
import VTableActivityStaff from '%/components/ActivityStaff/VTableActivityStaff.vue'
import VWindowEditActivityStaff from '%/components/ActivityStaff/VWindowEditActivityStaff.vue'
import VFilterByRangeComponent from '%/components/VFilterByRangeComponent.vue'

@Component({
  components: { VFilterByRangeComponent, VWindowEditActivityStaff, VTableActivityStaff }
})
export default class ActivityStaffView extends Vue {
  filter: FilterRequest = filterDefault

  created () {
    const timeLineHandle = this.$store.state.timeLineHandle
    if (timeLineHandle) {
      const beg = this.$moment(timeLineHandle.value[0])
      const end = this.$moment(timeLineHandle.value[0])
      this.filter.begDate = beg.toDate()
      this.filter.endDate = end.toDate()
      this.filter.begHour = parseInt(beg.format('h'))
      this.filter.endHour = parseInt(end.format('h'))
    }
  }

  beforeDestroy () {
    this.$store.commit('setTimeLineHandle', null)
  }
}
</script>
