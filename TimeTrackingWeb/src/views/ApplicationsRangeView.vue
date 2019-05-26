<template>
  <div>
    <v-layout row wrap>
      <v-flex xs12>
        <v-filter-by-range-component v-model="filter"/>
      </v-flex>
      <v-flex xs12>
        <v-table-applications-by-range
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
import VTableApplicationsByRange from '%/components/Applications/VTableApplicationsByRange.vue'
import VWindowEditActivityStaff from '%/components/ActivityStaff/VWindowEditActivityStaff.vue'
import VFilterByRangeComponent from '%/components/VFilterByRangeComponent.vue'

@Component({
  components: { VFilterByRangeComponent, VWindowEditActivityStaff, VTableApplicationsByRange }
})
export default class ApplicationsRangeView extends Vue {
  filter: FilterRequest = filterDefault
}
</script>
