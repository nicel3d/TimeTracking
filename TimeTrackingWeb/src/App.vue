<template>
  <div>
    <v-snackbar-component/>
    <v-fade-transition mode="out-in">
      <router-view/>
    </v-fade-transition>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'
import VSnackbarComponent from '%/components/VSnackbarComponent.vue'

@Component({
  components: { VSnackbarComponent }
})
export default class App extends Vue {
}
</script>
