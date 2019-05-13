<template>
  <div>
    <v-table-applications/>
    <v-window-edit-application/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VTableApplications from '%/components/Applications/VTableApplications.vue'
import VWindowEditApplication from '%/components/Applications/VWindowEditApplication.vue'

@Component({
  components: { VWindowEditApplication, VTableApplications }
})
export default class ApplicationsView extends Vue {
}
</script>
