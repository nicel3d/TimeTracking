<template>
  <v-layout
    row
    justify-center>
    <v-dialog
      v-model="dialog"
      persistent
      max-width="290">
      <v-card>
        <v-card-title class="headline">Вы действительно хотите выйти?</v-card-title>
        <v-card-actions>
          <v-spacer/>
          <v-btn
            color="blue darken-1"
            flat
            @click.native="$emit('update:dialog', false)">Отменить
          </v-btn>
          <v-btn
            color="blue darken-1"
            flat
            @click.native="success">Да
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-layout>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component
export default class VLogoutConfirmDialog extends Vue {
  @Prop({ default: false }) readonly dialog!: boolean

  success () {
    this.$emit('update:dialog', false)
    this.$router.push({ name: 'LayoutUnregistered' })
  }
}
</script>
