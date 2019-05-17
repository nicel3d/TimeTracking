<template xmlns:v-slot="http://www.w3.org/1999/XSL/Transform">
  <v-dialog
    v-model="modal"
    persistent
    lazy
    full-width
    width="290px"
  >
    <template v-slot:activator="{ on }">
      <v-text-field
        :value="value"
        :label="label"
        prepend-icon="access_time"
        readonly
        v-on="on"
      ></v-text-field>
    </template>
    <v-time-picker
      v-if="modal"
      v-model="timeNew"
      format="24hr"
      full-width>
      <v-spacer></v-spacer>
      <v-btn flat color="primary" @click="modal = false">Закрыть</v-btn>
      <v-btn flat color="primary" @click="onSave">Применить</v-btn>
    </v-time-picker>
  </v-dialog>
</template>

<script lang="ts">
import { Component, Emit, Prop, Vue } from 'vue-property-decorator'

@Component
export default class VTimeDialogTextField extends Vue {
  @Prop() readonly value!: string
  @Prop() readonly label!: string

  timeNew: any = null
  modal: boolean = false

  @Emit('input')
  onSave () {
    this.modal = false
    return this.timeNew
  }
}
</script>

<style scoped>

</style>
