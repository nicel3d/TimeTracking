<template>
  <div>
    <v-activity-filter-component
      class="mb-5"
      v-model="filter"
    />
    <v-activity-table-component
      ref="table"
      :filter="filter"
      @on-edit="$refs.dialog.onView($event)"/>
    <v-activity-details-component ref="dialog" @on-success="$refs.table.onPagination()"/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VActivityTableComponent from '%/components/ActivityStaff/VActivityTableComponent.vue'
import VActivityDetailsComponent from '%/components/ActivityStaff/VActivityDetailsComponent.vue'
import VActivityFilterComponent from '%/components/ActivityStaff/VActivityFilterComponent.vue'
import { FilterRequest } from '%/stores/api/SwaggerDocumentationTypescript'

@Component({
  components: { VActivityFilterComponent, VActivityDetailsComponent, VActivityTableComponent }
})
export default class ActivityStaffView extends Vue {
  filter: FilterRequest = new FilterRequest({
    begDate: this.$moment().subtract(7, 'days').toDate(),
    endDate: this.$moment().toDate(),
    begHour: 7,
    endHour: 17
  })
}
</script>
