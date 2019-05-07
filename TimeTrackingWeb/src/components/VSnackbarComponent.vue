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
import { ErrorBase, SwaggerException } from '%/stores/api/SwaggerDocumentationTypescript'
import { oc } from 'ts-optchain'

@Component
export default class VSnackbarComponent extends Vue {
  snackbar: boolean = false
  y = 'top'
  x = 'right'
  mode = 'multi-line'
  color = 'error'
  timeout: number = 6000
  text: string = ''

  mounted () {
    this.$root.$on('snackbar', (error: SwaggerException) => {
      try {
        const errorBase = JSON.parse(error.response) as ErrorBase
        this.text = oc(errorBase).failReason('')
      } catch (e) {
        this.text = error.message
      }

      this.snackbar = true
    })
  }
}
</script>

<style scoped>

</style>
