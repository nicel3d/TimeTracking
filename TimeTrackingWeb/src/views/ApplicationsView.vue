<template>
  <div>
    <v-applications-table-component/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VApplicationsTableComponent from '%/components/VApplicationsTableComponent.vue'

@Component({
  components: { VApplicationsTableComponent }
})
export default class ApplicationsView extends Vue {
}
</script>
