<template>
  <div>
    <v-settings-component/>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VSettingsComponent from '%/components/VSettingsComponent.vue'

@Component({
  components: { VSettingsComponent }
})
export default class SettingsView extends Vue {
}
</script>
