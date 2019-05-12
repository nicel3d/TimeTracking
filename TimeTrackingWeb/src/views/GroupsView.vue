<template>
  <div>
    <v-groups-table-component ref="table"/>
    <v-groups-add-window/>
    <v-groups-details-window/>
    <v-application-group-add-window/>
    <v-application-group-edit-window/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VGroupsTableComponent from '%/components/Groups/VGroupsTableComponent.vue'
import VGroupsAddWindow from '%/components/Groups/VGroupsAddWindow.vue'
import VGroupsDetailsWindow from '%/components/Groups/VGroupsDetailsWindow.vue'
import VApplicationGroupAddWindow from '%/components/ApplicationGroup/VApplicationGroupAddWindow.vue'
import VApplicationGroupEditWindow from '%/components/ApplicationGroup/VApplicationGroupEditWindow.vue'

@Component({
  components: {
    VApplicationGroupEditWindow,
    VApplicationGroupAddWindow,
    VGroupsDetailsWindow,
    VGroupsAddWindow,
    VGroupsTableComponent
  }
})
export default class ActivityStaffView extends Vue {
}
</script>
