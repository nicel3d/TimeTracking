<template>
  <div>
    Dashboard
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VActivityTableComponent from '%/components/ActivityStaff/VActivityTableComponent.vue'
import VActivityDetailsComponent from '%/components/ActivityStaff/VActivityDetailsComponent.vue'
import VActivityFilterComponent from '%/components/ActivityStaff/VActivityFilterComponent.vue'
import { FilterRequest } from '%/stores/api/SwaggerDocumentationTypescript'

@Component
export default class DashboardView extends Vue {
}
</script>
