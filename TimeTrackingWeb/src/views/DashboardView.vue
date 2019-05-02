<template>
  <div>
    <v-activity-table-component/>
    Dashboard
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VActivityTableComponent from '%/components/VActivityTableComponent.vue'

@Component({
  components: { VActivityTableComponent }
})
export default class DashboardView extends Vue {
}
</script>
