<template>
  <div>
    <v-groups-table-component ref="table"/>
    <v-groups-add-windows/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VGroupsTableComponent from '%/components/Groups/VGroupsTableComponent.vue'
import VGroupsAddWindows from '%/components/Groups/VGroupsAddWindows.vue'

@Component({
  components: { VGroupsAddWindows, VGroupsTableComponent }
})
export default class ActivityStaffView extends Vue {
}
</script>
