<template>
  <div>
    <v-activity-table-component ref="table" @on-edit="$refs.dialog.onView($event)"/>
    <v-activity-details-component ref="dialog" @on-success="$refs.table.onPagination()"/>
    Dashboard
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VActivityTableComponent from '%/components/VActivityTableComponent.vue'
import VActivityDetailsComponent from '%/components/VActivityDetailsComponent.vue'

@Component({
  components: { VActivityDetailsComponent, VActivityTableComponent }
})
export default class DashboardView extends Vue {
}
</script>
