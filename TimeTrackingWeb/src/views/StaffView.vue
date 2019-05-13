<template>
  <div>
    <v-staff-table-component/>
    <v-staff-edit-window/>
    <v-groups-add-window/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VStaffTableComponent from '%/components/Staff/VStaffTableComponent.vue'
import VStaffEditWindow from '%/components/Staff/VStaffEditWindow.vue'
import VGroupsAddWindow from '%/components/Groups/VGroupsAddWindow.vue'

@Component({
  components: { VGroupsAddWindow, VStaffEditWindow, VStaffTableComponent }
})
export default class ActivityStaffView extends Vue {
}
</script>
