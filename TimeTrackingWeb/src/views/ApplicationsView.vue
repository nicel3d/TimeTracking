<template>
  <div>
    <v-table-applications/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VTableApplications from '%/components/Applications/VTableApplications.vue'

@Component({
  components: { VTableApplications }
})
export default class ApplicationsView extends Vue {
}
</script>
