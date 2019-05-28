<template>
  <div>
    <v-table-groups/>
    <v-window-add-group/>
    <v-window-edit-group/>
    <v-window-add-application-group/>
    <v-window-edit-application-group/>
    <v-window-add-group-to-staff/>
    <v-window-add-group-application-title/>
    <v-window-edit-group-application-title/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VTableGroups from '%/components/Groups/VTableGroups.vue'
import VWindowAddGroup from '%/components/Groups/VWindowAddGroup.vue'
import VWindowEditGroup from '%/components/Groups/VWindowEditGroup.vue'
import VWindowAddApplicationGroup from '%/components/ApplicationGroups/VWindowAddApplicationGroup.vue'
import VWindowEditApplicationGroup from '%/components/ApplicationGroups/VWindowEditApplicationGroup.vue'
import VWindowAddGroupToStaff from '%/components/Staff/VWindowAddGroupToStaff.vue'
import VWindowAddGroupApplicationTitle from '%/components/GroupApplicationTitles/VWindowAddGroupApplicationTitle.vue'
import VWindowEditGroupApplicationTitle from '%/components/GroupApplicationTitles/VWindowEditGroupApplicationTitle.vue'

@Component({
  components: {
    VWindowEditGroupApplicationTitle,
    VWindowAddGroupApplicationTitle,
    VWindowAddGroupToStaff,
    VWindowEditApplicationGroup,
    VWindowAddApplicationGroup,
    VWindowEditGroup,
    VWindowAddGroup,
    VTableGroups
  }
})
export default class ActivityStaffView extends Vue {
}
</script>
