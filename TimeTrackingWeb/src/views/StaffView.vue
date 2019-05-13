<template>
  <div>
    <v-staff-table-component/>
    <v-window-edit-staff/>
    <v-groups-add-window/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VStaffTableComponent from '%/components/Staff/VStaffTableComponent.vue'
import VGroupsAddWindow from '%/components/Groups/VGroupsAddWindow.vue'
import VWindowEditStaff from '%/components/Staff/VWindowEditStaff.vue'

@Component({
  components: { VWindowEditStaff, VGroupsAddWindow, VStaffTableComponent }
})
export default class ActivityStaffView extends Vue {
}
</script>
