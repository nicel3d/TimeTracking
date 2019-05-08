<template>
  <div>
    <v-activity-filter-component
      class="mb-5"
      v-model="filter"
    />
    <v-applications-range-table-component
      ref="table"
      :filter="filter"
      @on-edit="$refs.dialog.onView($event)"/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VActivityTableComponent from '%/components/ActivityStaff/VActivityTableComponent.vue'
import VActivityFilterComponent from '%/components/ActivityStaff/VActivityFilterComponent.vue'
import { FilterRequest } from '%/stores/api/SwaggerDocumentationTypescript'
import VApplicationsRangeTableComponent from '%/components/Applications/VApplicationsRangeTableComponent.vue'
import { filterDefault } from '%/constants/FilterDefault'

@Component({
  components: {
    VApplicationsRangeTableComponent,
    VActivityFilterComponent,
    VActivityTableComponent
  }
})
export default class ApplicationsRangeView extends Vue {
  filter: FilterRequest = filterDefault
}
</script>
