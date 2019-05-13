<template>
  <div>
    <v-table-staff/>
    <v-window-edit-staff/>
    <v-window-add-group/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VTableStaff from '%/components/Staff/VTableStaff.vue'
import VWindowEditStaff from '%/components/Staff/VWindowEditStaff.vue'
import VWindowAddGroup from '%/components/Groups/VWindowAddGroup.vue'

@Component({
  components: { VWindowAddGroup, VWindowEditStaff, VTableStaff }
})
export default class ActivityStaffView extends Vue {
}
</script>
