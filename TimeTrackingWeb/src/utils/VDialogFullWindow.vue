<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-layout row justify-center>
    <v-dialog :value="value" fullscreen hide-overlay transition="dialog-bottom-transition">
      <v-card>
        <v-toolbar dark color="primary">
          <v-btn icon dark @click="$emit('input', false)">
            <v-icon>close</v-icon>
          </v-btn>
          <v-toolbar-title>
            <slot name="title"></slot>
          </v-toolbar-title>
          <v-spacer></v-spacer>
          <v-toolbar-items v-if="item">
            <v-btn dark flat @click="$emit('on-save')">Сохранить</v-btn>
          </v-toolbar-items>
          <template v-if="$slots.extension" v-slot:extension>
            <slot name="extension"></slot>
          </template>
        </v-toolbar>
        <v-progress-linear v-if="loading" color="blue" indeterminate></v-progress-linear>
        <slot v-else-if="item"></slot>
        <v-list v-else three-line subheader>
          <v-subheader>Детали по выбранному элементу небыли получены.</v-subheader>
        </v-list>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component
export default class VDialogFullWindow extends Vue {
  @Prop({ default: false }) value!: boolean
  @Prop() item: any = null
  @Prop({ default: false }) loading!: boolean
}
</script>

<style scoped>

</style>
