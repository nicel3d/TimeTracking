<template>
  <div>
    <v-staff-table-component/>
    <v-staff-edit-window/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VStaffTableComponent from '%/components/Staff/VStaffTableComponent.vue'
import VStaffEditWindow from '%/components/Staff/VStaffEditWindow.vue'

@Component({
  components: { VStaffEditWindow, VStaffTableComponent }
})
export default class ActivityStaffView extends Vue {
}
</script>
