<template>
  <div>
    <v-table-applications/>
    <v-window-edit-application/>
    <v-window-add-application-title/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VTableApplications from '%/components/Applications/VTableApplications.vue'
import VWindowEditApplication from '%/components/Applications/VWindowEditApplication.vue'
import VWindowAddApplicationTitle from '%/components/Applications/VWindowAddApplicationTitle.vue'

@Component({
  components: { VWindowAddApplicationTitle, VWindowEditApplication, VTableApplications }
})
export default class ApplicationsView extends Vue {
}
</script>
