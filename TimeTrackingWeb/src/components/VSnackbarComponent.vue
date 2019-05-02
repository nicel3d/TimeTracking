<template>
  <v-snackbar
    v-model="snackbar"
    :bottom="y === 'bottom'"
    :left="x === 'left'"
    :multi-line="mode === 'multi-line'"
    :color="color"
    :right="x === 'right'"
    :timeout="timeout"
    :top="y === 'top'"
    :vertical="mode === 'vertical'"
  >
    {{ text }}
    <v-btn
      flat
      @click="snackbar = false"
    >
      Закрыть
    </v-btn>
  </v-snackbar>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator'

@Component
export default class VSnackbarComponent extends Vue {
  snackbar: boolean = true
  y = 'top'
  x = 'right'
  mode = 'multi-line'
  color = 'error'
  timeout: number = 6000
  text: string = 'AAS'
}
</script>

<style scoped>

</style>
