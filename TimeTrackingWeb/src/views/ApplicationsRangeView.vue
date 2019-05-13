<template>
  <div>
    <v-filter-by-range-component
      class="mb-5"
      v-model="filter"
    />
    <v-table-applications-by-range
      ref="table"
      :filter="filter"
      @on-edit="$refs.dialog.onView($event)"
    />
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
