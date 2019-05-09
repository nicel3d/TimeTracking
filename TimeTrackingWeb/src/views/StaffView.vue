<template>
  <div>
    <v-staff-table-component
      ref="table"
      :filter="filter"
      @on-edit="$refs.dialog.onView($event)"/>
<!--    <v-activity-details-component ref="dialog" @on-success="$refs.table.onPagination()"/>-->
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import { FilterRequest } from '%/stores/api/SwaggerDocumentationTypescript'
import { filterDefault } from '%/constants/FilterDefault'
import VStaffTableComponent from '%/components/VStaffTableComponent.vue'

@Component({
  components: { VStaffTableComponent }
})
export default class ActivityStaffView extends Vue {
  filter: FilterRequest = filterDefault
}
</script>
